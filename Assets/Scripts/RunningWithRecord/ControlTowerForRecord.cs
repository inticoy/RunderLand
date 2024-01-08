using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class ControlTowerForRecord: MonoBehaviour
{
    [SerializeField] private Toggle avatarToggle;
    [SerializeField] private Toggle infoToggle;
    [SerializeField] private Toggle effectToggle;
    [SerializeField] private Player player;
    [SerializeField] private RunningInfo runningInfo;
    [SerializeField] private GameObject optionedInfo;
    [SerializeField] private AvatarWithRecord avatarWithRecord;
    [SerializeField] private Animator avatarAnime;
    [SerializeField] private StateBar stateBar;
    [SerializeField] private Camera arCamera;
    [SerializeField] private PlayButton playButton;
    [SerializeField] private LocationModule locationModule;
    [SerializeField] private GPXLogger GPXLogger;

    private bool isStart;
    private bool avatarToggleValue;
    private bool infoToggleValue;
    private bool effectToggleValue;
    private bool isPaused;    
    private float time;
    private float calorieTime;
    private float preTime;

    public void ToggleIsPaused()
    {
        isPaused = !isPaused;        
        SetAvatarAnime();
        player.ToggleIsPaused();
        avatarWithRecord.ToggleIsPaused();
        runningInfo.ToggleIsPaused();
    }

    void Start()
    {
        if (PlayerPrefs.GetString("avatar") != "BicycleMan")
            avatarAnime = GameObject.Find(PlayerPrefs.GetString("avatar") + "(Clone)").GetComponent<Animator>();
        avatarToggleValue = true;
        infoToggleValue = true;
        effectToggleValue = true;
        isPaused = true;        
        time = 0;
        preTime = 0;
    }

    void Update()
    {
        CheckOption();
        SetAvatarAnime();
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
                avatarWithRecord.enabled = true;
                GPXLogger.enabled = true;
                runningInfo.ToggleIsPaused();
                locationModule.InitializeQueue();
            }
        }       
    }

    public void ShowReadyScene()
    {
        if (stateBar.GetIsCountDownGoing())
        {
            if (avatarAnime != null)
            {
                avatarAnime.SetBool("isRun", true);
                avatarAnime.SetBool("isIdle", false);
            }
            preTime += Time.deltaTime;
            avatarWithRecord.ComeAvatar(preTime);
        }
        else
        {
            avatarWithRecord.FixAvatar();
            runningInfo.FixInfoPanel();
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
        else if (avatarWithRecord.GetSpeed() >= 2.77)
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