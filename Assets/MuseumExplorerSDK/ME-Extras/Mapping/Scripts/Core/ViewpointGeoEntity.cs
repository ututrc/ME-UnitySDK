using UnityEngine;
using System.Collections;

public class ViewpointGeoEntity : BaseGeoEntity {

    public GameObject MapPresentation;

    public override GameObject GetMapPresentation(PlaneMap map)
    {
        GameObject go = (GameObject)Instantiate(MapPresentation, map.transform.position, map.transform.rotation);
        float scale = (float)map.flatGeo.mapScale;

        go.transform.localScale = new Vector3(scale, scale, scale);

        return go;
    }

    public override GeoEntityType GetGeoEntityType()
    {
        return GeoEntityType.viewpoint;
    }
}
