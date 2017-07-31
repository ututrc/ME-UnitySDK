using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace ARMLParsing
{

    public class ArcByCenterPoint
    {
        [XmlAttribute("numarc")]
        public int numArc;

        [XmlAttribute("id", Namespace = "http://www.opengis.net/gml", Form = XmlSchemaForm.Qualified)]
        public string id;

        [XmlElement(Namespace = "http://www.opengis.net/gml")]
        public float startAngle;

        [XmlElement(Namespace = "http://www.opengis.net/gml")]
        public float endAngle;

        //[XmlAttribute("href", Namespace = "http://www.w3.org/1999/xlink", Form = XmlSchemaForm.Qualified)]
        //public string href;
    }
}