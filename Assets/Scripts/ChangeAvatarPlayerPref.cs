using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAvatarPlayerPref : MonoBehaviour
{
    [SerializeField] private GameObject pointer;
    private Vector3 newPosition;

    void Start()
    {
        pointer = GameObject.Find("Pointer");

        if (!PlayerPrefs.HasKey("avatar"))
        {
            PlayerPrefs.SetString("avatar", "AnimeMan");
        }
        chooseAvatar(PlayerPrefs.GetString("avatar"));
    }

    public void chooseAvatar(string avatar)
    {
        PlayerPrefs.SetString("avatar", avatar);
        
        newPosition = pointer.transform.localPosition;
        if (avatar == "AnimeMan")
            newPosition.x = -1.25f;
        else if (avatar == "RealisticMan")
            newPosition.x = 0.05f;
        else
            newPosition.x = 1.3f;
        pointer.transform.localPosition = newPosition;
    }

}
