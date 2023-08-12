using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercentageScaler : MonoBehaviour
{
    public float percentage = 1f;

    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private Vector3 targetScale;
   
    public float moveDuration = 0.2f; // Duration of the movement in seconds


    void Start()
    {
        originalPosition = transform.position - transform.parent.position;
    }

    void Update()
    {
        Vector3 rotatedVector = transform.parent.TransformDirection(originalPosition);
        targetPosition = transform.parent.position + rotatedVector + 0.15f * percentage * Vector3.Normalize(transform.parent.right);
        targetScale = new Vector3(percentage, 0.1f, 1f);
        

        float t = Mathf.Clamp01(Time.deltaTime / moveDuration);
        transform.position = Vector3.Lerp(transform.position, targetPosition, t);
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, t);
    }
}
