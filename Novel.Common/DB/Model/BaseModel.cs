using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Novel.Common.DB.Model
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string UserId { get; set; }
        

    }
}
