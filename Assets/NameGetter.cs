using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameGetter : MonoBehaviour
{
    TMP_Text nameText;

    // Start is called before the first frame update
    void Start()
    {
        nameText = GetComponent<TMP_Text>();
        string name = PlayerPrefs.GetString("playerName");
        nameText.text = name + "님,";
    }
}
