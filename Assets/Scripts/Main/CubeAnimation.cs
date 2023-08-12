using UnityEngine;
using UnityEngine.EventSystems;

public class CubeAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;
    public float moveDuration = 0.2f; // Duration of the movement in seconds

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position - transform.parent.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotatedVector = transform.parent.TransformDirection(originalPosition);

        

        targetPosition = transform.parent.position + rotatedVector - 0.15f * Vector3.Normalize(transform.parent.forward);


        if (isMoving)
        {
            float t = Mathf.Clamp01(Time.deltaTime / moveDuration);
            transform.position = Vector3.Lerp(transform.position, targetPosition, t);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMoving = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMoving = false;
        Vector3 rotatedVector = transform.parent.TransformDirection(originalPosition);
        transform.position = transform.parent.position + rotatedVector;
    }
}
