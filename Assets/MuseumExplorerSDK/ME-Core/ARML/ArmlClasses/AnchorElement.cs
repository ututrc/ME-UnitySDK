using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace ARMLParsing
{
    public class AnchorElement
    {

        [XmlAttribute("id")]
        public string id;

        [XmlElement(Namespace = "http://www.opengis.net/gml")]
        public PointElement Point;

        [XmlElement("asset")]
		public AssetsElement assetElements;

    }
}
