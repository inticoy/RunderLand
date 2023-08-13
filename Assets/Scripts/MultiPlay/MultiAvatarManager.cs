using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiAvatarManager : MonoBehaviour
{
    [SerializeField]
    AvatarWithFriend avatar;
    [SerializeField]
    Player player;
    public PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(updateAvatar());
    }

    IEnumerator updateAvatar()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1f);
            updateDist(player.GetTotalDist());
        }
    }

    public void updateDist(double _mydist)
    {
        photonView.RPC("updateDistRPC", RpcTarget.Others, _mydist);
    }

    [PunRPC]
    public void updateDistRPC(double _avatardist)
    {
        avatar.SetDist(_avatardist);
    }

}
