using System;
using System.Collections.Generic;
using System.Text;

namespace Novel.Common.DB.Model
{
    public class BookCatalog:BaseModel
    {
        public string NovelTitle { get; set; }
        public string ChapterName { get; set; }
        public string ChapterUrl { get; set; }
        public int Order { get; set; }
    }
}
