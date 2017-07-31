using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;


namespace ARMLParsing
{
	[System.Serializable]
    public class PrefabElement
    {

        [XmlAttribute("id")]
        public string id;

        //[XmlElement(ElementName = "Trackable")]
        //public TrackableLink TrackableLink;

        [XmlElement(ElementName = "href")]
        public PrefabLink PrefabLink;

		[XmlElement]
		public string description;

		[XmlElement]
		public string name;

		[XmlElement]
		public string thumbnail;

    }

	[System.Serializable]
    public class PrefabLink
    {

        [XmlAttribute("href", Namespace = "http://www.w3.org/1999/xlink", Form = XmlSchemaForm.Qualified)]
        public string href;

    }
}
