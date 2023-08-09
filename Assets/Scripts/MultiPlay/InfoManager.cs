using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class InfoManager : MonoBehaviour
{
    public TMP_Text roomNameText;
    // Start is called before the first frame update
    void Start()
    {
        roomNameText.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
