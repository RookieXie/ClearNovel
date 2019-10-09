using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Novel.Common.DB;
using Novel.Common.DB.Model;
using Novel.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Novel.Common.Services
{
    public class TaskGetService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        public TaskGetService(IServiceProvider serviceProvider, IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _mapper = mapper;
           
        }
        public void GetNovels(List<SearchResultContent> searchResultContents)
        {
            var _novelDBContext = (NovelDBContext)_serviceProvider.CreateScope().ServiceProvider.GetService(typeof(NovelDBContext));
            var novelBooks = _mapper.Map<List<SearchResultContent>, List<NovelBook>>(searchResultContents);
            novelBooks = novelBooks.Where(a => !_novelDBContext.NovelBook.Any(b => b.Title == a.Title && a.Author == b.Author)).ToList();
            novelBooks.ForEach(a =>
            {
                a.UserId = "admin";
                a.CreateTime = DateTime.Now;
                a.Author = a.Author;
            });
            _novelDBContext.NovelBook.AddRange(novelBooks);
            _novelDBContext.SaveChanges();
        }
    }
}
