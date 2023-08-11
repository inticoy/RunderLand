using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomJoinButton : MonoBehaviour
{
    public RoomInfo roomInfo;

    // Start is called before the first frame update
    void Start()
    {
        roomInfo = null;
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => OnRoomJoinButtonClicked());
    }

    void OnRoomJoinButtonClicked()
    {
        if (roomInfo == null)
            return;
        Debug.Log(roomInfo.Name);
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 2;

        PhotonNetwork.NickName = "junseo"; // inputtextfield required
        //PhotonNetwork.JoinOrCreateRoom(roomInfo.Name, ro, TypedLobby.Default);
        if (PhotonNetwork.IsConnectedAndReady)
            PhotonNetwork.JoinRoom(roomInfo.Name);
    }
    

}
