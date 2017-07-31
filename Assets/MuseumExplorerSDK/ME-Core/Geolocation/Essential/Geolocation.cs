namespace AR.Geolocation
{
    [System.Serializable]
    public struct GeoLocation
    {
        public double longitude;
        public double latitude;
        public float altitude;

        public GeoLocation(double longitude, double latitude, float altitude = 0)
        {
            this.longitude = longitude;
            this.latitude = latitude;
            this.altitude = altitude;
        }
    }
}

