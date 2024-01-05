using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace middleware_d26.Models.DTOs
{
    public class SubscriptionDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Endpoint { get; set; }

        [Required]
        public string EventType { get; set; }
    }
}