using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace ARMLParsing
{
    public class TrackableElement
    {

        [XmlAttribute("id")]
        public string id;

        [XmlElement("config")]
        public config config;

        [XmlElement("preview")]
        public string preview;

        //[XmlElement(ElementName = "ArcByCenterPoint", Namespace = "http://www.opengis.net/gml")]
        //public List<ArcByCenterPointLink> ArcByCenterPointLinks;

    }

    public class ArcByCenterPointLink
    {

        //[XmlAttribute("href", Namespace = "http://www.w3.org/1999/xlink", Form = XmlSchemaForm.Qualified)]
        //public string href;

    }
}

