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
        originalPosition = transform.position;
        targetPosition = originalPosition + new Vector3(0, 0, -0.15f);
    }

    // Update is called once per frame
    void Update()
    {
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
        transform.position = originalPosition;
    }
}
