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
    public Text status2_text;
    public Button enterLobby_button;

    public VoiceControl voiceControl;


    #region old
    //void Start()
    //{

    //    PhotonNetwork.ConnectUsingSettings();
    //    OnConnectedToMaster();
    //    Join();


    //}

    //private void Update()
    //{
    //    status2_text.text = PhotonNetwork.CountOfPlayers.ToString();
    //}

    //void Join()
    //{
    //    if (PhotonNetwork.IsConnected)
    //    {



    //        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 2 };

    //        PhotonNetwork.JoinOrCreateRoom(roomCode, roomOptions, null);

    //        // linked to the coroutine
    //        //PhotonNetwork.JoinOrCreateRoom(roomCode, roomOptions, null);


    //    }
    //    else
    //    {

    //        PhotonNetwork.ConnectUsingSettings();
    //    }
    //}
    //public override void OnConnectedToMaster()
    //{
    //    status_text.text = "서버에 연결되었습니다.";


    //}
    //public override void OnDisconnected(DisconnectCause cause)
    //{
    //    //base.OnDisconnected(cause);


    //    status_text.text = $"오프라인 {cause.ToString()} - 다시 연결 시도중";

    //    PhotonNetwork.ConnectUsingSettings();
    //}

    //public override void OnJoinedRoom()
    //{
    //    status_text.text = "방에 참가.";
    //    voiceControl.GenPlayerSpeaker();
    //}



    //public override void OnJoinRoomFailed(short returnCode, string message)
    //{

    //    status_text.text = "방에 참가하지 못했습니다.";
    //    //PhotonNetwork.CreateRoom(roomCode, new RoomOptions { MaxPlayers = 2 });

    //}
    #endregion


    void Start()
    {

        PhotonNetwork.ConnectUsingSettings();
        OnConnectedToMaster();


    }

    public void OnJoinButtonClicked()
    {

       
           

            if (PhotonNetwork.IsConnected)
            {


                status_text.text = "접속 중입니다";


                RoomOptions roomOptions = new RoomOptions { MaxPlayers = 2 };


            // linked to the coroutine
            PhotonNetwork.JoinOrCreateRoom(roomCode, roomOptions, null);


        }
            else
            {
                enterLobby_button.interactable = false;

                status_text.text = "Offline : Connection Disabled - Try Reconnecting";

                PhotonNetwork.ConnectUsingSettings();
            }
       

    }

   

    #region Photon Callback Methods
    public override void OnConnectedToMaster()
    {
        status_text.text = "서버에 연결되었습니다.";
        enterLobby_button.interactable = true;

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        //base.OnDisconnected(cause);


        status_text.text = $"오프라인 {cause.ToString()} - 다시 연결 시도중";

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinedRoom()
    {
        //base.OnJoinedRoom();

        status_text.text = "방에 입장하는 중입니다";
        voiceControl.GenPlayerSpeaker();
        status2_text.text = PhotonNetwork.CountOfPlayers.ToString();
    }



    public override void OnJoinRoomFailed(short returnCode, string message)
    {

        status_text.text = "방에 참가하지 못했습니다.";
        PhotonNetwork.CreateRoom(roomCode, new RoomOptions { MaxPlayers = 2 });

    }



    #endregion


}
