using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RecordSelectPlayButton : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetString("RecordFile") == null)
            GetComponent<Button>().enabled = false;
        else
            GetComponent<Button>().enabled = true;

    }
}
