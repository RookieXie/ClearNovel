using AutoMapper.Configuration;
using Novel.Common.DB;
using Novel.Common.DB.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Novel.Common.Services
{
    public class NovelDBService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly RedisCore _redisCore;
        private readonly NovelDBContext _novelDBContext;
        public NovelDBService(IHttpClientFactory httpClientFactory, IConfiguration configuration, RedisCore redisCore, NovelDBContext novelDBContext)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _redisCore = redisCore;
            _novelDBContext = novelDBContext;
        }

        public void AddBookToShelf(BookShelf book)
        {
            book.CreateTime = DateTime.Now;

        }
    }
}
