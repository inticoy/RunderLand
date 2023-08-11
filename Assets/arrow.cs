using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : MonoBehaviour
{
    public Camera arCamera;
    public LocationModule locationModule;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(locationModule.GetDirectionVector());

        
    }
}
