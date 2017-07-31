using System;

namespace AR.Geolocation
{
    public class LocationEventArgs : EventArgs
    {
        public GpsCoordinates gpsCoordinates;
        public SourceType sourceType;

        public LocationEventArgs(GpsCoordinates gpsCoordinates, SourceType sourceType)
        {
            this.gpsCoordinates = gpsCoordinates;
            this.sourceType = sourceType;
        }
    }
}
