using System;
using System.Collections.Generic;
using System.Text;

namespace Novel.Common.DB.Model
{
    public class BookShelf : BaseModel
    {
        public string Title { get; set; }
        /// <summary>
        /// 获取目录链接
        /// </summary>
        public string NovelUrl { get; set; }
    }
}
