using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class WaitingRoomPhoton : MonoBehaviourPunCallbacks
{
    bool playerJoin = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveMainScene());
    }

    private void Update()
    {
        int playerNum = PhotonNetwork.CurrentRoom.PlayerCount;
        if (playerNum > 1)
        {
            playerJoin = true;
            return;
        }

    }

    IEnumerator MoveMainScene()
    {
        yield return new WaitUntil(() => playerJoin);

        WaitingRoom.instance.WaitingAniSequence();


    }
}
