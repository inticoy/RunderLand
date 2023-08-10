using System.Collections;
using System.Collections.Generic;
using Microsoft.Maps.Unity;
using UnityEngine;

public class MapMovement : MonoBehaviour
{
    [SerializeField]
    MapRenderer mapRenderer;

    [SerializeField]
    LocationModuleForMain locationModule;

    // Start is called before the first frame update
    void Start()
    {
        mapRenderer.Center = new Microsoft.Geospatial.LatLon(locationModule.latitude, locationModule.longitude);
    }

    // Update is called once per frame
    void Update()
    {
        mapRenderer.Center = new Microsoft.Geospatial.LatLon(locationModule.latitude, locationModule.longitude);
        //mapRenderer.Center = new Microsoft.Geospatial.LatLon(37.4885, 127.0655);
    }
}
