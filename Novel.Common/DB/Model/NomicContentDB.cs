using System;
using System.Collections.Generic;
using System.Text;

namespace Novel.Common.DB.Model
{
    public class NomicContentDB:BaseModel
    {
        public int CatalogId { get; set; }
        public string ImgUrl { get; set; }
        public int Order { get; set; }
    }
}
