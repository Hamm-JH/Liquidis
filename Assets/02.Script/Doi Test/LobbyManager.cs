using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice;

public class LobbyManager : MonoBehaviourPunCallbacks
{

    public Button enterLobby_button;
    public Text status_text;

    public Animator lightgroup_ani;

  private string roomCode = "ABCD";
      void Start()
      {

          PhotonNetwork.ConnectUsingSettings();
        OnConnectedToMaster();

        
      }

      public void OnJoinButtonClicked()
      {
       

        if (MappingParameter.instance.AllMappedEmotionCheck())
        {
            enterLobby_button.interactable = false;

            if (PhotonNetwork.IsConnected)
            {


                status_text.text = "접속 중입니다";


                RoomOptions roomOptions = new RoomOptions { MaxPlayers = 2 };
                if(PhotonNetwork.CountOfRooms == 0)
                {
                    StartCoroutine(LoadWaitingAniCreate(roomOptions));

                }
                else if(PhotonNetwork.CountOfRooms == 1)
                {
                    StartCoroutine(LoadWaitingAniJoin());
                }
                // linked to the coroutine
                //PhotonNetwork.JoinOrCreateRoom(roomCode, roomOptions, null);


            }
            else
            {
                enterLobby_button.interactable = false;

                status_text.text = "Offline : Connection Disabled - Try Reconnecting";

                PhotonNetwork.ConnectUsingSettings();
            }
        }
        else
        {
            // 맵핑을 다 해야 넘어간다는 안내창 필요
            Debug.Log("맵핑이 다 안됐다");
        }
         




      }

    IEnumerator LoadWaitingAniCreate(RoomOptions roomOptions)
    {
        lightgroup_ani.SetTrigger("Start");
        yield return new WaitForSeconds(1.3f);
        PhotonNetwork.CreateRoom(roomCode, roomOptions, null);


    }

    IEnumerator LoadWaitingAniJoin()
    {
        lightgroup_ani.SetTrigger("Start");
        yield return new WaitForSeconds(1.3f);
        PhotonNetwork.JoinRoom(roomCode);


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

        MappingParameter.instance.playerNum = PhotonNetwork.LocalPlayer.ActorNumber - 1;

        // initial mapping parameters
        MappingParameter.instance.RemoveWaitingRoomParameters();


        // load scene
          PhotonNetwork.LoadLevel("04_Play_WaitingRoom");



      }



      public override void OnJoinRoomFailed(short returnCode, string message)
      {

          status_text.text = "방에 참가하지 못했습니다.";
          PhotonNetwork.CreateRoom(roomCode, new RoomOptions { MaxPlayers = 2 });

      }



      #endregion
}
