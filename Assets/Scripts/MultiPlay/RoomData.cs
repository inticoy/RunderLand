using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;


public class RoomData : MonoBehaviour
{
    private TMP_Text RoomInfoText;
    private TMP_Text userIdText;
    [SerializeField] private TMP_Text RoomNameText;
    [SerializeField] private TMP_Text MaxPlayersText;
    [SerializeField] private RoomJoinButton roomJoinButton;
    private RoomInfo _roomInfo;

    public RoomInfo RoomInfo
    {
        get
        {
            return _roomInfo;
        }
        set
        {
            _roomInfo = value;
            //RoomInfoText.text = $"{_roomInfo.Name} ({_roomInfo.PlayerCount}/{_roomInfo.MaxPlayers})";
            RoomInfoText.text = $"({_roomInfo.PlayerCount}/{_roomInfo.MaxPlayers})";

            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => OnRoomButtonClicked(_roomInfo));
        }
    }

    void Awake()
    {
        TMP_Text[] buttonTextComponent = GetComponentsInChildren<TMP_Text>();

        RoomNameText = GameObject.Find("Room Name Text").GetComponent<TMP_Text>();
        MaxPlayersText = GameObject.Find("Room Max Players Text").GetComponent<TMP_Text>();
        roomJoinButton = GameObject.Find("Room Join Button").GetComponent<RoomJoinButton>();

        RoomInfoText = buttonTextComponent[1];
        userIdText = buttonTextComponent[0];
    }

    void OnRoomButtonClicked(RoomInfo roomInfo)
    {
        roomJoinButton.roomInfo = roomInfo;
        RoomNameText.text = "이름: " + roomInfo.Name;
        MaxPlayersText.text = "최대 인원: " + roomInfo.MaxPlayers.ToString();
    }

    void OnEnterRoom(string roomName)
    {
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 2;

        PhotonNetwork.NickName = PlayerPrefs.GetString("playerName"); // inputtextfield required
        PhotonNetwork.JoinOrCreateRoom(roomName, ro, TypedLobby.Default);
    }
}
