using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickArrivals : MonoBehaviour
{
    public Image [] images = new Image[5];

    private void Start()
    {
        foreach (Image image in images)
        {
            image.color = new Color(0, 0, 0, 0);
        }
    }

    public void SetArrival(string arrival)
    {
        if (arrival == "Gyeongbokgung Palace")
        {
            PlayerPrefs.SetFloat("latitude", 37.575767f);
            PlayerPrefs.SetFloat("longitude", 126.976808f);
            foreach (Image image in images)
            {
                image.color = new Color(0, 0, 0, 0);
            }
            images[0].color = new Color(0, 0, 1, 0.3f);      
        }
        else if (arrival == "Banpo Hangang Park")
        {
            PlayerPrefs.SetFloat("latitude", 37.510746f);
            PlayerPrefs.SetFloat("longitude", 126.996019f);
            foreach (Image image in images)
            {
                image.color = new Color(0, 0, 0, 0);
            }
            images[1].color = new Color(0, 0, 1, 0.3f);
        }
        else if (arrival == "Namsan Tower")
        {
            PlayerPrefs.SetFloat("latitude", 37.551560f);
            PlayerPrefs.SetFloat("longitude", 126.988110f);
            foreach (Image image in images)
            {
                image.color = new Color(0, 0, 0, 0);
            }
            images[2].color = new Color(0, 0, 1, 0.3f);
        }
        else if (arrival == "Cheonggyecheon")
        {
            PlayerPrefs.SetFloat("latitude", 37.569251f);
            PlayerPrefs.SetFloat("longitude", 126.978601f);
            foreach (Image image in images)
            {
                image.color = new Color(0, 0, 0, 0);
            }
            images[3].color = new Color(0, 0, 1, 0.3f);
        }
        else if (arrival == "Lotte World")
        {
            PlayerPrefs.SetFloat("latitude", 37.512934f);
            PlayerPrefs.SetFloat("longitude", 127.102192f);
            foreach (Image image in images)
            {
                image.color = new Color(0, 0, 0, 0);
            }
            images[4].color = new Color(0, 0, 1, 0.3f);
        }
    }


}
