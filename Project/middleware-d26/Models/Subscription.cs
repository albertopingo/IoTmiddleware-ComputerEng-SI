using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace middleware_d26.Models
{
    public class Subscription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        public DateTime? Creation_Dt { get; set; }

        [Required]
        public int? Parent { get; set; }

        [Required]
        [StringLength(255)]
        public string Event { get; set; }

        [Required]
        [StringLength(255)]
        public string Endpoint { get; set; }
    }
}