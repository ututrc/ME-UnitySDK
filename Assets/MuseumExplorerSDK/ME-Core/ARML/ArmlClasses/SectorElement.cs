using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace ARMLParsing
{

    public class SectorElement
    {
        [XmlAttribute("id")]
        public string id;

        [XmlElement(ElementName = "trackable")]
        public TrackableLink TrackableLink;

		[XmlElement("ArcByCenterPoint", Namespace = "http://www.opengis.net/gml")]
        public ArcByCenterPoint ArcByCenterPoint;

    }
}
