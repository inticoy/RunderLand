using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAvatar : MonoBehaviour
{
    public GameObject AnimeMan, RealisticMan, BicycleMan;

    void Start()
    {
        if (!PlayerPrefs.HasKey("avatar"))
        {
            PlayerPrefs.SetString("avatar", "BicycleMan");            
        }
        string avatar = PlayerPrefs.GetString("avatar");
        if (avatar == "BicycleMan")
        {
            AnimeMan.SetActive(false);
            RealisticMan.SetActive(false);
            BicycleMan.SetActive(true);
        }
        else if (avatar == "AnimeMan")
        {
            AnimeMan.SetActive(true);
            RealisticMan.SetActive(false);
            BicycleMan.SetActive(false);
        }
        else if (avatar == "RealisticMan")
        {
            AnimeMan.SetActive(false);
            RealisticMan.SetActive(true);
            BicycleMan.SetActive(false);
        }
    }
}
