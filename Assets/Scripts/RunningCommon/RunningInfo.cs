using UnityEngine;
using System;
using TMPro;

public class RunningInfo : MonoBehaviour
{
    [SerializeField] private LocationModule   locationModule;
    [SerializeField] private Camera           arCamera;
    [SerializeField] private StateBar         stateBar;
    [SerializeField] private TMP_Text         timeText;
    [SerializeField] private TMP_Text         paceText;
    [SerializeField] private TMP_Text         caloriesText;
    [SerializeField] private TMP_Text         canvasTimeText;
    [SerializeField] private TMP_Text         canvasPaceText;
    [SerializeField] private TMP_Text         canvasCaloriesText;
    [SerializeField] private GameObject       pauseIcon;
    [SerializeField] private GameObject       playIcon2;
    [SerializeField] private GameObject       canvasPauseIcon;
    [SerializeField] private GameObject       canvasPlayIcon2;

    private Vector3 directionVector;
    private bool    isPaused;

    void Start()
    {
        isPaused = true;
    }

    void Update()
    {
        // Direction vector not changes only when player moves slowly
        if (locationModule.GetIsValidMovement() || isPaused)
        {
            directionVector = arCamera.transform.forward;
            directionVector.y = 0;
            Vector3.Normalize(directionVector);
            transform.position = arCamera.transform.position + directionVector * 2;
            transform.rotation = Quaternion.LookRotation(transform.position - arCamera.transform.position);
        }
    }

    public void InfoUpdate(float time, double playerDist, float calorieTime)
    {
        timeText.text = GetTimeInFormat(time);
        canvasTimeText.text = GetTimeInFormat(time);

        if (playerDist < 0.1)
        {
            paceText.text = "계산 중";
            canvasPaceText.text = paceText.text;
        }
        else
        {
            paceText.text = ((time / playerDist * 1000) / 60).ToString("0") + "' "
                            + ((time / playerDist * 1000) % 60).ToString("0") + '"';
            canvasPaceText.text = paceText.text;
        }
        caloriesText.text = (0.18958333333 * calorieTime).ToString("0.0");
        canvasCaloriesText.text = (0.18958333333 * calorieTime).ToString("0.0") + " kcal";
    }

    public string GetTimeInFormat(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);

        return (timeSpan.Hours + ":" + timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00"));
    }

    public void ToggleIsPaused()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            pauseIcon.SetActive(false);
            playIcon2.SetActive(true);

            canvasPauseIcon.SetActive(false);
            canvasPlayIcon2.SetActive(true);
        }
        else
        {
            pauseIcon.SetActive(true);
            playIcon2.SetActive(false);

            canvasPauseIcon.SetActive(true);
            canvasPlayIcon2.SetActive(false);
        }
    }

    public void FixInfoPanel()
    {
        Vector3 newDirVec = arCamera.transform.forward;
        newDirVec.y = 0;
        Vector3.Normalize(newDirVec);
        Vector3 pos = arCamera.transform.position + newDirVec * 2;
        transform.position = pos;
        Quaternion newRot = Quaternion.Euler(0f, arCamera.transform.rotation.eulerAngles.y, 0f);
        transform.rotation = newRot;
    }
}
