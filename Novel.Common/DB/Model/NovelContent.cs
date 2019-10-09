using System;
using System.Collections.Generic;
using System.Text;

namespace Novel.Common.DB.Model
{
    public class NovelContent:BaseModel
    {
     public int CatalogId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Order { get; set; }
    }
}
