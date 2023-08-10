using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using Geocoding;

namespace Geocoding
{
    [System.Serializable]
    public class Center
    {
        public string crs;
        public float x;
        public float y;
    }

    [System.Serializable]
    public class Area0
    {
        public string name;
        public Center coords;
    }

    [System.Serializable]
    public class Area1
    {
        public string name;
        public Center coords;
        public string alias;
    }

    [System.Serializable]
    public class Area2
    {
        public string name;
        public Center coords;
    }

    [System.Serializable]
    public class Area3
    {
        public string name;
        public Center coords;
    }

    [System.Serializable]
    public class Area4
    {
        public string name;
        public Center coords;
    }

    [System.Serializable]
    public class Region
    {
        public Area0 area0;
        public Area1 area1;
        public Area2 area2;
        public Area3 area3;
        public Area4 area4;
    }

    [System.Serializable]
    public class Code
    {
        public string id;
        public string type;
        public string mappingId;
    }

    [System.Serializable]
    public class Result
    {
        public string name;
        public Code code;
        public Region region;
    }

    [System.Serializable]
    public class Status
    {
        public int code;
        public string name;
        public string message;
    }

    [System.Serializable]
    public class Root
    {
        public Status status;
        public List<Result> results;
    }

}

public class NaverReverseGeocoding : MonoBehaviour
{
    [SerializeField]
    TMP_Text area1Text;

    [SerializeField]
    TMP_Text area2Text;

    [SerializeField]
    LocationModuleForMain location;

    private Root response;
    private int status_code = -1;


    private string url = "https://naveropenapi.apigw.ntruss.com/map-reversegeocode/v2/gc";

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(location.longitude.ToString() + "," + location.latitude.ToString());
        url += "?coords=" + location.longitude.ToString() + "," + location.latitude.ToString();
        url += "&orders=legalcode&output=json";
        StartCoroutine(GetReverseGeocode());
    }

    // Update is called once per frame
    void Update()
    {
        if (status_code == -1)
        {
            area1Text.text = "위치를 찾을 수 없습니다.";
            area2Text.text = "Location Module 확인";
        }
        else if (status_code == 0)
        {
            area1Text.text = response.results[0].region.area1.name;
            area2Text.text = response.results[0].region.area2.name;
        }
        else if (status_code == 3)
        {
            area1Text.text = "검색 결과가";
            area2Text.text = "없습니다.";
        }
    }

    IEnumerator GetReverseGeocode()
    {

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("X-NCP-APIGW-API-KEY-ID", "wnapo1j7yy");
            webRequest.SetRequestHeader("X-NCP-APIGW-API-KEY", "WD2NPl4SffTzHK1G8vyLlHUjOtAm3jYQUKqpjPsa");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                response = JsonUtility.FromJson<Root>(webRequest.downloadHandler.text);
                status_code = response.status.code;
            }
        }

    }
}
