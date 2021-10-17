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
    //    status_text.text = "������ ����Ǿ����ϴ�.";


    //}
    //public override void OnDisconnected(DisconnectCause cause)
    //{
    //    //base.OnDisconnected(cause);


    //    status_text.text = $"�������� {cause.ToString()} - �ٽ� ���� �õ���";

    //    PhotonNetwork.ConnectUsingSettings();
    //}

    //public override void OnJoinedRoom()
    //{
    //    status_text.text = "�濡 ����.";
    //    voiceControl.GenPlayerSpeaker();
    //}



    //public override void OnJoinRoomFailed(short returnCode, string message)
    //{

    //    status_text.text = "�濡 �������� ���߽��ϴ�.";
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


                status_text.text = "���� ���Դϴ�";


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
        status_text.text = "������ ����Ǿ����ϴ�.";
        enterLobby_button.interactable = true;

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        //base.OnDisconnected(cause);


        status_text.text = $"�������� {cause.ToString()} - �ٽ� ���� �õ���";

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinedRoom()
    {
        //base.OnJoinedRoom();

        status_text.text = "�濡 �����ϴ� ���Դϴ�";
        voiceControl.GenPlayerSpeaker();
        status2_text.text = PhotonNetwork.CountOfPlayers.ToString();
    }



    public override void OnJoinRoomFailed(short returnCode, string message)
    {

        status_text.text = "�濡 �������� ���߽��ϴ�.";
        PhotonNetwork.CreateRoom(roomCode, new RoomOptions { MaxPlayers = 2 });

    }



    #endregion


}
