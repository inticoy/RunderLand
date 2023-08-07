using UnityEngine;

public class RunningInfo : MonoBehaviour
{
    public LocationModule   locationModule;
    public Camera           arCamera;
    public StateBar         stateBar;

    private Vector3         directionVector;
    private Vector3         dirVectAtPause;
    private bool            isPaused;

    void Start()
    {
        isPaused = true;
        GetDirVectAtPause();
    }

    void Update()
    {
        if (!stateBar.GetIsCountDownEnd())
            return;
        if (isPaused)
        {
            transform.position = arCamera.transform.position + dirVectAtPause * 2;
            transform.rotation = Quaternion.LookRotation(transform.position - arCamera.transform.position);
        }
        else
        {
            if (locationModule.GetIsValidMovement())
            {
                transform.position = arCamera.transform.position + arCamera.transform.forward * 2;
                transform.rotation = Quaternion.LookRotation(transform.position - arCamera.transform.position);
            }
            else
            {
                directionVector = locationModule.GetWeightedVector();
                transform.position = arCamera.transform.position + directionVector * 2;
                transform.rotation = Quaternion.LookRotation(transform.position - arCamera.transform.position);
            }
        }
    }

    private void GetDirVectAtPause()
    {
        dirVectAtPause = arCamera.transform.forward;
        dirVectAtPause.y = 0;
        Vector3.Normalize(dirVectAtPause);
    }

    public void ToggleIsPaused()
    {
        GetDirVectAtPause();
        isPaused = !isPaused;
    }

    public void FixInfo()
    {
        Vector3 newDirVec = arCamera.transform.forward;
        newDirVec.y = 0;
        Vector3.Normalize(newDirVec);
        Vector3 pos = arCamera.transform.position + newDirVec;
        transform.position = pos;
        Quaternion newRot = Quaternion.Euler(0f, arCamera.transform.rotation.eulerAngles.y, 0f);
        transform.rotation = newRot;
    }
}
