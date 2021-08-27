using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{

  private string roomCode = "ABCD";
      void Start()
      {

          PhotonNetwork.ConnectUsingSettings();
      }

      private void OnJoinButtonClicked()
      {
          enterLobby_button.interactable = false;

          if (PhotonNetwork.IsConnected)
          {


              status_text.text = "접속 중입니다";

              RoomOptions roomOptions = new RoomOptions { MaxPlayers = 2 };

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
          generateCode_button.interactable = false;

          status_text.text = $"오프라인 {cause.ToString()} - 다시 연결 시도중";

          PhotonNetwork.ConnectUsingSettings();
      }

      public override void OnJoinedRoom()
      {
          //base.OnJoinedRoom();

          status_text.text = "방에 입장하는 중입니다";

          PhotonNetwork.LoadLevel("03.MeetingRoom");



      }



      public override void OnJoinRoomFailed(short returnCode, string message)
      {

          status_text.text = "방에 참가하지 못했습니다.";
          PhotonNetwork.CreateRoom(roomCode, new RoomOptions { MaxPlayers = 2 });

      }



      #endregion
}
