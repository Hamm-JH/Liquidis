using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WaitingRoom : MonoBehaviour
{
    public void LoadMeetingRoom()
    {
        PhotonNetwork.LoadLevel("MeetingRoom");
    }
}
