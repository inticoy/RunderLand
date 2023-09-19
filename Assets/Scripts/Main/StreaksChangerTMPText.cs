using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StreaksChangerTMPText : MonoBehaviour
{
    public TMP_Text text3d;

    // Start is called before the first frame update
    void Start()
    {
        text3d.text = "7";
    }

    // Update is called once per frame
    void Update()
    {
        text3d.text = PlayerPrefs.GetInt("streakDays", 1).ToString();
    }
}
