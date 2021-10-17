using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice;
using UnityEngine.UI;

public class LobbyDebug : MonoBehaviourPunCallbacks
{

    private string roomCode = "ABCD";
    public Text status_text;

    public VoiceControl voiceControl;

    void Start()
    {

        PhotonNetwork.ConnectUsingSettings();
        OnConnectedToMaster();

       

    }

    void Join()
    {
        if (PhotonNetwork.IsConnected)
        {



            RoomOptions roomOptions = new RoomOptions { MaxPlayers = 2 };

            PhotonNetwork.JoinOrCreateRoom(roomCode, roomOptions, null);

            // linked to the coroutine
            //PhotonNetwork.JoinOrCreateRoom(roomCode, roomOptions, null);


        }
        else
        {
            
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    public override void OnConnectedToMaster()
    {
        status_text.text = "서버에 연결되었습니다.";
        Join();

    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        //base.OnDisconnected(cause);


        status_text.text = $"오프라인 {cause.ToString()} - 다시 연결 시도중";

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinedRoom()
    {
        status_text.text = "방에 참가.";
        voiceControl.GenPlayerSpeaker();
    }



    public override void OnJoinRoomFailed(short returnCode, string message)
    {

        status_text.text = "방에 참가하지 못했습니다.";
        PhotonNetwork.CreateRoom(roomCode, new RoomOptions { MaxPlayers = 2 });

    }
}
