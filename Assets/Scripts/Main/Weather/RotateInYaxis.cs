using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateInYaxis : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, 30 * Time.deltaTime, 0);
    }
}
