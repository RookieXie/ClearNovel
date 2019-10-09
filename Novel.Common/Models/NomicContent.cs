using System;
using System.Collections.Generic;
using System.Text;

namespace Novel.Common.Models
{
    public class NomicContent
    {
        public List<string> ImgUrls { get; set; }
        public string NextPage { get; set; }
        public string PreviousPage { get; set; }
        public string CatalogUrl { get; set; }
    }
}
