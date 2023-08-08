using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] Camera arCamera;

    void Start()
    {
        
    }

    void Update()
    {
        //// Make the canvas face the AR camera
        //transform.LookAt(transform.position + arCamera.transform.rotation * Vector3.forward);

        //// Keep the canvas position aligned with the AR camera position
        //transform.position = arCamera.transform.position + Vector3.forward;
        transform.position = arCamera.transform.position;
        transform.rotation = arCamera.transform.rotation;
    }
}
