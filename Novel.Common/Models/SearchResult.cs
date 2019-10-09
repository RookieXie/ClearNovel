using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Novel.Common.Models
{
    public class SearchResult
    {
        public string Caption { get; set; }
        public List<SearchResultContent> Contents { get; set; }
    }
    public class SearchResultContent
    {
        public string Title { get; set; }
        public string NovelUrl { get; set; }
        public string Newest { get; set; }
        public string NewestUrl { get; set; }
        public string Author { get; set; }
        public string WordSize { get; set; }
        public string UpdateTime { get; set; }
        public string Status { get; set; }

    }

}
