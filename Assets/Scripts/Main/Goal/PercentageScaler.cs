using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercentageScaler : MonoBehaviour
{
    public float targetScaleX = 0.5f; 
    public float scaleSpeed = 0.1f; 

    
    private Vector3 initialPosition;
    private Vector3 initialScale;
    private Vector3 startPosition;

    
    void Start()
    {
        initialPosition = transform.position;
        initialScale = transform.localScale;
        startPosition = transform.position - transform.parent.position;
    }

    void Update()
    {
        if (transform.localScale.x < targetScaleX)
        {
            float prevScaleX = transform.localScale.x;
            float newScaleX = Mathf.MoveTowards(prevScaleX, targetScaleX, scaleSpeed * Time.deltaTime);
            transform.localScale = new Vector3(newScaleX, transform.localScale.y, transform.localScale.z);
            transform.position = new Vector3(transform.parent.position.x + startPosition.x + (newScaleX / 4.0f), transform.parent.position.y + startPosition.y, transform.position.z + startPosition.z);
        }
    }
}
