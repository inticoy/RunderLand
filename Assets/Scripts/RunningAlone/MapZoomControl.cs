using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapZoomControl : MonoBehaviour
{
    public Microsoft.Maps.Unity.MapRenderer mapRender;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ZoomIn()
    {
        Debug.Log(mapRender.ZoomLevel);
        if (mapRender.ZoomLevel < 18)
            mapRender.ZoomLevel += 0.5f;
    }

    public void ZoomOut()
    {
        Debug.Log(mapRender.ZoomLevel);
        if (mapRender.ZoomLevel > 1)
            mapRender.ZoomLevel -= 0.5f;

    }
}
