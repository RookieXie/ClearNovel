using AutoMapper;
using Novel.Common.DB;
using Novel.Common.DB.Model;
using Novel.Common.Models;
using Novel.Common.Services;
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

        public NomicTaskService(SearchService searchService, NovelDBContext novelDBContext, IMapper mapper)
        {
            _searchService = searchService;
            _novelDBContext = novelDBContext;
            _mapper = mapper;
        }
        public async Task<string> GetNomicToDb()
        {
            string msg = "";
            try
            {
                
                var count = _novelDBContext.Nomic.Count();
                if (count % 10 == 0)
                {
                    msg += "开始";
                    int pageIndex = 1 + (count / 10);
                    List<Nomic> nomics = await _searchService.Getcaomics(pageIndex);
                    List<NomicDB> nomicDBs = _mapper.Map<List<Nomic>, List<NomicDB>>(nomics);
                    nomicDBs = nomicDBs.Where(a => !_novelDBContext.Nomic.Any(b => b.Title == a.Title)).ToList();
                    foreach (var nomic in nomicDBs)
                    {
                        int nomicId = _novelDBContext.Nomic.Add(nomic).Entity.Id;
                        List<NomicCatalog> nomicCatalogs = await _searchService.GetcaomicCatalog(nomic.Url);
                        List<NomicCatalogDB> nomicCatalogDBs = _mapper.Map<List<NomicCatalog>, List<NomicCatalogDB>>(nomicCatalogs);
                        foreach (var nomicCatalog in nomicCatalogDBs)
                        {
                            nomicCatalog.NomicId = nomicId;
                            int nomicCatalogId = _novelDBContext.NomicCatalog.Add(nomicCatalog).Entity.Id;
                            NomicContent nomicContent = await _searchService.NomicContent(nomicCatalog.Url);
                            int i = 0;
                            List<NomicContentDB> nomicContentDBs = new List<NomicContentDB>();
                            foreach (var item in nomicContent.ImgUrls)
                            {
                                nomicContentDBs.Add(new NomicContentDB { ImgUrl = item, Order = i, CatalogId = nomicCatalogId });
                                i++;
                            }
                            _novelDBContext.NomicContent.AddRange(nomicContentDBs);
                        }
                        await _novelDBContext.SaveChangesAsync();
                        Console.WriteLine($"{nomic.Title}成功");
                    }
                }
            }
            catch (Exception e)
            {
                msg = $"{msg} 错误：{e.Message}";
            }
            return msg;
        }
    }
}
