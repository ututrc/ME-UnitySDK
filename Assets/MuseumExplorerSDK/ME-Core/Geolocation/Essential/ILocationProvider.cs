namespace AR.Geolocation
{
    // Sourcetypes. Add more if needed
    public enum SourceType
    {
        gps,
        nimble,
        fake
    }

    // Interface for locationsources
    public interface ILocationProvider
    {
        GpsCoordinates GetLocation();
        string GetZone();
        void StartLocationService();
        void StopLocationService();
        bool IsLocationServiceRunning();
        bool IsLocationServiceEnabled { get; }
        SourceType GetSourceType();
    }
}

