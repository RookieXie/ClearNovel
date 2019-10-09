using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Novel.Common.Models
{
    public class ChapterContent
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string PreviousUrl { get; set; }
        public string NextUrl { get; set; }
    }
}
