using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace ARMLParsing
{
    public class AssetsElement
    {
        [XmlAttribute("type")]
        public string type;

        [XmlAttribute("id")]
        public string id;

        [XmlElement("href")]
        public AssetLink assetLink;

		[XmlElement("transform")]
		public TransformElement transform;

        //[XmlAttribute("href", Namespace = "http://www.w3.org/1999/xlink", Form = XmlSchemaForm.Qualified)]
        //public string href;

        //[XmlElement("Model")]
        //public List<ModelElement> models = new List<ModelElement>();

        //[XmlElement("Prefab")]
        //public List<PrefabElement> prefabs = new List<PrefabElement>();
    }

    public class AssetLink
    {
        [XmlAttribute("href", Namespace = "http://www.w3.org/1999/xlink", Form = XmlSchemaForm.Qualified)]
        public string href;
    }
}