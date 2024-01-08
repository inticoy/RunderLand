using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class RecordsDownloader : MonoBehaviour
{
    string path;
    string filePath;

    void Start()
    {
        path = Application.persistentDataPath + "/running_records";
        filePath = path + "/donga.gpx";

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        if (File.Exists(filePath))
            return;
        File.Create(filePath);
        StartCoroutine(DownLoadGet("https://soso-k.com/race/?uid=415&action=kboard_file_download&kboard-file-download-nonce=c79f2dbfb2&file=file1"));
    }

    public IEnumerator DownLoadGet(string URL)
        {
            UnityWebRequest request = UnityWebRequest.Get(URL);

            yield return request.SendWebRequest();
            // 에러 발생 시
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(request.error);
            }
            else
            {
                File.WriteAllBytes(filePath, request.downloadHandler.data); // 파일 다운로드
            }
        }

    }