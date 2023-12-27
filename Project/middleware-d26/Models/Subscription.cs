using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace middleware_d26.Models
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public DateTime? Creation_Dt { get; set; }

        public int? Parent { get; set; }

        [StringLength(50)]
        public string Event { get; set; }

        [StringLength(50)]
        public string Endpoint { get; set; }
    }
}