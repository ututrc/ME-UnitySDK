using System;

namespace AR.Geolocation
{
    public static class GeolocationHelper
    {
        //Source: http://stackoverflow.com/questions/6544286/calculate-distance-of-two-geo-points-in-km-c-sharp
        // cos(d) = sin(φА)·sin(φB) + cos(φА)·cos(φB)·cos(λА − λB),
        //  where φА, φB are latitudes and λА, λB are longitudes
        // Distance = d * R
        public static double DistanceBetweenPlaces(GeoLocation from, GeoLocation to)
        {
            double R = 6371; // km
            double sLatFrom = Math.Sin(Radians(from.latitude));
            double sLatTo = Math.Sin(Radians(to.latitude));
            double cLatFrom = Math.Cos(Radians(from.latitude));
            double cLatTo = Math.Cos(Radians(to.latitude));
            double cLon = Math.Cos(Radians(from.longitude) - Radians(to.longitude));
            double cosD = sLatFrom * sLatTo + cLatFrom * cLatTo * cLon;
            double d = Math.Acos(cosD);
            double dist = R * d;
            return dist;
        }

        public static double DistanceBetweenPlaces(double fromLat, double fromLong, double toLat, double toLong)
        {
            GeoLocation from = new GeoLocation(fromLong, fromLat);
            GeoLocation to = new GeoLocation(toLong, toLat);
            return DistanceBetweenPlaces(from, to);
        }

        public static double Radians(double angle)
        {
            return (Math.PI / 180) * angle;
        }

        public static double InterpolateBetween(double srcValue, double srcMin, double srcMax, double dstMin, double dstMax)
        {
            double t = (srcMax - srcValue) / (srcMax - srcMin);
            if (srcMax < srcMin)
            {
                t = 1.0d - t;
            }
            return (1.0d - t) * dstMax + t * dstMin;
        }
    }
}
