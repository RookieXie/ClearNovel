using System;
using System.Collections.Generic;
using System.Text;

namespace Novel.Common.DB.Model
{
    public class ReadRecord:BaseModel
    {
        /// <summary>
        /// 章节链接
        /// </summary>
        public string ChapterUrl { get; set; }
        public string Chapter { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
    }
}
