using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ARMLParsing
{

    public class ModelElement
    {

        //[XmlAttribute("id")]
        //public string id;

        //	[XmlAttribute("href")]
        //	public string href;

        [XmlElement(ElementName = "href")]
        public ModelLink ModelLink;

        //[XmlElement(ElementName = "Anchor")]
        //public AnchorLink AnchorLink;

    }

    //public class AnchorLink
    //{

    //    [XmlAttribute("href", Namespace = "http://www.w3.org/1999/xlink", Form = XmlSchemaForm.Qualified)]
    //    public string href;
    //}

    public class ModelLink
    {
        [XmlAttribute("href", Namespace = "http://www.w3.org/1999/xlink", Form = XmlSchemaForm.Qualified)]
        public string href;
    }
}
