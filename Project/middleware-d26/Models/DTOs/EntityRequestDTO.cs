using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace middleware_d26.Models.DTOs
{
    [XmlRoot("EntityRequest", Namespace = "Middleware-d26")]
    public class EntityRequestDTO
    {
        [XmlElement("res_type")]
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
        public string Event { get; set; }

        [XmlElement("endpoint")]
        public string Endpoint { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }
    }

    public class DataDTO
    {
        [XmlElement("content")]
        public string Content { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }
    }
}
