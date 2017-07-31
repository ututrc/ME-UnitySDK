using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;


namespace ARMLParsing
{
    public class ARElements
    {

        //      [XmlElement("ViewPoint")]
        //public List<ARMLParsing.ViewPointElement> ViewPoints = new List<ARMLParsing.ViewPointElement>();

        //      [XmlElement("Trackable")]
        //public List<TrackableElement> Trackables = new List<ARMLParsing.TrackableElement>();

        //      [XmlElement("Feature")]
        //public List<FeatureElement> Features = new List<ARMLParsing.FeatureElement>();

        [XmlElement("application")]
        public ApplicationElement application;

        [XmlArray("trackers")]
        [XmlArrayItem("tracker")]
        public List<TrackerElement> Trackers;

        [XmlArray("trackables")]
        [XmlArrayItem("trackable")]
        public List<TrackableElement> Trackables;

        [XmlArray("features")]
        [XmlArrayItem("feature")]
        public List<FeatureElement> Features;

        [XmlArray("viewpoints")]
        [XmlArrayItem("viewpoint")]
        public List<ViewPointElement> ViewPoints;
        
        [XmlArray("sectors")]
        [XmlArrayItem("sector")]
        public List<SectorElement> Sectors;

        /*
		[XmlElement("Userdata")]
		public List<UserDataElement1> UserData = new List<ARMLParsing.UserDataElement1>();
        */

        //public ARMLParsing.Tracker Tracker;
    }

    //public class Tracker
    //{
    //    [XmlAttribute("id")]
    //    public string id;

    //    public uri uri;
    //}


}
