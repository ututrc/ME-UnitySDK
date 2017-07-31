using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;
using System.Collections;

namespace ARMLParsing
{
    public class ApplicationElement
    {
        [XmlElement("name")]
        public string name;

        [XmlElement("description")]
        public string description;

        [XmlElement("indoormap")]
        public string indoormap;

    }
}
