using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace UserdataParsing
{
    [XmlRoot("xml", Namespace = "http://ar.utu.fi/userdata")]
    public class FeaturesDataElement {

        [XmlElement("features")]
        public UserdataElement1 ude1; 
    }
}
