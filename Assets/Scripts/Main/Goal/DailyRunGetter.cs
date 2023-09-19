using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using TMPro;

public class DailyRunGetter : MonoBehaviour
{
    public PercentageScaler percentageScaler;
    public RunnerGauge runnerGauge;

    List<string> todayFiles;
    [SerializeField] TMP_Text dailyGoalText;
    [SerializeField] TMP_Text dailyDistanceText;

    // Start is called before the first frame update
    void Start()
    {
        todayFiles = new();
        string path = Application.persistentDataPath + "/running_logs";
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        string[] fileList = Directory.GetFiles(path);

        foreach (string filePath in fileList)
        {
            if (Path.GetFileName(filePath).Substring(0, 8).CompareTo(DateTime.Now.ToString("yyyyMMdd")) == 0)
                todayFiles.Add(filePath);
        }
        dailyGoalText.text = (GetDailyRunDistance() / 1000).ToString("0.0") + " / 5km";
        dailyDistanceText.text = (GetDailyRunDistance() / 1000).ToString("0.00");

        percentageScaler.percentage = (GetDailyRunDistance() / 5000);
        runnerGauge.percentage = percentageScaler.percentage;
    }

    public float GetDailyRunDistance()
    {
        float dailyRunDistance = 0f;

        foreach (string filePath in todayFiles)
        {
            dailyRunDistance += GPXReader.getGPXDistance(GPXReader.ReadGPXFile(filePath));
        }

        return (dailyRunDistance);
    }
}
