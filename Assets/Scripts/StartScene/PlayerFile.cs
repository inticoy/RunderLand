using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlayerFile : MonoBehaviour
{
    public GameObject keyboard;
    string playerName;
    DateTime todayDate;
    DateTime lastRunDate;
    int streakDays;
    bool isKeyboardActive;

    void Start()
    {
        todayDate = DateTime.Now.Date;
        Debug.Log("Today : " + todayDate);

        // Get playerName
        if (PlayerPrefs.HasKey("playerName"))
        {
            playerName = PlayerPrefs.GetString("playerName");
            Debug.Log("playerName is : " + playerName);
            isKeyboardActive = false;
        }
        else
        {
            keyboard.SetActive(true);
            isKeyboardActive = true;
            Debug.Log("Set name");
            // Show keyboard and get name by input                        
        }

        // Get lastRunDate
        if (PlayerPrefs.HasKey("lastRunDate"))
        {
            lastRunDate = DateTime.Parse(PlayerPrefs.GetString("lastRunDate"));
            Debug.Log("lastDay : " + lastRunDate);
        }
        else
        {
            lastRunDate = DateTime.Now.Date;            
            Debug.Log("last : " + lastRunDate.ToString());
        }        

        // Get streakDays using lastRunDate
        streakDays = PlayerPrefs.GetInt("streakDays", 1);
        int daySpan = (lastRunDate - todayDate).Days;
        
        if (daySpan == 1)
        {
            streakDays++;
            PlayerPrefs.SetInt("streakDays", streakDays);
        }
        else
        {
            // daySpan == 0 || daySpan > 1
            streakDays = 1;
            PlayerPrefs.SetInt("streakDays", 1);
        }
        Debug.Log("StreakDays : " + streakDays.ToString());

        // Update lastRunDate
        PlayerPrefs.SetString("lastRunDate", todayDate.ToString());
        PlayerPrefs.Save();
        if (isKeyboardActive == false)
        {
            SceneManager.LoadScene("MainScene");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
