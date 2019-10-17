using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Novel.Common.DB;
using Novel.Common.Models;
using Novel.Common.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Novel.Common.Services
{
    public class NovelService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly RedisCore _redisCore;
        public NovelService(IHttpClientFactory httpClientFactory, IConfiguration configuration, RedisCore redisCore)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _redisCore = redisCore;
        }        
        public async Task<List<NearlyUpdateNovel>> GetNearlyUpdateList()
        {
            List<NearlyUpdateNovel> list = new List<NearlyUpdateNovel>();
            var key = $"homepage_nearlyupdate";
            var redis = _redisCore._redisDB;
            list = redis.GetCache<List<NearlyUpdateNovel>>(key);
            var backKey = "homepage_nearlyupdate_back";
            if (list == null)
            {
                var homeUrl = $"https://www.biquge5200.com";
                var homepage = await GetHtml(homeUrl);
                list = HandleNearlyUpdate(homepage);
                redis.SetCache(key, list, TimeSpan.MaxValue);
                redis.SetCache(backKey, list, new TimeSpan(24,0,0));
            }
            Task task = new Task(async () => { 
                var _list= redis.GetCache<List<NearlyUpdateNovel>>(backKey);
                if (_list == null)
                {
                    var homeUrl = $"https://www.biquge5200.com";
                    var homepage = await GetHtml(homeUrl);
                    list = HandleNearlyUpdate(homepage);
                    redis.SetCache(key, list, TimeSpan.MaxValue);
                    redis.SetCache(backKey, list, new TimeSpan(24, 0, 0));
                }
            });
            task.Start();
            return list;
            
        }
        /// <summary>
        /// 解析最新更新
        /// </summary>
        /// <param name="homepage"></param>
        /// <returns></returns>
        public List<NearlyUpdateNovel> HandleNearlyUpdate(string homepage)
        {
            List<NearlyUpdateNovel> nearlyUpdateNovels = new List<NearlyUpdateNovel>();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(homepage);
            var liNodes = htmlDocument.DocumentNode.SelectNodes("//div[@id='newscontent']//div//ul//li");
            foreach (var liDoc in liNodes)
            {
                NearlyUpdateNovel nearlyUpdateNovel = new NearlyUpdateNovel();
                nearlyUpdateNovel.Tag = liDoc.SelectSingleNode($"{liDoc.XPath}//span[@class='s1']")?.InnerText;
                var titleDoc = liDoc.SelectSingleNode($"{liDoc.XPath}//span[@class='s2']//a");
                if (titleDoc != null)
                {
                    nearlyUpdateNovel.Title = titleDoc.InnerText;
                    nearlyUpdateNovel.Url = titleDoc.GetAttributeValue("href", ""); ;
                }
                var nearlyDoc = liDoc.SelectSingleNode($"{liDoc.XPath}//span[@class='s3']//a");
                if (nearlyDoc != null)
                {
                    nearlyUpdateNovel.NearlyChapter = nearlyDoc.InnerText;
                    nearlyUpdateNovel.NearlyUrl = nearlyDoc.GetAttributeValue("href", "");
                }

                nearlyUpdateNovel.Author = liDoc.SelectSingleNode($"{liDoc.XPath}//span[@class='s4']")?.InnerText;
                nearlyUpdateNovel.UpdateTime = liDoc.SelectSingleNode($"{liDoc.XPath}//span[@class='s5']")?.InnerText;
                nearlyUpdateNovels.Add(nearlyUpdateNovel);
            }
            return nearlyUpdateNovels;
        }

        public async Task<string> GetHtml(string url)
        {
            var client = _httpClientFactory.CreateClient();
            var userAgent = _configuration.GetSection("User_Agents").Get<string[]>();

            Random random = new Random();

            //var url = $"https://www.biquge5200.cc/tongrenxiaoshuo/";
            client.DefaultRequestHeaders.Add("User-Agent", userAgent[random.Next(0, userAgent.Length - 1)]);
            var response = await client.GetAsync(url);
            var str = await response.Content.ReadAsByteArrayAsync();

            string responseString = Encoding.GetEncoding("gbk").GetString(str);
            return responseString;
        }
        public async Task<List<NearlyUpdateNovel>> GetTongRenList()
        {
            var homeUrl = $"https://www.biquge5200.com";
            string url1 = "https://www.biquge5200.cc/xuanhuanxiaoshuo/";
            string url2 = "https://www.biquge5200.cc/xiuzhenxiaoshuo/";
            string url3 = "https://www.biquge5200.cc/dushixiaoshuo/";
            string url4 = "https://www.biquge5200.cc/chuanyuexiaoshuo/";
            string url5 = "https://www.biquge5200.cc/wangyouxiaoshuo/";
            string url6 = "https://www.biquge5200.cc/kehuanxiaoshuo/";
            string url7 = "https://www.biquge5200.cc/yanqingxiaoshuo/";
            string url8 = "https://www.biquge5200.cc/tongrenxiaoshuo/";
            var homepage = await GetHtml(url1);
            return HandleGoodLooking(homepage);
        }
        /// <summary>
        /// 解析最新更新
        /// </summary>
        /// <param name="homepage"></param>
        /// <returns></returns>
        public List<NearlyUpdateNovel> HandleGoodLooking(string homepage)
        {
            List<NearlyUpdateNovel> nearlyUpdateNovels = new List<NearlyUpdateNovel>();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(homepage);
            var liNodes = htmlDocument.DocumentNode.SelectNodes("//div[@class='r']//div//ul//li");
            foreach (var liDoc in liNodes)
            {
                NearlyUpdateNovel nearlyUpdateNovel = new NearlyUpdateNovel();
                nearlyUpdateNovel.Tag = liDoc.SelectSingleNode($"{liDoc.XPath}//span[@class='s1']")?.InnerText;
                var titleDoc = liDoc.SelectSingleNode($"{liDoc.XPath}//span[@class='s2']//a");
                if (titleDoc != null)
                {
                    nearlyUpdateNovel.Title = titleDoc.InnerText;
                    nearlyUpdateNovel.Url = titleDoc.GetAttributeValue("href", ""); ;
                }
                var nearlyDoc = liDoc.SelectSingleNode($"{liDoc.XPath}//span[@class='s3']//a");
                if (nearlyDoc != null)
                {
                    nearlyUpdateNovel.NearlyChapter = nearlyDoc.InnerText;
                    nearlyUpdateNovel.NearlyUrl = nearlyDoc.GetAttributeValue("href", "");
                }

                nearlyUpdateNovel.Author = liDoc.SelectSingleNode($"{liDoc.XPath}//span[@class='s4']")?.InnerText;
                nearlyUpdateNovel.UpdateTime = liDoc.SelectSingleNode($"{liDoc.XPath}//span[@class='s5']")?.InnerText;
                nearlyUpdateNovels.Add(nearlyUpdateNovel);
            }
            return nearlyUpdateNovels;
        }
    }
}
