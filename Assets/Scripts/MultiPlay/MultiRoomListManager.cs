using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Security.Cryptography;

public class MultiRoomListManager : MonoBehaviourPunCallbacks
{
    private Dictionary<string, GameObject> roomDict = new Dictionary<string, GameObject>();
    public GameObject roomPrefab;
    public Transform scrollContent;
    public GameObject loadingModal;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Photon Callbacks

    public override void OnConnectedToMaster()
    {
        UnityEngine.Debug.Log("PUN Basics: OnConnectedToMaster()\n");
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        UnityEngine.Debug.LogWarningFormat("Disconnected()");
    }

    public override void OnJoinedLobby()
    {
        UnityEngine.Debug.Log("Lobby()");
        loadingModal.SetActive(false);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        UnityEngine.Debug.Log("JoinRandomFailed");
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 2;
        ro.IsOpen = true;
        ro.IsVisible = true;
        PhotonNetwork.CreateRoom("Jukim2", ro);
    }

    public override void OnJoinedRoom()
    {
        UnityEngine.Debug.Log("OnJoinedRoom");
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            UnityEngine.Debug.Log("PhotonNetwork: Loading Waiting Room");
            PhotonNetwork.LoadLevel("MultiWaitingScene");
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            UnityEngine.Debug.Log("PhotonNetwork: Loading Gaming Room");
            PhotonNetwork.LoadLevel("MultiPlayScene");
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UnityEngine.Debug.Log("OnRoomListUpdate()");
        GameObject tempRoom = null;

        foreach (var room in roomList)
        {
            UnityEngine.Debug.Log("hi");
            if (room.RemovedFromList == true)
            {
                roomDict.TryGetValue(room.Name, out tempRoom);
                Destroy(tempRoom);
                roomDict.Remove(room.Name);
            }

            else
            {
                if (roomDict.ContainsKey(room.Name) == false)
                {
                    GameObject _room = Instantiate(roomPrefab, scrollContent);
                    _room.GetComponent<RoomData>().RoomInfo = room;
                    roomDict.Add(room.Name, _room);
                }
                else
                {
                    roomDict.TryGetValue(room.Name, out tempRoom);
                    tempRoom.GetComponent<RoomData>().RoomInfo = room;
                }    
            }
        }
    }

    #endregion

    public void OnMakeRoomClick()
    {
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 2;

        PhotonNetwork.CreateRoom("Jinhchoi", ro);
    }
}
