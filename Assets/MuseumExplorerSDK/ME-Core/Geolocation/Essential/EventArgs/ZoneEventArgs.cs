using System;

namespace AR.Geolocation
{
    public class ZoneEventArgs : EventArgs
    {
        public string ZoneName { get; set; }

        public ZoneEventArgs(string name)
        {
            ZoneName = name;
        }
    }
}
