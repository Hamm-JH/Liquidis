using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;
using Photon.Pun;
using Photon.Voice;
using Photon.Voice.Unity;
using Photon.Voice.PUN;




public class VoiceControl : MonoBehaviour
{

    public GameObject playerPref;
    public AudioClip playerClip;
    public AudioSource playerSource;
   
    PhotonVoiceNetwork photonVoiceNetwork;
   
    void Start()
    {
        //PhotonNetwork.Instantiate("player", Vector3.zero, Quaternion.Euler(Vector3.zero));
        //GenPlayerSpeaker();
        photonVoiceNetwork = GetComponent<PhotonVoiceNetwork>();
    }

    public void GenPlayerSpeaker()
    {
        GameObject player;

       

        player = PhotonNetwork.Instantiate("player", Vector3.zero, Quaternion.Euler(Vector3.zero));
       
        if(photonVoiceNetwork != null)
        {
            photonVoiceNetwork.InitRecorder(photonVoiceNetwork.GetComponent<Recorder>());

        }

        playerClip = player.GetComponentInChildren<AudioSource>().clip;
        playerSource.clip = playerClip;
    }

    public void EndTransmition()
    {
        photonVoiceNetwork.GetComponent<Recorder>().TransmitEnabled = false;
    }
}
