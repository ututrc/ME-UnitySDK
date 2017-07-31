using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ARMLParsing
{
    public class TrackerElement
    {

        [XmlAttribute("id")]
        public string id;

        [XmlElement("uri")]
        public uri uri;


		[XmlElement("name")]
		public string name;
    }

    public class uri
    {
        [XmlAttribute("href", Namespace = "http://www.w3.org/1999/xlink", Form = XmlSchemaForm.Qualified)]
        public string href;
    }
}