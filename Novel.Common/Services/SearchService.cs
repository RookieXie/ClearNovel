using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Novel.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Novel.Common.Services
{
    public class SearchService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration configuration;
        public SearchService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            this.httpClientFactory = httpClientFactory;
            this.configuration = configuration;
        }
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="novelName"></param>
        /// <returns></returns>
        public async Task<SearchResult> SearchNovel(string novelName)
        {
            var client = httpClientFactory.CreateClient();
            var userAgent = configuration.GetSection("User_Agents").Get<string[]>();
          
            Random random = new Random();

            var url = $"https://www.biquge5200.com/modules/article/search.php?searchkey={WebUtility.UrlDecode(novelName)}";
            client.DefaultRequestHeaders.Add("User-Agent", userAgent[random.Next(0, userAgent.Length - 1)]);
            var response = await client.GetAsync(url);
            var str = await response.Content.ReadAsByteArrayAsync();

           string  responseString = Encoding.GetEncoding("gbk").GetString(str);

            //return str;
            return HandleSearchHtml(responseString);
        }
        /// <summary>
        /// 解析搜索
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public SearchResult HandleSearchHtml(string html)
        {
            SearchResult searchResult = new SearchResult();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var tableDoc = htmlDocument.DocumentNode.SelectSingleNode("//table");
            var captionDoc = tableDoc.SelectSingleNode("//caption");
            searchResult.Caption = captionDoc.GetDirectInnerText();
            var trDocs = tableDoc.SelectNodes($"{tableDoc.XPath}//tr");
            List<SearchResultContent> searchResultContents = new List<SearchResultContent>();
            foreach (var trDoc in trDocs)
            {
                var tdDocs = trDoc.SelectNodes($"{trDoc.XPath}//td");
                if (tdDocs != null)
                {
                    SearchResultContent searchResultContent = new SearchResultContent()
                    {
                        Title = tdDocs.Where(a => a.FirstChild.Name == "a" && a.GetClasses().FirstOrDefault() == "odd" ).FirstOrDefault()?.FirstChild.InnerHtml,
                        NovelUrl = tdDocs.Where(a => a.FirstChild.Name == "a" && a.GetClasses().FirstOrDefault() == "odd").FirstOrDefault()?.FirstChild.GetAttributeValue("href", ""),
                        Newest = tdDocs.Where(a => a.FirstChild.Name == "a" && a.GetClasses().FirstOrDefault() == "even").FirstOrDefault()?.FirstChild.InnerHtml,
                        NewestUrl= tdDocs.Where(a => a.FirstChild.Name == "a" && a.GetClasses().FirstOrDefault() == "even").FirstOrDefault()?.FirstChild.GetAttributeValue("href", ""),
                        Author = tdDocs.Where(a => a.FirstChild.Name == "#text" && a.GetClasses().FirstOrDefault() == "odd").FirstOrDefault()?.FirstChild.InnerHtml,
                        WordSize = tdDocs.Where(a => a.FirstChild.Name == "#text" && a.GetClasses().FirstOrDefault() == "even").FirstOrDefault()?.FirstChild.InnerHtml,
                        UpdateTime = tdDocs.Where(a => a.FirstChild.Name == "#text" && a.GetClasses().FirstOrDefault() == "odd" && a.Attributes.Any(b => b.Name == "align")).FirstOrDefault()?.FirstChild.InnerHtml,
                        Status = tdDocs.Where(a => a.FirstChild.Name == "#text" && a.GetClasses().FirstOrDefault() == "even" && a.Attributes.Any(b=>b.Name == "align")).FirstOrDefault()?.FirstChild.InnerHtml,
                    };
                    searchResultContents.Add(searchResultContent);
                }
            }
            searchResult.Contents = searchResultContents;
            return searchResult;
        }
        /// <summary>
        /// 请求目录
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<List<Catalog>> GetCatalog(string url)
        {
            var client = httpClientFactory.CreateClient();
            var userAgent = configuration.GetSection("User_Agents").Get<string[]>();
            Random random = new Random();
            client.DefaultRequestHeaders.Add("User-Agent", userAgent[random.Next(0, userAgent.Length - 1)]);
            var response = await client.GetAsync(url);
            var str = await response.Content.ReadAsByteArrayAsync();

            string responseString = Encoding.GetEncoding("gbk").GetString(str);
            return HandleCatalogHtml(responseString);
        }
        /// <summary>
        /// 解析目录
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public List<Catalog> HandleCatalogHtml(string html)
        {
            List<Catalog> catalogs = new List<Catalog>();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var catalodNodes = htmlDocument.DocumentNode.SelectNodes("//div[@id='list']//dl//dd").ToList();
            if (catalodNodes.Count > 9)
            {
                catalodNodes = catalodNodes.Skip(9).ToList();
            }
            else
            {
                catalodNodes = catalodNodes.Skip(catalodNodes.Count/2).ToList();
            }
            foreach (var catalodNode in catalodNodes)
            {
                var chapterNode = catalodNode.SelectSingleNode($"{catalodNode.XPath}//a");
                var chapterName = chapterNode.InnerHtml;
                var chapterUrl = chapterNode.GetAttributeValue("href", "");
                catalogs.Add(new Catalog
                {
                    ChapterName= chapterName,
                    ChapterUrl= chapterUrl
                });
            }
            return catalogs;
        }
        /// <summary>
        /// 请求内容
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<ChapterContent> GetChapterContent(string url)
        {
            var client = httpClientFactory.CreateClient();
            var userAgent = configuration.GetSection("User_Agents").Get<string[]>();
            Random random = new Random();
            client.DefaultRequestHeaders.Add("User-Agent", userAgent[random.Next(0, userAgent.Length - 1)]);
            var response = await client.GetAsync(url);
            var str = await response.Content.ReadAsByteArrayAsync();

            string responseString = Encoding.GetEncoding("gbk").GetString(str);
            return HandleChapterContentHtml(responseString);
        }
        /// <summary>
        /// 解析目录
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public ChapterContent HandleChapterContentHtml(string html)
        {
            ChapterContent chapterContent = new ChapterContent();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var title= htmlDocument.DocumentNode.SelectSingleNode("//div[@class='bookname']//h1");
            var catalodNodes = htmlDocument.DocumentNode.SelectSingleNode("//div[@id='content']");
            var previous_url = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='bottem2']//a[position()=2]");
            var next_url = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='bottem2']//a[position()=4]");
            chapterContent.Title = title.InnerText;
            chapterContent.Content = catalodNodes.InnerText;
            chapterContent.PreviousUrl = previous_url?.GetAttributeValue("href","");
            chapterContent.NextUrl = next_url?.GetAttributeValue("href", "");
            return chapterContent;
        }
    }
}
