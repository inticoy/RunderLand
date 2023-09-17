using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickArrivals : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] squares = new SpriteRenderer[5];

    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            squares[i] = GameObject.Find("Square" + (i + 1).ToString()).GetComponent<SpriteRenderer>();
            squares[i].enabled = false;
        }

    }

    public void SetArrival(string arrival)
    {
        Debug.Log("gogo0");
        if (arrival == "Gyeongbokgung Palace")
        {
            Debug.Log("gogo1");
            PlayerPrefs.SetFloat("latitude", 37.575767f);
            PlayerPrefs.SetFloat("longitute", 126.976808f);
            foreach (SpriteRenderer square in squares)
            {
                square.enabled = false;
            }
            squares[0].enabled = true;      
        }
        else if (arrival == "Banpo Hangang Park")
        {
            PlayerPrefs.SetFloat("latitude", 37.510746f);
            PlayerPrefs.SetFloat("longitute", 126.996019f);
            foreach (SpriteRenderer square in squares)
            {
                square.enabled = false;
            }
            squares[1].enabled = true;
        }
        else if (arrival == "Namsan Tower")
        {
            PlayerPrefs.SetFloat("latitude", 37.551560f);
            PlayerPrefs.SetFloat("longitute", 126.988110f);
            foreach (SpriteRenderer square in squares)
            {
                square.enabled = false;
            }
            squares[2].enabled = true;
        }
        else if (arrival == "Cheonggyecheon")
        {
            PlayerPrefs.SetFloat("latitude", 37.569251f);
            PlayerPrefs.SetFloat("longitute", 126.978601f);
            foreach (SpriteRenderer square in squares)
            {
                square.enabled = false;
            }
            squares[3].enabled = true;
        }
        else if (arrival == "Lotte World")
        {
            PlayerPrefs.SetFloat("latitude", 37.512934f);
            PlayerPrefs.SetFloat("longitute", 127.102192f);
            foreach (SpriteRenderer square in squares)
            {
                square.enabled = false;
            }
            squares[4].enabled = true;
        }
    }


}
