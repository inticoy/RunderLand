using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.Text;

public class MultiManager : MonoBehaviourPunCallbacks
{
    #region Photon Callbacks

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player other)
    {
        UnityEngine.Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName);

        if (PhotonNetwork.IsMasterClient)
        {
            UnityEngine.Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);

            LoadArena();
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player other)
    {
        UnityEngine.Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName);

        if (PhotonNetwork.IsMasterClient)
        {
            UnityEngine.Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
            LoadArena();
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("MultiListScene");
    }

    #endregion

    #region Public Methods

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    #endregion

    #region Private Methods

    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            UnityEngine.Debug.LogError("PhotonNetwork: not master");
            return;
        }
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

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            //PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }
}
