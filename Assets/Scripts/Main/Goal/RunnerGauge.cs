using UnityEngine;

public class RunnerGauge : MonoBehaviour
{
    public float distance = 1.5f;
    public float speed = 0.1f;
    float currentDistance;
    private float targetX;
    private Vector3 startPosition;

    private void Start()
    {
        currentDistance = 0;
        startPosition = transform.position - transform.parent.position;
    }

    private void Update()
    {
        targetX = transform.parent.position.x + startPosition.x + distance;
        // step = speed * Time.deltaTime;
        currentDistance += speed * Time.deltaTime;
        // float newX = Mathf.MoveTowards(transform.position.x, targetX, step);
        // transform.position = tranform new Vector3(newX, transform.position.y, transform.position.z);
        if(currentDistance < distance){
            transform.position = new Vector3(transform.parent.position.x + startPosition.x + currentDistance, transform.parent.position.y + startPosition.y, transform.position.z + startPosition.z);
        }
    }
}