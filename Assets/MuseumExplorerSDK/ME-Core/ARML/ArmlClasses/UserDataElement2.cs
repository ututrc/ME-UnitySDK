using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;



namespace UserdataParsing{
	
	public class UserdataElement2{

		[XmlAttribute("id")]
		public string id;

        [XmlElement("name")]
        public string name;

		[XmlElement("stories")]
		public List<StoriesElement> stories = new List<StoriesElement>();
		
	}
	
	
}
