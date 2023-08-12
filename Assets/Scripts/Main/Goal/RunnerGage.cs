using UnityEngine;

public class RunnerGage : MonoBehaviour
{
    public float distance = 0.5f;
    public float speed = 0.1f;
    private float targetX;

    private void Start()
    {
        targetX = transform.position.x + distance;
    }

    private void Update()
    {
        float step = speed * Time.deltaTime;
        // float newX = Mathf.MoveTowards(transform.position.x, targetX, step);
        // transform.position = tranform new Vector3(newX, transform.position.y, transform.position.z);
        if(transform.position.x < targetX){
            transform.position = transform.position + new Vector3(step, 0, 0);
        }
    }
}
