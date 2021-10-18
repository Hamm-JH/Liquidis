using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class DataSyncronize : MonoBehaviourPunCallbacks
{
    private static DataSyncronize _dataSyncronize;
    public static DataSyncronize instance
    {
        get
        {
            if (_dataSyncronize == null)
                return null;
            else
                return _dataSyncronize;
        }
    }

    public float sympathy_p1;
    public float sympathy_p2;


    private void Awake()
    {
        if (_dataSyncronize == null)
            _dataSyncronize = this;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        //base.OnDisconnected(cause);


        Debug.Log($"오프라인 {cause.ToString()} - 다시 연결 시도중");

        PhotonNetwork.ConnectUsingSettings();
    }

    public void SetSympathy(int targetPlayer, float value)
    {
        photonView.RPC("RPC_SetSympathy", RpcTarget.AllBuffered, targetPlayer, value);
    }

    [PunRPC]
    void RPC_SetSympathy(int targetPlayer, float value)
    {
        if(targetPlayer == 0)
        {
            sympathy_p1 = value;
        }
        else
        {
            sympathy_p2 = value;
        }

        if(MappingParameter.instance.playerNum == 0)
        {
            MappingParameter.instance.SetVFXValueMeeting(sympathy_p2);
            MeetingRoom.instance.mirrorPlane.GetComponent<Renderer>().material.SetFloat("_ReflectionIntensity", sympathy_p2);
        }
        else if(MappingParameter.instance.playerNum == 1)
        {
            MappingParameter.instance.SetVFXValueMeeting(sympathy_p1);
            MeetingRoom.instance.mirrorPlane.GetComponent<Renderer>().material.SetFloat("_ReflectionIntensity", sympathy_p1);

        }
    }
}
