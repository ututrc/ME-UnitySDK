using System.Xml;
using System.Xml.Serialization;

namespace ARMLParsing
{
    public class PointElement
    {

        //[XmlAttribute("id")]
        //public string id;

        [XmlElement(Namespace = "http://www.opengis.net/gml")]
        public string pos;


        public double[] ParsePos()
        {
            double[] pos = new double[2];
            string[] components = this.pos.Split(' ');

            pos[0] = double.Parse(components[0]);
            pos[1] = double.Parse(components[1]);

            return pos;
        }

    }


}

