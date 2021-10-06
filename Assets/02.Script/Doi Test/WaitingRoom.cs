using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;


public class WaitingRoom : MonoBehaviour
{

    [Header("UI")]
    public Slider concentration_slider;
    public Slider excitement_slider;
    public Slider positive_slider;
    public Slider sympathy_slider;

    [Header("Obj")]
    public GameObject waitingPreview;

    private void Start()
    {
        StartCoroutine(InitialMappingParameters());
    }

    IEnumerator InitialMappingParameters()
    {
        yield return new WaitForEndOfFrame();
        MappingParameter.instance.InitialWaitingRoomParameters(waitingPreview);
        MappingSliders();

    }

    public void LoadMeetingRoom()
    {
        PhotonNetwork.LoadLevel("MeetingRoom");
    }

    // UI Slider ¡∂¿€
    void MappingSliders()
    {
        for (int i = 0; i < MappingParameter.instance.matchType.Length; i++)
        {
            if (MappingParameter.instance.matchType[i] == 1)
            {
                MappingParameter.instance.geo_slider = concentration_slider;
                MappingParameter.instance.geo_slider.onValueChanged.AddListener(delegate { SetGeoValue(); });

            }
            else if(MappingParameter.instance.matchType[i] == 2)
            {
                MappingParameter.instance.color_slider = positive_slider;
                MappingParameter.instance.geo_slider.onValueChanged.AddListener(delegate { SetColorValue(); });


            }
            else if (MappingParameter.instance.matchType[i] == 3)
            {
                MappingParameter.instance.speed_slider = excitement_slider;
                MappingParameter.instance.geo_slider.onValueChanged.AddListener(delegate { SetSpeedValue(); });


            }
        }

        MappingParameter.instance.vfx_slider = sympathy_slider;
    }
    public void SetGeoValue()
    {
        //for (int i = 0; i < MappingParameter.instance.matchType.Length; i++)
        //{
        //    if (MappingParameter.instance.matchType[i] == 1)
        //    {
        //        MappingParameter.instance.SetGeoValue();

        //    }
            
        //}

        MappingParameter.instance.SetGeoValue();
    }

    public void SetColorValue()
    {
        //for (int i = 0; i < MappingParameter.instance.matchType.Length; i++)
        //{
        //    if (MappingParameter.instance.matchType[i] == 2)
        //    {
        //        MappingParameter.instance.SetColorValue();

        //    }

        //}

        MappingParameter.instance.SetColorValue();

    }

    public void SetSpeedValue()
    {
        //for (int i = 0; i < MappingParameter.instance.matchType.Length; i++)
        //{
        //    if (MappingParameter.instance.matchType[i] == 3)
        //    {
        //        MappingParameter.instance.SetSpeedValue();

        //    }

        //}

        MappingParameter.instance.SetSpeedValue();

    }

    public void SetVFXValue()
    {
        MappingParameter.instance.SetVFXValue();

    }


}
