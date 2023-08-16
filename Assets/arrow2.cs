using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow2 : MonoBehaviour
{
    public Camera arCamera;
    // Start is called before the first frame update
    void Awake()
    {
        Vector3 dir = arCamera.transform.forward;
        dir.y = 0;
        Vector3.Normalize(dir);
        Quaternion.LookRotation(dir);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
