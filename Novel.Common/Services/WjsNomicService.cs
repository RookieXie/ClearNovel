using HtmlAgilityPack;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Novel.Common.DB;
using Novel.Common.Models;
using Novel.Common.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Novel.Common.Services
{
    public class WjsNomicService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration configuration;
        private readonly RedisCore _redisCore;
        private readonly IWebHostEnvironment env;
        public WjsNomicService(IHttpClientFactory httpClientFactory, IConfiguration configuration, RedisCore redisCore, IWebHostEnvironment webHostEnvironment)
        {
            this.httpClientFactory = httpClientFactory;
            this.configuration = configuration;
            _redisCore = redisCore;
            env = webHostEnvironment;
        }
        public async Task<List<Banner>> GetBanners()
        {
            var key = $"nomicbanners_wjs";
            var redis = _redisCore._redisDB;
            var list = redis.GetCache<List<Banner>>(key);
            var backKey = "nomicbanners_wjs_back";
            if (list == null)
            {
                var url = $"http://www.weijiaoshou.cn/";
                var client = httpClientFactory.CreateClient();
                var userAgent = configuration.GetSection("User_Agents").Get<string[]>();
                Random random = new Random();
                client.DefaultRequestHeaders.Add("User-Agent", userAgent[random.Next(0, userAgent.Length - 1)]);
                var response = await client.GetAsync(url);
                var str = await response.Content.ReadAsStringAsync();
                list = HandleNomicBanner(str);
                redis.SetCache(key, list, TimeSpan.MaxValue);
                redis.SetCache(backKey, list, new TimeSpan(24, 0, 0));
            }
            Task task = new Task(async () =>
            {
                var _list = redis.GetCache<List<Banner>>(backKey);
                if (_list == null)
                {
                    var url = $"http://www.weijiaoshou.cn/";
                    var client = httpClientFactory.CreateClient();
                    var userAgent = configuration.GetSection("User_Agents").Get<string[]>();
                    Random random = new Random();
                    client.DefaultRequestHeaders.Add("User-Agent", userAgent[random.Next(0, userAgent.Length - 1)]);
                    var response = await client.GetAsync(url);
                    var str = await response.Content.ReadAsStringAsync();
                    list = HandleNomicBanner(str);
                    redis.SetCache(key, list, TimeSpan.MaxValue);
                    redis.SetCache(backKey, list, new TimeSpan(24, 0, 0));
                }
            });
            task.Start();
            return list;

        }
        /// <summary>
        /// 解析目录
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public List<Banner> HandleNomicBanner(string html)
        {
            List<Banner> list = new List<Banner>();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var htmlNodes = htmlDocument.DocumentNode.SelectNodes("//div[@class='list']//a");
            var db = _redisCore._redisDB;
            Banner banner_new = new Banner
            {
                Title = "最新上传"
            };
            list.Add(banner_new);
            foreach (var htmlNode in htmlNodes)
            {
                Banner banner = new Banner();
                var titleNode = htmlNode.SelectSingleNode($"{htmlNode.XPath}//div[@class='fl']");
                var url = $"http://www.weijiaoshou.cn{htmlNode.GetAttributeValue("href", "")}";
                banner.Title = titleNode.InnerText.Trim();
                //nomic.Title = urlNode.InnerText;
                var pageKey = $"{banner.Title}_1";
                db.StringSet(pageKey, url, TimeSpan.MaxValue);
                list.Add(banner);
            }
            var newHtmlNodes = htmlDocument.DocumentNode.SelectNodes("//a[@class='men clearfix']");
            List<Nomic> nomics = new List<Nomic>();
            foreach (var htmlNode in newHtmlNodes)
            {
                Nomic nomic = new Nomic();
                var imgNode = htmlNode.SelectSingleNode($"{htmlNode.XPath}//div[@class='fl men-l']//img");
                var imgUrl = $"http://www.weijiaoshou.cn{imgNode.GetAttributeValue("src", "")}";
                var titleNode = htmlNode.SelectSingleNode($"{htmlNode.XPath}//div[@class='fl men-r']//div[@class='men-r-t']");
                var desNode = htmlNode.SelectSingleNode($"{htmlNode.XPath}//div[@class='fl men-r']//div[@class='men-r-b']");
                nomic.Url = $"http://www.weijiaoshou.cn{htmlNode.GetAttributeValue("href", "")}";
                nomic.Title = titleNode.InnerText.Trim();
                nomic.ImgUrl = imgUrl;
                nomic.Description = desNode.InnerText;
                nomics.Add(nomic);
            }
            var key = $"最新上传_list_1";
            db.SetCache(key, nomics, TimeSpan.MaxValue);
            return list;
        }
        public async Task<List<Nomic>> GetNomics(string title, int pageIndex)
        {
            var redis = _redisCore._redisDB;
            List<Nomic> list = new List<Nomic>();
            var key = $"{title}_list_{pageIndex}";
            if (title == "最新上传")
            {
                key = $"{title}_list_1";
                list = redis.GetCache<List<Nomic>>(key);
            }
            else
            {
                list = redis.GetCache<List<Nomic>>(key);

                var pageKey = $"{title}_{pageIndex}";
                var url = redis.StringGet(pageKey);
                if (list == null)
                {
                    var client = httpClientFactory.CreateClient();
                    var userAgent = configuration.GetSection("User_Agents").Get<string[]>();
                    Random random = new Random();
                    client.DefaultRequestHeaders.Add("User-Agent", userAgent[random.Next(0, userAgent.Length - 1)]);
                    var response = await client.GetAsync(url);
                    var str = await response.Content.ReadAsStringAsync();
                    list = HandleNomic(str, title);
                    redis.SetCache(key, list, TimeSpan.MaxValue);
                }
                var backKey = $"{key}_back_{pageIndex}";
                Task task = new Task(async () =>
                {
                    var _list = redis.GetCache<List<Nomic>>(backKey);
                    if (_list == null)
                    {
                        var client = httpClientFactory.CreateClient();
                        var userAgent = configuration.GetSection("User_Agents").Get<string[]>();
                        Random random = new Random();
                        client.DefaultRequestHeaders.Add("User-Agent", userAgent[random.Next(0, userAgent.Length - 1)]);
                        var response = await client.GetAsync(url);
                        var str = await response.Content.ReadAsStringAsync();
                        list = HandleNomic(str, title);
                        redis.SetCache(key, list, TimeSpan.MaxValue);
                        redis.SetCache(backKey, list, new TimeSpan(24, 0, 0));
                    }
                });
                task.Start();
            }
            return list;

        }
        /// <summary>
        /// 解析目录
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public List<Nomic> HandleNomic(string html, string title)
        {
            var redis = _redisCore._redisDB;
            List<Nomic> list = new List<Nomic>();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var htmlNodes = htmlDocument.DocumentNode.SelectNodes("//div[@class='recommend']//a");
            foreach (var htmlNode in htmlNodes)
            {
                Nomic nomic = new Nomic();
                var imgNode = htmlNode.SelectSingleNode($"{htmlNode.XPath}//div[@class='fl men-l']//img");
                var imgUrl = $"http://www.weijiaoshou.cn{imgNode.GetAttributeValue("src", "")}";
                var titleNode = htmlNode.SelectSingleNode($"{htmlNode.XPath}//div[@class='fl men-r']//div[@class='men-r-t']");
                var desNode = htmlNode.SelectSingleNode($"{htmlNode.XPath}//div[@class='fl men-r']//div[@class='men-r-b']");
                nomic.Url = $"http://www.weijiaoshou.cn{htmlNode.GetAttributeValue("href", "")}";
                nomic.Title = titleNode.InnerText.Trim();
                nomic.ImgUrl = imgUrl;
                nomic.Description = desNode.InnerText;
                list.Add(nomic);
            }
            //分页
            var pageHtmlNodes = htmlDocument.DocumentNode.SelectNodes("//div[@class='turn']//a");
            foreach (var htmlNode in pageHtmlNodes)
            {
                var url = htmlNode.GetAttributeValue("href", "");
                if (!string.IsNullOrEmpty(url))
                {
                    var urls = url.Substring(0, url.Length - 5).Split('-');
                    string pageIndex = urls[urls.Length - 1];
                    var pageKey = $"{title}_{pageIndex}";
                    var realUrl = $"http://www.weijiaoshou.cn{htmlNode.GetAttributeValue("href", "")}";
                    redis.SetCache(pageKey, realUrl, TimeSpan.MaxValue);
                }
            }
            return list;
        }

        public async Task<List<NomicCatalog>> GetcaomicCatalog(string key, string title)
        {
            List<NomicCatalog> nomicCatalogs = new List<NomicCatalog>();
            var redis = _redisCore._redisDB;
            var catalogKey = $"Catalog_{title}";
            var catalogKeyBack = $"Catalog_{title}_back";
            nomicCatalogs = redis.GetCache<List<NomicCatalog>>(catalogKey);
            var list = redis.GetCache<List<Nomic>>(key);


            if (nomicCatalogs == null)
            {
                var nomic = list.Where(a => a.Title == title).FirstOrDefault();
                if (nomic != null)
                {
                    var client = httpClientFactory.CreateClient();
                    var userAgent = configuration.GetSection("User_Agents").Get<string[]>();
                    Random random = new Random();
                    client.DefaultRequestHeaders.Add("User-Agent", userAgent[random.Next(0, userAgent.Length - 1)]);
                    var response = await client.GetAsync(nomic.Url);
                    var str = await response.Content.ReadAsStringAsync();
                    nomicCatalogs = HandleNomicCatalog(str);
                    redis.SetCache(catalogKey, nomicCatalogs, TimeSpan.MaxValue);
                }
            }
            if (list != null)
            {
                var nomic = list.Where(a => a.Title == title).FirstOrDefault();
                if (nomic != null)
                {
                    Task task = new Task(async () =>
                {
                    var _list = redis.GetCache<List<NomicCatalog>>(catalogKeyBack);
                    if (_list == null)
                    {
                        var client = httpClientFactory.CreateClient();
                        var userAgent = configuration.GetSection("User_Agents").Get<string[]>();
                        Random random = new Random();
                        client.DefaultRequestHeaders.Add("User-Agent", userAgent[random.Next(0, userAgent.Length - 1)]);
                        var response = await client.GetAsync(nomic.Url);
                        var str = await response.Content.ReadAsStringAsync();
                        nomicCatalogs = HandleNomicCatalog(str);
                        redis.SetCache(catalogKey, nomicCatalogs, TimeSpan.MaxValue);
                        redis.SetCache(catalogKeyBack, nomicCatalogs, new TimeSpan(24, 0, 0));
                    }
                });
                    task.Start();
                }
            }

            return nomicCatalogs.OrderBy(a => a.Index).ToList();

        }
        /// <summary>
        /// 解析目录
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public List<NomicCatalog> HandleNomicCatalog(string html)
        {
            List<NomicCatalog> list = new List<NomicCatalog>();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var htmlNodes = htmlDocument.DocumentNode.SelectNodes("//div[@class='list']//a");
            int i = 1;
            foreach (var htmlNode in htmlNodes)
            {
                NomicCatalog nomic = new NomicCatalog
                {
                    Url = $"http://www.weijiaoshou.cn{htmlNode.GetAttributeValue("href", "")}"
                };
                //var urlNode = htmlNode.SelectSingleNode($"{htmlNode.XPath}//h2//a");
                //var urls = urlNode.GetAttributeValue("href", "").Split('/');
                var title = WebUtility.HtmlDecode(htmlNode.SelectSingleNode($"{htmlNode.XPath}//div[@class='fl']").InnerText).Trim();
                nomic.Index = i;
                nomic.Title = title;
                list.Add(nomic);
                i++;
            }
            return list;
        }
        /// <summary>
        /// 内容
        /// </summary>
        /// <param name="title"></param>
        /// <param name="catalog"></param>
        /// <returns></returns>
        public async Task<NomicContent> NomicContent(string title, string catalog)
        {
            var redis = _redisCore._redisDB;
            var catalogKey = $"Catalog_{title}";
            var catalogKeyBack = $"Catalog_{title}_back";
            var nomicCatalogs = redis.GetCache<List<NomicCatalog>>(catalogKey);
            NomicContent nomicContent = new NomicContent();
            if (nomicCatalogs != null)
            {
                nomicCatalogs.ForEach(a =>
                {
                    a.Title = a.Title.Trim();
                });
                var nomicCatalog = nomicCatalogs.Where(a => a.Title == catalog).FirstOrDefault();
                if (nomicCatalog != null)
                {
                    var key = $"content_{title}_{nomicCatalog.Title}";
                    nomicContent = redis.GetCache<NomicContent>(key);
                    if (nomicContent == null)
                    {
                        nomicContent = new NomicContent();
                        var client = httpClientFactory.CreateClient();
                        var userAgent = configuration.GetSection("User_Agents").Get<string[]>();
                        Random random = new Random();
                        client.DefaultRequestHeaders.Add("User-Agent", userAgent[random.Next(0, userAgent.Length - 1)]);
                        var response = await client.GetAsync(nomicCatalog.Url);
                        var str = await response.Content.ReadAsStringAsync();
                        nomicContent.Title = nomicCatalog.Title;
                        nomicContent.ImgUrls = await HandleNomicContent(str);
                        nomicContent.PreviousPage = nomicCatalogs.Where(a => a.Index == (nomicCatalog.Index - 1)).Select(b => b.Title).FirstOrDefault();
                        nomicContent.NextPage = nomicCatalogs.Where(a => a.Index == (nomicCatalog.Index + 1)).Select(b => b.Title).FirstOrDefault(); ;
                        nomicContent.CatalogUrl = title;
                        redis.SetCache(key, nomicContent, TimeSpan.MaxValue);
                    }
                    Task task = new Task(() => {
                        if (nomicContent.ImgUrls.Exists(a => a.Contains("http")))
                        {
                            nomicContent.ImgUrls.ForEach(async a => {
                                a = await DownLoad(a);
                            });
                            redis.SetCache(key, nomicContent, TimeSpan.MaxValue);
                        }
                    });
                    task.Start();
                }
            }
            
            return nomicContent;

        }
        /// <summary>
        /// 解析目录
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public async Task<List<string>> HandleNomicContent(string html)
        {
            List<string> list = new List<string>();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var htmlNodes = htmlDocument.DocumentNode.SelectNodes("//div[@class='detail']//p");           
            foreach (var htmlNode in htmlNodes)
            {
                var imgNode = htmlNode.SelectSingleNode($"{htmlNodes[1].XPath}//img");
                var img = $"http://www.weijiaoshou.cn{imgNode.GetAttributeValue("src", "")}";
               
                list.Add(img);
            }
            return list;
        }
        public async Task<string> DownLoad(string imgUrl)
        {
            var uploadPath = "/Image/nomic/";
            var filePath = env.WebRootPath + uploadPath;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            var imgName = Guid.NewGuid().ToString() + ".jpg";
            var webUrl = uploadPath + imgName;
            var headFile = filePath + imgName;
            //获取远程图片到本地
            if (!File.Exists(headFile))
            {
                var client = httpClientFactory.CreateClient();
                var userAgent = configuration.GetSection("User_Agents").Get<string[]>();
                var headBytes = await client.GetByteArrayAsync(imgUrl);
                File.WriteAllBytes(headFile, headBytes);

            }
            return webUrl;
        }
    }
}
