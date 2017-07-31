using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace ARMLParsing
{
    public class FeatureElement
    {

        [XmlAttribute("id")]
        public string id;

        [XmlElement("name")]
        public string name;

        [XmlElement("description")]
        public string description;

        [XmlElement("anchor")]
        public List<AnchorElement> anchors;

        [XmlArray("trackables")]
        [XmlArrayItem("trackable")]
        public List<TrackableLink> Trackables;

        [XmlArray("assets")]
        [XmlArrayItem("asset")]
        public List<AssetsElement> Assets;

        [XmlArray("annotations")]
        [XmlArrayItem("annotation")]
        public List<AnnotationElement> Annotations;

    }

    public class TrackableLink
    {
        [XmlAttribute("href", Namespace = "http://www.w3.org/1999/xlink", Form = XmlSchemaForm.Qualified)]
        public string href;

		[XmlElement("transform")]
		public TransformElement transform;

    }
}
