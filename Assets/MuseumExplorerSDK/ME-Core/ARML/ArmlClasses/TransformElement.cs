using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace ARMLParsing
{

	public class Vector3Element
	{
		[XmlElement("x")]
		public float x;

		[XmlElement("y")]
		public float y;

		[XmlElement("z")]
		public float z;
	}

	public class TransformElement
	{
		[XmlElement("position")]
		public Vector3Element position;
		[XmlElement("rotation")]
		public Vector3Element rotation;
		[XmlElement("scale")]
		public Vector3Element scale;
	}
}
