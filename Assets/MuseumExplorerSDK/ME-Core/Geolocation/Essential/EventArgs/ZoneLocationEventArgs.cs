using System;

namespace AR.Geolocation
{
    public class ZoneLocationEventArgs : EventArgs
    {
        public String zoneName;
        public SourceType sourceType;

        public ZoneLocationEventArgs(String zoneName, SourceType sourceType)
        {
            this.zoneName = zoneName;
            this.sourceType = sourceType;
        }
    }
}

