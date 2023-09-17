using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleNavi : MonoBehaviour
{
    [SerializeField] private GameObject ScrollCanvas;
    [SerializeField] private GameObject ButtonCanvas;

    private void Start()
    {
        ScrollCanvas = GameObject.Find("ScrollCanvas");
        ButtonCanvas = GameObject.Find("ButtonCanvas");
    }

    public void ToggleNavigation(int i)
    {
        if (i == 1)
        {
            PlayerPrefs.SetInt("IsNaviOn", i);
            ScrollCanvas.SetActive(false);
            ButtonCanvas.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt("IsNaviOn", 0);
            ScrollCanvas.SetActive(false);
            ButtonCanvas.SetActive(false);
        }
    }
}
