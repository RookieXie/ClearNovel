using System;
using System.Collections.Generic;
using System.Text;

namespace Novel.Common.DB.Model
{
    public class NomicCatalogDB : BaseModel
    {
        public int NomicId { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
    }
}
