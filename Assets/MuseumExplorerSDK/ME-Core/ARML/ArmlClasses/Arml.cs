using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace ARMLParsing
{
    //Root object of arml deserialization
    [XmlRoot("arml", Namespace = "http://ar.utu.fi/arml")]
    public class Arml
    {
        [XmlElement(ElementName = "arelements")]
        public ARElements aRElements;
        
    }
}

