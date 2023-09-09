using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAvatarPlayerPref : MonoBehaviour
{
    void Start()
    {
        if (!PlayerPrefs.HasKey("avatar"))
        {
            PlayerPrefs.SetString("avatar", "AnimeMan");
        }
    }

    public void chooseAvatar(string avatar)
    {
        PlayerPrefs.SetString("avatar", avatar);
        Debug.Log(PlayerPrefs.GetString("avatar"));
    }

}
