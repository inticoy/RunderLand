using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationModuleForMain : MonoBehaviour
{
    public double latitude;
    public double longitude;
    public double altitude;
    public bool isLocationModuleReady;

    // Start is called before the first frame update
    void Start()
    {
        isLocationModuleReady = false;
        Input.location.Start(0.1f, 0.1f);

        if (Input.location.isEnabledByUser)
        {
            StartCoroutine(InitializeGPS());
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
        }
        else
        {
            Debug.Log("Failed to initialize GPS");
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
