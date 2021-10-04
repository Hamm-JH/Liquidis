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
    public GameObject previewSphere;

    private void Start()
    {
        StartCoroutine(InitialMappingParameters());
    }

    IEnumerator InitialMappingParameters()
    {
        yield return new WaitForEndOfFrame();
        MappingParameter.instance.InitialWaitingRoomParameters(previewSphere);
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

            }else if(MappingParameter.instance.matchType[i] == 2)
            {
                MappingParameter.instance.color_slider = excitement_slider;

            }
            else if (MappingParameter.instance.matchType[i] == 3)
            {
                MappingParameter.instance.speed_slider = positive_slider;

            }
        }

        MappingParameter.instance.vfx_slider = sympathy_slider;
    }
    public void SetConcentrationValue()
    {
        for (int i = 0; i < MappingParameter.instance.matchType.Length; i++)
        {
            if (MappingParameter.instance.matchType[i] == 1)
            {
                MappingParameter.instance.SetGeoValue();

            }
            
        }
    }

    public void SetExcitementValue()
    {
        for (int i = 0; i < MappingParameter.instance.matchType.Length; i++)
        {
            if (MappingParameter.instance.matchType[i] == 2)
            {
                MappingParameter.instance.SetColorValue();

            }

        }
    }

    public void SetPositiveValue()
    {
        for (int i = 0; i < MappingParameter.instance.matchType.Length; i++)
        {
            if (MappingParameter.instance.matchType[i] == 3)
            {
                MappingParameter.instance.SetSpeedValue();

            }

        }
    }

    public void SetSympathyValue()
    {
        MappingParameter.instance.SetVFXValue();

    }


}
