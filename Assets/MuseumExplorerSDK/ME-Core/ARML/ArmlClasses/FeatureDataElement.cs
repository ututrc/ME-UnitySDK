using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace UserdataParsing
{
    public class FeatureDataElement
    {

        [XmlAttribute("id")]
        public string id;

        [XmlElement("name")]
        public string name;

        [XmlElement("src")]
        public string src;

        [XmlElement("description")]
        public string description;
    }
}
