using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreaksChanger : MonoBehaviour
{
    public SimpleHelvetica text3d;

    // Start is called before the first frame update
    void Start()
    {
        text3d.Text = "7";
    }

    // Update is called once per frame
    void Update()
    {
        text3d.Text = PlayerPrefs.GetInt("streakDays", 1).ToString();
    }
}
