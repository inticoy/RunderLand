using UnityEngine;

public class RunnerGauge : MonoBehaviour
{
    public float percentage = 1f;

    private Vector3 originalPosition;
    private Vector3 targetPosition;

    public float moveDuration = 0.2f; // Duration of the movement in seconds


    void Start()
    {
        originalPosition = transform.position - transform.parent.position;
    }

    void Update()
    {
        Vector3 rotatedVector = transform.parent.TransformDirection(originalPosition);
        targetPosition = transform.parent.position + rotatedVector + 0.3f * (percentage * 0.8f)* Vector3.Normalize(transform.parent.right);

        float t = Mathf.Clamp01(Time.deltaTime / moveDuration);
        transform.position = Vector3.Lerp(transform.position, targetPosition, t);
    }
}