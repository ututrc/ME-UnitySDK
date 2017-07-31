using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace ARMLParsing

{
    //Methods for serializing and deserializing arml
    public class ArmlMethods
    {
        public static Arml LoadFromFile(string path)
        {
            var serializer = new XmlSerializer(typeof(Arml));
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return serializer.Deserialize(stream) as Arml;
            }
        }

        public static Arml LoadFromStream(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(Arml));
            return serializer.Deserialize(stream) as Arml;
        }

        public static void SaveToStream(Arml arml, Stream stream)
        {
            var serializer = new XmlSerializer(typeof(Arml));
            serializer.Serialize(stream, arml);
        }

        public static Arml LoadFromText(string text)
        {
            var serializer = new XmlSerializer(typeof(Arml));
            return serializer.Deserialize(new StringReader(text)) as Arml;
        }
    }
}
