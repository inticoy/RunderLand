using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using TMPro;

public class AvatarWithFriend : MonoBehaviour
{
    public Camera arCamera;
    public LocationModule locationModule;
    public StateBar stateBar;
    public Player player;
    public TMP_Text avatarDistText;
    public TMP_Text distDiffText;
    public GameObject avatarPointer;

    private Vector3 pos;
    private Vector3 directionVector;
    private Vector3 avatarFixedLocation;
    private List<double> distanceList;
    private bool isPaused;
    private int distIdx = 0;
    private float threshold;
    private float distDiff;
    private double avatarTotalDist = 0;
    private double speed;

    public double GetSpeed()
    {
        return (speed);
    }

    public double GetAvatarTotalDist()
    {
        return (avatarTotalDist);
    }

    public void SetDist(double avatarTotalDist)
    {
        this.avatarTotalDist = avatarTotalDist;
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
    }

    public bool IsOutOfRange()
    {
        if (distanceList.Count >= distIdx)
        {
            return (true);
        }
        return (false);
    }


    public void FixedUpdate()
    {
        double playerTotalDist = player.GetTotalDist();

        if (!isPaused)
        {
            distDiff = Mathf.Clamp((float)(avatarTotalDist - playerTotalDist), -threshold, threshold);
            directionVector = locationModule.GetDirectionVector();

            distDiffText.text = distDiff.ToString("0.0");
            Vector3 avatarPointDir = transform.position - avatarPointer.transform.position;
            avatarPointDir.y = 0;            
            avatarPointer.transform.rotation = Quaternion.LookRotation(-avatarPointDir);

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