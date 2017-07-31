using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace UserdataParsing{
    
    public class UserdataElement1{

		[XmlElement("feature")]
		public List<FeatureDataElement> data;

	}


}