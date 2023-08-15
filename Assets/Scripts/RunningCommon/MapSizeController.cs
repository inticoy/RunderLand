using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSizeController : MonoBehaviour
{
    public Microsoft.Maps.Unity.MapRenderer mapRender;
    private bool isExtended = false;
    private float contractedScale = 0.42f;
    private float extendScale = 1.2f;

    public void ChangeSize()
    {
        if (isExtended)
        {
            transform.localScale = new Vector3(contractedScale, contractedScale, contractedScale);
            mapRender.ZoomLevel = 18;
        }
        else
        {
            transform.localScale = new Vector3(extendScale, contractedScale, extendScale);
            mapRender.ZoomLevel = 16;
        }
        isExtended = !isExtended;
    }
}
