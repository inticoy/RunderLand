using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class ControlTowerForRunningAlone : MonoBehaviour
{
    public Toggle avatarToggle;
    public Toggle infoToggle;
    public Toggle effectToggle;
    public Slider slider;
    public Player player;
    public RunningInfo runningInfo;
    public GameObject optionedInfo;
    public AvatarAlone avatarAlone;
    public StateBar stateBar;
    public Camera arCamera;
    public PlayButton playButton;
    public LocationModule locationModule;
    public GPXLogger GPXLogger;
    public TMP_Text timeText;
    public TMP_Text paceText;
    public TMP_Text caloriesText;
    public GameObject pauseIcon;
    public GameObject playIcon2;

    [SerializeField] private Animator avatarAnime;
    private float runningSpeed;
    private bool avatarToggleValue;
    private bool infoToggleValue;
    private bool effectToggleValue;
    private bool isPaused;
    private bool isPauseButtonPressed;
    private float time;
    private float calorieTime;
    private float preTime;

    public void ToggleIsPaused()
    {
        isPaused = !isPaused;
        isPauseButtonPressed = true;
        SetAvatarAnime();
        if (isPaused)
        {
            pauseIcon.SetActive(false);
            playIcon2.SetActive(true);
        }
        else
        {
            pauseIcon.SetActive(true);
            playIcon2.SetActive(false);
        }
    }

    void Start()
    {
        if (PlayerPrefs.GetString("avatar") != "BicycleMan")
            avatarAnime = GameObject.Find(PlayerPrefs.GetString("avatar") + "(Clone)").GetComponent<Animator>();
        
        avatarToggleValue = true;
        infoToggleValue = true;
        effectToggleValue = true;
        isPaused = true;
        isPauseButtonPressed = false;
        time = 0;
        preTime = 0;
    }

    void Update()
    {
        //avatarAnime = GameObject.Find("AnimeMan(Clone)").GetComponent<Animator>();
        CheckOption();
        SetAvatarAnime();        
        if (stateBar.GetIsCountDownEnd())
        {
            if (time == 0)
            {
                isPaused = false;
                player.enabled = true;
                avatarAlone.enabled = true;
                GPXLogger.enabled = true;
                runningInfo.ToggleIsPaused();
                locationModule.InitializeQueue();
            }
            if (!isPaused)
                time += Time.deltaTime;
            if (locationModule.GetIsValidMovement())
                calorieTime += Time.deltaTime;
            if (isPauseButtonPressed)
            {
                player.ToggleIsPaused();
                avatarAlone.ToggleIsPaused();
                runningInfo.ToggleIsPaused();
                isPauseButtonPressed = false;
            }
            timeText.text = GetTimeInFormat(time);
            paceText.text = (player.GetTotalDist() / time).ToString("0.0");
            caloriesText.text = (0.18958333333 * calorieTime).ToString("0.0");
        }
        else if (stateBar.GetIsCountDownGoing())
        {
            if (avatarAnime != null)
            {
                avatarAnime.SetBool("isRun", true);
                avatarAnime.SetBool("isIdle", false);
            }
            preTime += Time.deltaTime;
            avatarAlone.ComeAvatar(preTime);
        }
        else
        {            
            avatarAlone.FixAvatar();
            runningInfo.FixInfo();
        }
    }

    public void SetAvatarAnime()
    {
        if (avatarAnime == null)
            return;
        if (isPaused)
        {
            avatarAnime.SetBool("isIdle", true);
            avatarAnime.SetBool("isRun", false);
            avatarAnime.SetBool("isSprint", false);
        }
        else if (runningSpeed >= 10)
        {
            avatarAnime.SetBool("isIdle", false);
            avatarAnime.SetBool("isRun", false);
            avatarAnime.SetBool("isSprint", true);
        }
        else
        {
            avatarAnime.SetBool("isIdle", false);
            avatarAnime.SetBool("isRun", true);
            avatarAnime.SetBool("isSprint", false);
        }         
    }

    public void CheckOption()
    {
        if (runningSpeed != slider.value)
        {
            runningSpeed = slider.value;
            avatarAlone.SetMovePerFrame(slider.value / 180);
        }
        if (avatarToggleValue != avatarToggle.isOn)
        {
            avatarToggleValue = avatarToggle.isOn;
            if (avatarToggleValue == true)
                arCamera.cullingMask |= 1 << LayerMask.NameToLayer("Avatar");
            else
                arCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Avatar"));
        }
        if (infoToggleValue != infoToggle.isOn)
        {
            infoToggleValue = infoToggle.isOn;
            optionedInfo.SetActive(infoToggleValue);            
        }
        if (effectToggleValue != effectToggle.isOn)
        {
            effectToggleValue = effectToggle.isOn;
            if (effectToggleValue == true)
                arCamera.cullingMask |= 1 << LayerMask.NameToLayer("Effect");
            else
                arCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Effect"));
        }
    }

    public string GetTimeInFormat(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);

        return (timeSpan.Hours + ":" + timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00"));
    }
}