using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace middleware_d26.Models
{
    public class Data
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string Content { get; set; }

        public DateTime? Creation_Dt { get; set; }

        public int? Parent { get; set; }
    }
}