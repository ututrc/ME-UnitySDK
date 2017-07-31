using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace ARMLParsing
{
    public class ViewPointElement
    {

        [XmlAttribute("id")]
        public string id;

		[XmlElement("name")]
		public string name;

        [XmlElement("zoneid")]
        public string zoneid;

        [XmlElement(Namespace = "http://www.opengis.net/gml")]
        public PointElement Point;

        [XmlArray("sectors")]
        [XmlArrayItem("sector")]
        public List<SectorLink> SectorLinks;

        //[XmlElement("ArcByCenterPoint", Namespace = "http://www.opengis.net/gml")]
        //public List<ArcByCenterPoint> ArcByCenterPoints;

    }

    public class SectorLink
    {
        [XmlAttribute("href", Namespace = "http://www.w3.org/1999/xlink", Form = XmlSchemaForm.Qualified)]
        public string href;
    }
}


