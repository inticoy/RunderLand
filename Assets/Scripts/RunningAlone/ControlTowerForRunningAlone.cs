using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class ControlTowerForRunningAlone : MonoBehaviour
{
    [SerializeField] private Toggle avatarToggle;
    [SerializeField] private Toggle infoToggle;
    [SerializeField] private Toggle effectToggle;
    [SerializeField] private Slider slider;
    [SerializeField] private Player player;
    [SerializeField] private RunningInfo runningInfo;
    [SerializeField] private GameObject optionedInfo;
    [SerializeField] private AvatarAlone avatarAlone;
    [SerializeField] private StateBar stateBar;
    [SerializeField] private Camera arCamera;
    [SerializeField] private PlayButton playButton;
    [SerializeField] private LocationModule locationModule;
    [SerializeField] private GPXLogger GPXLogger;
    [SerializeField] private Animator avatarAnime;

    private float   runningSpeed;
    private bool    avatarToggleValue;
    private bool    infoToggleValue;
    private bool    effectToggleValue;
    private bool    isPaused;
    private bool    isStart;
    private float   time;
    private float   preTime;
    private float   calorieTime;


    void Start()
    {
        if (PlayerPrefs.GetString("avatar") != "BicycleMan")
            avatarAnime = GameObject.Find(PlayerPrefs.GetString("avatar") + "(Clone)").GetComponent<Animator>();     
        avatarToggleValue = true;
        infoToggleValue = true;
        effectToggleValue = true;
        isPaused = true;
        isStart = false;
        time = 0;
        preTime = 0;
    }

    void Update()
    {
        CheckOption(); // avatar speed, effect etc.
        SetAvatarAnime(); // According to avatar speed, change animation of avatar.
        if (isStart)
        {
            if (!isPaused)
                time += Time.deltaTime;
            if (locationModule.GetIsValidMovement())
                calorieTime += Time.deltaTime;
            runningInfo.InfoUpdate(time, player.GetTotalDist(), calorieTime);
        }
        else
        {
            ShowReadyScene();
            if (stateBar.GetIsStart())
            {
                isPaused = false;
                isStart = true;
                player.enabled = true;
                avatarAlone.enabled = true;
                GPXLogger.enabled = true;
                runningInfo.ToggleIsPaused();
                locationModule.InitializeQueue();
            }
        }  
    }

    public void PressPauseButton()
    {
        isPaused = !isPaused;
        SetAvatarAnime();
        player.ToggleIsPaused();
        avatarAlone.ToggleIsPaused();
        runningInfo.ToggleIsPaused();
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

    public void ShowReadyScene()
    {
        // Start button pressed, Count down 3, 2, 1
        if (stateBar.GetIsCountDownGoing())
        {
            if (avatarAnime != null)
            {
                avatarAnime.SetBool("isRun", true);
                avatarAnime.SetBool("isIdle", false);
            }
            preTime += Time.deltaTime;
            avatarAlone.ComeAvatar(preTime);
        }
        // Start button not pressed, all component fixed
        else
        {
            avatarAlone.FixAvatar();
            runningInfo.FixInfoPanel();
        }
    }

    public string GetTimeInFormat(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);

        return (timeSpan.Hours + ":" + timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00"));
    }
}