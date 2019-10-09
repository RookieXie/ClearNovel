using System;
using System.Collections.Generic;
using System.Text;

namespace Novel.Common.DB.Model
{
    public class NovelBook:BaseModel
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
