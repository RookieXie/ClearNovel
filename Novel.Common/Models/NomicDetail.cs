using System;
using System.Collections.Generic;
using System.Text;

namespace Novel.Common.Models
{
    public class NomicDetail
    {
        public string Description { get; set; }
        public List<NomicCatalog> Catalogs { get; set; }
    }
}
