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
    public class NovelService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public NovelService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task<string> GetHomepageHtml()
        {
            var client = _httpClientFactory.CreateClient();
            var userAgent = _configuration.GetSection("User_Agents").Get<string[]>();

            Random random = new Random();

            var url = $"https://www.biquge5200.com";
            client.DefaultRequestHeaders.Add("User-Agent", userAgent[random.Next(0, userAgent.Length - 1)]);
            var response = await client.GetAsync(url);
            var str = await response.Content.ReadAsByteArrayAsync();

            string responseString = Encoding.GetEncoding("gbk").GetString(str);
            return responseString;
        }
        public async Task<List<NearlyUpdateNovel>> GetNearlyUpdateList()
        {
            List<NearlyUpdateNovel> nearlyUpdateNovels = new List<NearlyUpdateNovel>();
            var homepage= await GetHomepageHtml();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(homepage);
            var liNodes = htmlDocument.DocumentNode.SelectNodes("//div[@id='newscontent']//div//ul//li");           
            foreach (var liDoc in liNodes)
            {
                NearlyUpdateNovel nearlyUpdateNovel = new NearlyUpdateNovel();
                nearlyUpdateNovel.Tag = liDoc.SelectSingleNode($"{liDoc.XPath}//span[@class='s1']")?.InnerText;
                var titleDoc=liDoc.SelectSingleNode($"{liDoc.XPath}//span[@class='s2']//a");
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
