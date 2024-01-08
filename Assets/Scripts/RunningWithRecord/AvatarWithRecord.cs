using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class AvatarWithRecord : MonoBehaviour
{
    [SerializeField] private Camera arCamera;
    [SerializeField] private LocationModule locationModule;
    [SerializeField] private StateBar stateBar;
    [SerializeField] private Player player;
    [SerializeField] private TMP_Text avatarDistText;
    [SerializeField] private TMP_Text distDiffText;
    [SerializeField] private TMP_Text gameEndText;
    [SerializeField] private GameObject avatarPointer;
    [SerializeField] private GameObject lightEffect;

    private int time;
    private Vector3 pos;
    private Vector3 directionVector;
    private Vector3 avatarFixedLocation;
    private List<double> distanceList;
    private bool isPaused;
    private int distIdx = 0;
    private float threshold;
    private float distDiff;
    private double movePerFrame = 0;
    private double avatarTotalDist = 0;
    private double speed;
    private double playerTotalDist;

    public double GetSpeed()
    {
        return (speed);
    }

    public double GetAvatarTotalDist()
    {
        return (avatarTotalDist);
    }

    public void SetMovePerFrame(double movePerFrame)
    {
        this.movePerFrame = movePerFrame;
    }

    public void FixAvatar()
    {
        Vector3 newDirVec = arCamera.transform.forward;
        newDirVec.y = 0;
        Vector3.Normalize(newDirVec);
        Vector3 pos = arCamera.transform.position + newDirVec * 4f;
        pos.y -= 1.4f;
        transform.position = pos;
        Quaternion newRot = Quaternion.Euler(0f, 180f + arCamera.transform.rotation.eulerAngles.y, 0f);
        transform.rotation = newRot;
    }

    public void ComeAvatar(float time)
    {
        if (avatarFixedLocation == Vector3.zero)
            avatarFixedLocation = transform.position;
        Vector3 avatarPosition = Vector3.Lerp(avatarFixedLocation, arCamera.transform.position - new Vector3(0, 1.4f, 0) - 1.5f * arCamera.transform.right, time / 3);
        transform.position = avatarPosition;
    }

    public void ToggleIsPaused()
    {
        isPaused = !isPaused;
    }

    public void Start()
    {
        isPaused = false;
        threshold = 3f;
        directionVector = Vector3.zero;
        avatarFixedLocation = Vector3.zero;

        string filePath = PlayerPrefs.GetString("RecordFile");
        List<GPSData> gpsDataList = GPXReader.ReadGPXFile(filePath);

        distanceList = new List<double>();

        if (gpsDataList != null && gpsDataList.Count > 1)
        {
            for (int idx = 0; idx < gpsDataList.Count - 1; idx++)
            {
                distanceList.Add(GPSUtils.CalculateDistance(gpsDataList[idx], gpsDataList[idx + 1]));
                if (idx == 0)
                    movePerFrame = distanceList[0] * 0.02; 
            }
        }
        else
        {
            // ErrText !!!
            gameEndText.text = "Invalid File";
        }
    }

    public bool IsOutOfRange()
    {
        if (distanceList.Count <= distIdx)
        {
            if (playerTotalDist > avatarTotalDist)
                gameEndText.text = "Record End! You Won!!";
            else
                gameEndText.text = "Record End! You Lose..";
            this.gameObject.SetActive(false);
            lightEffect.SetActive(true);
            return (true);
        }
        return (false);
    }

    public void FixedUpdate()
    {        
        playerTotalDist = player.GetTotalDist();

        if (IsOutOfRange())
            return;
        if (time == 50)
        {
            time = 0;
            distIdx++;
            if (IsOutOfRange())
                return;
            speed = distanceList[distIdx];
            movePerFrame = distanceList[distIdx] * 0.02;
        }

        if (!isPaused)
        {
            avatarTotalDist += movePerFrame;            
            time++;
            distDiff = Mathf.Clamp((float)(avatarTotalDist - playerTotalDist), -threshold, threshold);
            directionVector = locationModule.GetDirectionVector();

            distDiffText.text = (avatarTotalDist - playerTotalDist).ToString("0.0") + "m";
            Vector3 avatarPointDir = transform.position - arCamera.transform.position;
            avatarPointDir.y = 0;
            avatarPointer.transform.rotation = Quaternion.LookRotation(avatarPointDir);

            if (distDiff > 2 || distDiff < -2)
                pos = arCamera.transform.position + directionVector * distDiff;
            else
                pos = arCamera.transform.position + directionVector * distDiff + Vector3.Normalize(Vector3.Cross(directionVector, Vector3.up)) * Math.Abs(2 - distDiff);
            pos.y -= 1.4f;

            transform.position = pos;
            transform.rotation = Quaternion.LookRotation(directionVector);
        }
        avatarDistText.text = ((float)(avatarTotalDist)).ToString();
    }
}