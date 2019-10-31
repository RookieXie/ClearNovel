using AutoMapper;
using Novel.Common.DB;
using Novel.Common.DB.Model;
using Novel.Common.Models;
using Novel.Common.Services;
using Novel.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spoondrift.Web.Tasks
{
    public class NomicTaskService
    {
        private readonly SearchService _searchService;
        private readonly IMapper _mapper;
        private readonly NovelDBContext _novelDBContext;
        private readonly RedisCore _redisCore;

        public NomicTaskService(SearchService searchService, NovelDBContext novelDBContext, IMapper mapper, RedisCore redisCore)
        {
            _searchService = searchService;
            _novelDBContext = novelDBContext;
            _mapper = mapper;
            _redisCore = redisCore;
        }
        public async Task<string> GetNomicToDb()
        {
            string msg = "";
            var redis = _redisCore._redisDB;
            var key = "Nomic_PageIndex";
            try
            {
                int pageIndex = 1;
                var value = redis.GetCache(key);
                if (!string.IsNullOrEmpty(value))
                {
                    int.TryParse(value, out pageIndex);
                }
                if (pageIndex != -1)
                {
                    redis.SetCache(key, pageIndex + 1, TimeSpan.MaxValue);
                    msg += "开始";
                    List<Nomic> nomics = await _searchService.GetNomics(pageIndex);
                    List<NomicDB> nomicDBs = _mapper.Map<List<Nomic>, List<NomicDB>>(nomics);
                    nomicDBs = nomicDBs.Where(a => !_novelDBContext.Nomic.Any(b => b.Title == a.Title)).ToList();

                    foreach (var nomic in nomicDBs)
                    {
                        using (var trans = _novelDBContext.Database.BeginTransaction())
                        {
                            try
                            {
                                _novelDBContext.Nomic.Add(nomic);
                                await _novelDBContext.SaveChangesAsync();
                                List<NomicCatalog> nomicCatalogs = await _searchService.GetcaomicCatalog(nomic.Url);
                                List<NomicCatalogDB> nomicCatalogDBs = _mapper.Map<List<NomicCatalog>, List<NomicCatalogDB>>(nomicCatalogs);
                                foreach (var nomicCatalog in nomicCatalogDBs)
                                {
                                    nomicCatalog.NomicId = nomic.Id;
                                    _novelDBContext.NomicCatalog.Add(nomicCatalog);
                                    await _novelDBContext.SaveChangesAsync();
                                    NomicContent nomicContent = await _searchService.NomicContent(nomicCatalog.Url);
                                    int i = 0;
                                    List<NomicContentDB> nomicContentDBs = new List<NomicContentDB>();
                                    foreach (var item in nomicContent.ImgUrls)
                                    {
                                        nomicContentDBs.Add(new NomicContentDB { ImgUrl = item, Order = i, CatalogId = nomicCatalog.Id });
                                        i++;
                                    }
                                    _novelDBContext.NomicContent.AddRange(nomicContentDBs);
                                    await _novelDBContext.SaveChangesAsync();
                                }
                                trans.Commit();
                                msg = $"msg {nomic.Title} 成功";
                            }
                            catch (Exception e)
                            {
                                trans.Rollback();
                                throw e;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                msg = $"{msg} 错误：{e.Message}";
                redis.SetCache(key, -1, TimeSpan.MaxValue);
            }
            return msg;
        }
    }
}
