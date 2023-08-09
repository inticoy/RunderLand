using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiAvatarManager : MonoBehaviour
{
    //[SerializeField]
    //AvatarRecord avatar;
    //[SerializeField]
    //Player player;
    //public PhotonView photonView;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    StartCoroutine(updateAvatar());
    //}

    //IEnumerator updateAvatar()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSecondsRealtime(1f);
    //        updateDist((float) player.GetTotalDist());
    //    }
    //}

    //public void updateDist(float _mydist)
    //{
    //    photonView.RPC("updateDistRPC", RpcTarget.Others, _mydist);
    //}

    //[PunRPC]
    //public void updateDistRPC(float _avatardist)
    //{
    //    avatar.setDist(_avatardist);
    //}

}
