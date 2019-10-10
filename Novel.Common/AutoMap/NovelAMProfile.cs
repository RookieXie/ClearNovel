
using AutoMapper;
using Novel.Common.DB.Model;
using Novel.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Novel.Common.AutoMap
{
    public class NovelAMProfile:Profile
    {
        public NovelAMProfile()
        {
            CreateMap<SearchResultContent, NovelBook>();
            CreateMap<NovelBook, SearchResultContent>();

            CreateMap<Nomic, NomicDB>();
            CreateMap<NomicDB, Nomic>();

            CreateMap<NomicCatalog, NomicCatalogDB>();
            CreateMap<NomicCatalogDB, NomicCatalog>();
        }
    }
}
