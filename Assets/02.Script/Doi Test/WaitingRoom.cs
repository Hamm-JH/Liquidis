using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.VFX;

public class WaitingRoom : MonoBehaviour
{

    [Header("UI")]
    public Slider concentration_slider;
    public Slider excitement_slider;
    public Slider positive_slider;
    public Slider sympathy_slider;

    [Header("Obj")]
    public GameObject waitingPreview;

    [Header("Animator")]
    public Animator slider_canvas;
    public Animator selectBox;
    public VisualEffect vfxEffect;
    public Animator cylinderHall;
    public float cylinderAniTime = 5f;

    private static WaitingRoom _waitingRoom;
    public static WaitingRoom instance
    {
        get
        {
            if (_waitingRoom == null)
                return null;
            else
                return _waitingRoom;
        }
    }
    private void Awake()
    {
        if (_waitingRoom == null)
        {
            _waitingRoom = this;

        }
    }
    private void Start()
    {
        StartCoroutine(InitialMappingParameters());
        SliderAni();
    }

    IEnumerator InitialMappingParameters()
    {
        yield return new WaitForEndOfFrame();
        MappingParameter.instance.InitialWaitingRoomParameters(waitingPreview, vfxEffect);
        MappingSliders();

    }

    public void LoadMeetingRoom()
    {
        PhotonNetwork.LoadLevel("MeetingRoom");
    }
    
    void SliderAni()
    {
        slider_canvas.SetTrigger("GuideStart");
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
        MappingParameter.instance.vfx_slider.onValueChanged.AddListener(delegate { SetVFXValue(); });

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

    void WaitingAniSequence()
    {
        //StartCoroutine(WaitingAni());
        Debug.LogError("ani start");
        selectBox.SetTrigger("Start");
        //selectBox.GetComponent<SelectBoxAni>().HelpUIStart();
        slider_canvas.SetTrigger("FadeStart");
        vfxEffect.SetFloat("SpawnRate", 0f);
    }

    //IEnumerator WaitingAni()
    //{
    //    selectBox.SetTrigger("Start");
    //    slider_canvas.SetTrigger("FadeStart");
    //    vfxEffect.SetFloat("SpawnRate", 0f);

    //    yield return null;
    //}

    public void DisabledSelectBox()
    {
        selectBox.gameObject.SetActive(false);
        cylinderHall.SetTrigger("WaitingRoomLight");
        StartCoroutine(CylinderSequence());
    }

    IEnumerator CylinderSequence()
    {
        yield return new WaitForSeconds(cylinderAniTime);
        cylinderHall.SetTrigger("WaitingRoomMove");

    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            WaitingAniSequence();
            Debug.Log("f");
        }
    }

}
