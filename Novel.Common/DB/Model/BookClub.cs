using System;
using System.Collections.Generic;
using System.Text;

namespace Novel.Common.DB.Model
{
    public class BookClub:BaseModel
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public int UserLevel { get; set; }
    }
}
