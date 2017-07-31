using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace ARMLParsing
{
    public class AnnotationElement
    {
        [XmlAttribute("number")]
        public string id;

        [XmlElement("name")]
        public string name;

        [XmlElement("description")]
        public string description;

        [XmlElement("position")]
        public string position;

        public float[] ParsePos()
        {
            float[] pos = new float[3];
			if (this.position != null) {
				string[] components = this.position.Split (',');

				if (pos.Length > 2) {
					pos [0] = float.Parse (components [0]);
					pos [1] = float.Parse (components [1]);
					pos [2] = float.Parse (components [2]);
				}
			}
            return pos;
        }
    }
}
