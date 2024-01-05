using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace middleware_d26.Models.DTOs
{
    [XmlRoot("CreateDTO")]
    public class CreateDTO
    {
        [XmlElement("res_type")]
        [Required]
        public string ResType { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("subscription")]
        public SubscriptionDTO Subscription { get; set; }

        [XmlElement("data")]
        public DataDTO Data { get; set; }
    }

    public class SubscriptionDTO
    {
        [XmlElement("event")]
        [Required]
        public string Event { get; set; }

        [XmlElement("endpoint")]
        [Required]
        public string Endpoint { get; set; }

        [XmlElement("name")]
        [Required]
        public string Name { get; set; }
    }

    public class DataDTO
    {
        [XmlElement("content")]
        [Required]
        public string Content { get; set; }

        [XmlElement("name")]
        [Required]
        public string Name { get; set; }
    }
}
