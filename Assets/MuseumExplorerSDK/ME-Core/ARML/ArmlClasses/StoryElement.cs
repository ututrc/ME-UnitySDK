using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace UserdataParsing
{
	public class StoryElement {


		[XmlAttribute("id")]
		public string id;

		[XmlElement("username")]
		public string username;

		[XmlElement("text")]
		public string text;

	}
}
