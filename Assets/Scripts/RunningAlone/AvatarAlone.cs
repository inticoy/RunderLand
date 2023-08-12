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
    private Vector3 avatarFixedLocation;
    private List<double> distanceList;
    private bool isPaused;   
    private float threshold;
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

    public void AfterMove(Vector3 directionVector)
    {
        Vector3 crossProductedVector = Vector3.Normalize(Vector3.Cross(directionVector, Vector3.up));
        float distDiff = Vector3.Distance(transform.position, arCamera.transform.position);

        if (distDiff < 1)
        {
            transform.position += crossProductedVector * (1 - distDiff);
        }
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

    public void FixedUpdate()
    {
        List<Tuple<GPSData, double, Vector3>> route = player.route;
        double playerTotalDist = player.GetTotalDist();

        if (!isPaused)
        {
            avatarTotalDist += movePerFrame;
            float distDiff = Mathf.Clamp((float)(avatarTotalDist - playerTotalDist), -threshold, threshold);

            //avatarDistText.text = distDiff.ToString();

            directionVector = locationModule.GetDirectionVector();
            //avatarDistText.text = directionVector.ToString();
            if (distDiff > 2)
                pos = arCamera.transform.position + directionVector * distDiff;
            else
                pos = arCamera.transform.position + directionVector * distDiff + Vector3.Normalize(Vector3.Cross(directionVector, Vector3.up)) * (2 - distDiff);
            pos.y -= 1.4f;
            transform.position = pos;
            
            //if (locationModule.GetIsValidMovement())
            //{
            //    directionVector = locationModule.GetWeightedVector();

            //    pos = arCamera.transform.position + directionVector * distDiff;              
            //    pos.y -= 1.4f;
            //    transform.position = pos;
            //}
            //// At first
            //if (directionVector == Vector3.zero)
            //{
            //    directionVector = arCamera.transform.forward;
            //    directionVector.y = 0;
            //    Vector3.Normalize(directionVector);                
            //}
            transform.rotation = Quaternion.LookRotation(directionVector);
        }
        //avatarDistText.text = ((float)(avatarTotalDist)).ToString();
    }
}