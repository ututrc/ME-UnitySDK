using UnityEngine;
using System;
using System.Collections;

//Converter takes two pairs of corresponding points in cartesian 2d-coordinate systems and solves equations for transformations.
//Basic theory for 2d-mapping and distance calculation is based on procedure to map between two 2d-coordinate systems using linear equations:
//https://msdn.microsoft.com/en-us/library/jj635757(v=vs.85).aspx

public class Converter2D {

    private Matrix4x4 traMatrix;
    private Vector4 constants;

    public Converter2D(Vector2 org1, Vector2 org2, Vector2 tra1, Vector2 tra2) {

        traMatrix = new Matrix4x4();
        traMatrix.SetRow(0, new Vector4((float)org1.x, (float)org1.y, 1, 0));
        traMatrix.SetRow(1, new Vector4((float)org1.y, -(float)org1.x, 0, 1));
        traMatrix.SetRow(2, new Vector4((float)org2.x, (float)org2.y, 1, 0));
        traMatrix.SetRow(3, new Vector4((float)org2.y, -(float)org2.x, 0, 1));

        Vector4 points = new Vector4((float)tra1.x, (float)tra1.y, (float)tra2.x, (float)tra2.y);

        Matrix4x4 invMat = traMatrix.inverse;

        constants = invMat * points;
    }

    public Vector2 getOrgToTra(Vector2 org)
    {
        //Linear equations for mapping points
        float transformedX = (constants.x * org.x) + (constants.y * org.y) + constants.z;
        float transformedY = -(constants.y * org.x) + (constants.x * org.y) + constants.w;

        return new Vector2(transformedX, transformedY);
    }

    public Vector2 getTraToOrg(Vector2 tra)
    {
        //solving system of equations with two unknown gives these equations
        double orgX = ((constants.x * tra.x) - (constants.y * tra.y) + (constants.y * constants.w) - (constants.x * constants.z)) / (Math.Pow(constants.x, 2) + Math.Pow(constants.y, 2));
        double orgY = ((constants.y * tra.x) + (constants.x * tra.y) - (constants.x * constants.w) - (constants.y * constants.z)) / (Math.Pow(constants.x, 2) + Math.Pow(constants.y, 2));
        
        return new Vector2((float)orgX, (float)orgY);
    }
}
