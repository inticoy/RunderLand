using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixCycle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(0, -0.4f, 0);
        transform.rotation = Quaternion.Euler(0, 180, 0);
    }
}
