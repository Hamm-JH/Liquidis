using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;
using Photon.Pun;

public class VoiceControl : MonoBehaviour
{

    public GameObject playerPref;
    public AudioClip playerClip;
    public AudioSource playerSource;

    void Start()
    {
        //PhotonNetwork.Instantiate("player", Vector3.zero, Quaternion.Euler(Vector3.zero));
        //GenPlayerSpeaker();
    }

    public void GenPlayerSpeaker()
    {
        GameObject player;
        player = PhotonNetwork.Instantiate("player", Vector3.zero, Quaternion.Euler(Vector3.zero));
        playerClip = player.GetComponentInChildren<AudioSource>().clip;
        playerSource.clip = playerClip;
    }

}
