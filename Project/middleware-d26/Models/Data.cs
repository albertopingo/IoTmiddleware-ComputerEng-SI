using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace middleware_d26.Models
{
    [XmlRoot("data")]
    public class Data
    {
        [XmlElement("id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [XmlElement("name")]
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [XmlElement("content")]
        [Required]
        [StringLength(255)]
        public string Content { get; set; }

        [XmlElement("creation_dt")]
        [Required]
        public DateTime? Creation_Dt { get; set; }

        [XmlElement("parent")]
        [Required]
        public int? Parent { get; set; }
    }
}
