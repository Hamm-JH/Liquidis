using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;
using Photon.Pun;

public class VoiceControl : MonoBehaviour
{

    public GameObject playerPref;

    
    void Start()
    {
        PhotonNetwork.Instantiate("player", Vector3.zero, Quaternion.Euler(Vector3.zero));
    }

    public void GenPlayerSpeaker()
    {
        PhotonNetwork.Instantiate("player", Vector3.zero, Quaternion.Euler(Vector3.zero));

    }

}
