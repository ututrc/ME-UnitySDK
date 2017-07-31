using UnityEngine;
using System;
using System.Collections;

//FlatGeo takes two points in local 2d-coordinate system and two corresponging gps-points and can then map other gps-points to given coordinate system.
//Projection to plane is done using equirectangular projection with standard parallels at given geopoint1.
//FlatGeos constructor first calculates distance between gps-locations in dX and dY and forms world scale 2d-coordinate system where y-axis is facing north.
//Gps-point1 is placed in 0,0 and gps-point2 in x=dx and y=dy. Two set of 2d-points is then inserted to Converter2D and it gives constants for linear equations.
//Constans can be used to map other gps-points to local 2d-coordinate system points or vice versa. 

//Calculating cartesian x- and y-components of gps-points in plane map
//http://stackoverflow.com/questions/2839533/adding-distance-to-a-gps-coordinate
//https://en.wikipedia.org/wiki/Equirectangular_projection

public struct Gps
{
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }

    public Gps(double latitude, double longitude)
    {
        this.Latitude = latitude;
        this.Longitude = longitude;
    }
}

public class FlatGeo {

    readonly static double RADIUS = 6378137;

    private Func<double, double> GetDy;
    private Func<double, double> GetDx;

    private Func<double, double> GetLat;
    private Func<double, double> GetLon;

    private double preCalc1;
    private double preCalc2;

    private Converter2D converter;

    public double mapScale { get; private set;}
    public double northRot { get; private set;}
    
    public FlatGeo(Gps geoPoint1, Gps geoPoint2, Vector2 point1, Vector2 point2) {

        preCalc1 = (Mathf.PI / 180) * RADIUS;
        preCalc2 = ((Mathf.PI / 180) * RADIUS) * Math.Cos(Math.PI / 180.0 * geoPoint1.Latitude);

        GetDy = (lat) => { return (lat - geoPoint1.Latitude) * preCalc1; };
        GetDx = (lon) => { return (lon - geoPoint1.Longitude) * preCalc2; };

        GetLat = (dY) => { return (dY / preCalc1) + geoPoint1.Latitude; };
        GetLon = (dX) => { return (dX / preCalc2) + geoPoint1.Longitude; };

        float point2Y = (float)GetDy(geoPoint2.Latitude);
        float point2X = (float)GetDx(geoPoint2.Longitude);
        
        converter = new Converter2D(new Vector2(0, 0), new Vector2(point2X, point2Y), point1, point2);

        mapScale = Vector2.Distance(new Vector2((float)point1.x, (float)point1.y), new Vector2((float)point2.x, (float)point2.y)) / Vector2.Distance(new Vector2(0,0), new Vector2((float)point2X, (float)point2Y));
        northRot = Vector2.Angle( new Vector2(point2X, point2Y), new Vector2((point2.x - point1.x), (point2.y - point1.y)));

        northRot= northRot* Mathf.Sign(Vector3.Cross(new Vector2(point2X, point2Y), new Vector2((point2.x - point1.x), (point2.y - point1.y))).z);
    }

    public Vector2 getPoint(Gps geoPoint)
    {
        float orgY = (float)GetDy(geoPoint.Latitude);
        float orgX = (float)GetDx(geoPoint.Longitude);

        Vector2 point = converter.getOrgToTra(new Vector2(orgX, orgY));
        return point;
    }

    public Gps getGeolocation(Vector2 point)
    {
        Vector2 realPoint = converter.getTraToOrg(point);

        double lat = GetLat(realPoint.y);
        double lon = GetLon(realPoint.x);

        return new Gps(lat, lon);
    }
}
