using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Novel.Common.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Novel.Common.Services
{
   public class WJSNomicService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration configuration;
        public WJSNomicService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            this.httpClientFactory = httpClientFactory;
            this.configuration = configuration;
        }
        //http://www.weijiaoshou.cn/
        //https://m.mhzhan.com/  亲亲漫画
        //https://mymhh.com/chapter/23940
        //http://www.lelehanman.com/chapter/30519
        public async Task<List<Nomic>> GetNomics(int pageIndex)
        {
            var url = $"http://www.weijiaoshou.cn/";
            var client = httpClientFactory.CreateClient();
            var userAgent = configuration.GetSection("User_Agents").Get<string[]>();
            Random random = new Random();
            client.DefaultRequestHeaders.Add("User-Agent", userAgent[random.Next(0, userAgent.Length - 1)]);
            var response = await client.GetAsync(url);
            var str = await response.Content.ReadAsStringAsync();
            return HandleNomic(str);

        }
        /// <summary>
        /// 解析目录
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public List<Nomic> HandleNomic(string html)
        {
            List<Nomic> list = new List<Nomic>();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var htmlNodes = htmlDocument.DocumentNode.SelectNodes("//recommend");
            foreach (var htmlNode in htmlNodes)
            {
                Nomic nomic = new Nomic();
                var urlNode = htmlNode.SelectSingleNode($"{htmlNode.XPath}//h2//a");
                var urls = urlNode.GetAttributeValue("href", "").Split('/');
                nomic.Url = urls[urls.Length - 1].Split('.')[0];
                nomic.Title = urlNode.InnerText;
                list.Add(nomic);
            }
            return list;
        }
        public async Task<List<NomicCatalog>> GetcaomicCatalog(string urlHtml)
        {
            var url = $"https://www.jjhanman.com/{urlHtml}.html";
            var client = httpClientFactory.CreateClient();
            var userAgent = configuration.GetSection("User_Agents").Get<string[]>();
            Random random = new Random();
            client.DefaultRequestHeaders.Add("User-Agent", userAgent[random.Next(0, userAgent.Length - 1)]);
            var response = await client.GetAsync(url);
            var str = await response.Content.ReadAsStringAsync();
            return HandleNomicCatalog(str, urlHtml);

        }
        /// <summary>
        /// 解析目录
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public List<NomicCatalog> HandleNomicCatalog(string html, string url)
        {
            List<NomicCatalog> list = new List<NomicCatalog>();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var htmlNodes = htmlDocument.DocumentNode.SelectNodes("//div[@class='article-paging']//a");
            foreach (var htmlNode in htmlNodes)
            {
                NomicCatalog nomic = new NomicCatalog();
                //nomic.Url = htmlNode.GetAttributeValue("href", "");  
                //var urlNode = htmlNode.SelectSingleNode($"{htmlNode.XPath}//h2//a");
                //var urls = urlNode.GetAttributeValue("href", "").Split('/');
                nomic.Url = url;
                var titleNode = htmlNode.SelectSingleNode($"{htmlNode.XPath}//span");
                nomic.Index = Convert.ToInt32(titleNode.InnerText);
                nomic.Title = $"第{nomic.Index - 1}话";
                list.Add(nomic);
            }
            return list;
        }
        public async Task<NomicContent> NomicContent(string urlHtml, int index)
        {
            var url = $"https://www.jjhanman.com/{urlHtml}.html/{index}";
            var client = httpClientFactory.CreateClient();
            var userAgent = configuration.GetSection("User_Agents").Get<string[]>();
            Random random = new Random();
            client.DefaultRequestHeaders.Add("User-Agent", userAgent[random.Next(0, userAgent.Length - 1)]);
            var response = await client.GetAsync(url);
            var str = await response.Content.ReadAsStringAsync();

            var nomicContent = HandleNomicContent(str);
            nomicContent.PreviousPage = (index - 1).ToString();
            nomicContent.NextPage = (index + 1).ToString();
            nomicContent.CatalogUrl = urlHtml;
            return nomicContent;

        }
        /// <summary>
        /// 解析目录
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public NomicContent HandleNomicContent(string html)
        {
            NomicContent nomicContent = new NomicContent();
            List<string> list = new List<string>();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var htmlNodes = htmlDocument.DocumentNode.SelectNodes("//article[@class='article-content']//p");
            nomicContent.Title = htmlNodes[0].InnerText;
            var imgNodes = htmlNodes[1].SelectNodes($"{htmlNodes[1].XPath}//img");
            foreach (var imgNode in imgNodes)
            {
                var img = imgNode.GetAttributeValue("src", "");

                list.Add(img);
            }
            nomicContent.ImgUrls = list;
            return nomicContent;
        }
    }
}
