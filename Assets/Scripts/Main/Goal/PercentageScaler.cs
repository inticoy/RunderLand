using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercentageScaler : MonoBehaviour
{
    public float targetScaleX = 0.5f; 
    public float scaleSpeed = 0.1f; 

    
    private Vector3 initialPosition;
    private Vector3 initialScale;

    
    void Start()
    {
        initialPosition = transform.position;
        initialScale = transform.localScale;
    }

    void Update()
    {
        if (transform.localScale.x < targetScaleX)
        {
            float prevScaleX = transform.localScale.x;
            float newScaleX = Mathf.MoveTowards(prevScaleX, targetScaleX, scaleSpeed * Time.deltaTime);
            transform.localScale = new Vector3(newScaleX, transform.localScale.y, transform.localScale.z);
            transform.position = new Vector3(initialPosition.x + (newScaleX / 4.0f), transform.position.y, transform.position.z);
        }
    }
}
