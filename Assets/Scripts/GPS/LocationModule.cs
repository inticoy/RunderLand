using UnityEngine;
using System;
using System.Collections;
using TMPro;

public class LocationModule : MonoBehaviour
{
    public double latitude, longitude, altitude;   
    public Camera arCamera;
    public bool isLocationModuleReady;
    
    private LimitedSizeQueue directionVectorList = new LimitedSizeQueue(10);        
    private Vector3 currPositionMov, prevPositionMov;
    private Vector3 directionVector;            
    private bool isValidMovement;        
    private float dxMov, dyMov, dzMov;
    private Vector3 initDirVec;

    public Vector3 GetDirectionVector()
    {
        return (directionVector);
    }

    public bool GetIsValidMovement()
    {
        return (isValidMovement);
    }

    public void InitializeQueue()
    {
        initDirVec = arCamera.transform.forward;
        initDirVec.y = 0;
        Vector3.Normalize(initDirVec);

        for (int i = 0; i < 10; i++)
        {
            directionVectorList.Enqueue(initDirVec);
        }
        directionVectorList.SetLastVector(initDirVec);
    }

    void Start()
    {
        isLocationModuleReady = false;
        //위치 서비스 초기화
        Input.location.Start(0.1f, 0.1f);
        //StartCoroutine(UpdateGPSData());
        //StartCoroutine(UpdateMovement());
        //위치 서비스 활성화 확인
        if (Input.location.isEnabledByUser)
        {            
            // 위치 서비스 초기화까지 대기
            StartCoroutine(InitializeGPS());
        }
        else
        {            
            Debug.Log("GPS not available");
        }
    }

    IEnumerator InitializeGPS()
    {
        // 위치 서비스 초기화 중일 때까지 대기
        while (Input.location.status == LocationServiceStatus.Initializing)
        {            
            yield return new WaitForSeconds(1);
        }

        // 위치 서비스 초기화가 성공한 경우
        if (Input.location.status == LocationServiceStatus.Running)
        {
            // GPS 데이터 갱신 시작            
            StartCoroutine(UpdateGPSData());
            StartCoroutine(UpdateMovement());
        }
        else
        {
            Debug.Log("Failed to initialize GPS");
        }
    }

    IEnumerator UpdateMovement()
    {        
        prevPositionMov = arCamera.transform.position;

        ///
        float rot = arCamera.transform.rotation.eulerAngles.y;
        float prevRot = rot;
        ///
        while (true)
        {            
            rot = arCamera.transform.rotation.eulerAngles.y;
            currPositionMov = arCamera.transform.position;

            dxMov = currPositionMov.x - prevPositionMov.x;
            dyMov = currPositionMov.y - prevPositionMov.y;
            dzMov = currPositionMov.z - prevPositionMov.z;

            double movement = Math.Sqrt(Math.Pow(dxMov, 2) + Math.Pow(dyMov, 2) + Math.Pow(dzMov, 2));
            //debug.text = Math.Abs(rot - prevRot).ToString();
            if (Math.Abs(rot - prevRot) > 1.3)
                directionVectorList.setArgument(0.6f);
            else
                directionVectorList.setArgument(0.1f);
            if (movement > 0.025)
            {
                isValidMovement = true;
                directionVectorList.Enqueue(new Vector3(dxMov, 0, dzMov));
            }
            else
            {
                isValidMovement = false;               
            }
            prevRot = rot;
                        
            prevPositionMov = currPositionMov;
            directionVector = Vector3.Normalize(directionVectorList.GetFilteredDirectionVector());
            yield return new WaitForSecondsRealtime(0.04f);
        }        
    }


    IEnumerator UpdateGPSData()
    {
        int gps_connect = 0;
        
        while (true)
        {
            // GPS 데이터 업데이트 대기
            yield return new WaitForSecondsRealtime(1);            
            // 현재 GPS 데이터 가져오기
            LocationInfo currentGPSPosition = Input.location.lastData;
            
            // 위도와 경도 텍스트 업데이트
            gps_connect++;

            latitude = currentGPSPosition.latitude;
            longitude = currentGPSPosition.longitude;
            altitude = currentGPSPosition.altitude;

            isLocationModuleReady = true;
        }
    }
}
