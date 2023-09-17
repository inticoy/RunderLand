using UnityEngine;

public class FixCycle2 : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(0, -0.4f, 0);
        transform.localRotation = Quaternion.Euler(0, 180, 0);
    }
}
