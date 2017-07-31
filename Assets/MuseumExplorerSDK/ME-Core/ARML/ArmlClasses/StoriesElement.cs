using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;



namespace UserdataParsing{
	
	public class StoriesElement{

		[XmlElement("story")]
		public List<StoryElement> story = new List<StoryElement>();
		
	}
	
	
}
