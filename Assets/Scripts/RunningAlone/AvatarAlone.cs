using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using TMPro;

public class AvatarAlone : MonoBehaviour
{
    public Camera arCamera;    
    public LocationModule locationModule;
    public StateBar stateBar;
    public Player player;
    public TMP_Text avatarDistText;   

    private Vector3 pos;
    private Vector3 directionVector;
    private List<double> distanceList;
    private bool isPaused;   
    private double movePerFrame = 0;
    private double avatarTotalDist = 0;

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
        Vector3 pos = arCamera.transform.position + newDirVec * 1.5f;        
        pos.y -= 1.4f;
        transform.position = pos;        
        Quaternion newRot = Quaternion.Euler(0f, arCamera.transform.rotation.eulerAngles.y, 0f);
        transform.rotation = newRot;
    }

    public void ToggleIsPaused()
    {
        isPaused = !isPaused;
    }

    public void Start()
    {
        ///
        movePerFrame = 0.1f;
        ///
        isPaused = true;
        directionVector = Vector3.zero;
    }

    public void FixedUpdate()
    {
        List<Tuple<GPSData, double, Vector3>> route = player.route;
        double playerTotalDist = player.GetTotalDist();

        if (!isPaused)
        {
            avatarTotalDist += movePerFrame;
            float distDiff = Mathf.Clamp((float)(avatarTotalDist - playerTotalDist), -2, 2);

            if (locationModule.GetIsValidMovement())
            {
                directionVector = locationModule.GetWeightedVector();

                pos = arCamera.transform.position + directionVector * distDiff;
                pos.y -= 1.4f;
                transform.position = pos;
            }
            if (directionVector == Vector3.zero)
            {
                directionVector = arCamera.transform.forward;
                directionVector.y = 0;
                Vector3.Normalize(directionVector);                
            }
            transform.rotation = Quaternion.LookRotation(directionVector);
        }
        avatarDistText.text = ((float)(avatarTotalDist)).ToString();
    }
}