using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerController : MonoBehaviourPun
{
    private void Start()
    {
        if (photonView.IsMine)
            DontDestroyOnLoad(gameObject);
    }
}
