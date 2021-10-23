using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.VFX;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class WaitingRoom : MonoBehaviour
{

    [Header("UI")]
    public Slider concentration_slider;
    public Slider excitement_slider;
    public Slider positive_slider;
    public Slider sympathy_slider;

    public GameObject[] concentration_subTexts;
    public GameObject[] excitement_subTexts;
    public GameObject[] positive_subTexts;



    [Header("Obj")]
    public GameObject waitingPreview;

    [Header("Animator")]
    public Animator slider_canvas;
    
    public Animator selectBox;
    public VisualEffect vfxEffect;
    public Animator cylinderHall;
   
    public Animator light_group_ani;
    public Animator counterHead_ani;

    bool vfxOn = true;

    public float afterSliderFadeIn = 1f;
    public float afterSelectBoxShake = 7f;
    public float selectBoxFalseTime = 9f;
    public float afterSelectBox = 1f;
    public float cylinderAniTime = 5f;
    public float afterWaitingLight = 1f;
    public float meetStartTime = 3.3f;
    public float counterHeadTime = 12.8f;

    [Header("API")]
    public UnityAction<API.Brainwave> biGetter;
    UnityAction<API.Lerp> lerpGetter;

    public float concentrationCurrentValue = 0f;
    public float excitementCurrentValue = 0f;
    public float positiveCurrentValue = 0f;
    public float sympathyCurrentValue = 0f;

    public float speedInterval = 3f;
    API.Lerp.Function functionType;


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
        // 이벤트에 메서드 연결
        biGetter += Receive;

        lerpGetter += Receive;

        //StartCoroutine(InitialMappingParameters());
        InitialMappingParameters();

        light_group_ani.SetTrigger("SceneStart");
        SliderAni();
    }

    void InitialMappingParameters()
    {
        //yield return new WaitForEndOfFrame();
        MappingParameter.instance.InitialWaitingRoomParameters(waitingPreview, vfxEffect);
        MappingSliders();

    }

   
    
    void SliderAni()
    {

        // debug disabled
        if (PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[0])
        {
            slider_canvas.gameObject.SetActive(true);
            slider_canvas.SetTrigger("SceneStart");
            StartCoroutine(SliderGuide());

        }
        // debug enabled
        //slider_canvas.gameObject.SetActive(true);
        //slider_canvas.SetTrigger("SceneStart");
        //StartCoroutine(SliderGuide());

    }

    IEnumerator SliderGuide()
    {
        yield return new WaitForSeconds(afterSliderFadeIn);
        slider_canvas.SetTrigger("GuideStart");

    }



    // UI Slider 조작
    void MappingSliders()
    {

        // match type 0 = geo
        // match type 1 = color
        // match type 2 = speed

        // match value 1 = concentration
        // match value 2 = positive
        // match value 3 = excitement

        for (int i = 0; i < MappingParameter.instance.matchType.Length; i++)
        {
            if (MappingParameter.instance.matchType[i] == 1)
            {
                MappingParameter.instance.geo_slider = concentration_slider;
                concentration_subTexts[i].SetActive(true);
                MappingParameter.instance.geo_slider.onValueChanged.AddListener(delegate { SetGeoValue(); });

            }
            else if(MappingParameter.instance.matchType[i] == 2)
            {
                MappingParameter.instance.color_slider = positive_slider;
                positive_subTexts[i].SetActive(true);
                MappingParameter.instance.color_slider.onValueChanged.AddListener(delegate { SetColorValue(); });


            }
            else if (MappingParameter.instance.matchType[i] == 3)
            {
                MappingParameter.instance.speed_slider = excitement_slider;
                excitement_subTexts[i].SetActive(true);
                MappingParameter.instance.speed_slider.onValueChanged.AddListener(delegate { SetSpeedValue(); });


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
    public void SetIntervalType(float _interval)
    {
        speedInterval = _interval;


    }

    public void SetVFXValue()
    {
        MappingParameter.instance.SetVFXValue();

    }

    public void WaitingAniSequence()
    {
        // 영상 시작할때
        // BIManager.Status = Status.영상시작구간;
        Manager.BIManager.Instance._CollectionStatus = Manager.CollectionStatus.Reference;

        //StartCoroutine(WaitingAni());
        Debug.Log("ani start");
        selectBox.SetTrigger("Start");
        StartCoroutine(WaitingAniSequenceAfterShake());
        
        StartCoroutine(SelectBoxFalseSequence());
        slider_canvas.SetTrigger("FadeStart");


        // sensor disconnected
        vfxOn = false;
        vfxEffect.SetFloat("SpawnRate", 0f); // -> 몇초?
    }

    IEnumerator WaitingAniSequenceAfterShake()
    {
        Debug.Log("light start waiting");
        yield return new WaitForSeconds(afterSelectBoxShake);
        light_group_ani.SetTrigger("Start");
        Debug.Log("light done");


    }

    IEnumerator SelectBoxFalseSequence()
    {
        yield return new WaitForSeconds(selectBoxFalseTime);
        
        selectBox.gameObject.SetActive(false);
        StartCoroutine(CylinderHallStart());
    }

    //IEnumerator SliderCanvasFalse()
    //{
    //    yield return new WaitForSeconds(2f);
    //    slider_canvas.gameObject.SetActive(false);
    //}

    IEnumerator CylinderHallStart()
    {
        yield return new WaitForSeconds(afterSelectBox);
        cylinderHall.SetTrigger("WaitingRoomLight");

        yield return new WaitForSeconds(afterWaitingLight);
        cylinderHall.SetTrigger("WaitingRoomMove");

        yield return new WaitForSeconds(meetStartTime);
        counterHead_ani.SetTrigger("MeetStart");
        Debug.Log("counter head start");
        yield return new WaitForSeconds(counterHeadTime);

		// 영상 종료시 Stanby(대기) 상태로 전환
		// 대기 상태에서는 데이터 할당 코드를 업데이트하지 않아 성능 낭비를 하지않음
		Manager.BIManager.Instance._CollectionStatus = Manager.CollectionStatus.Stanby;

		//photon call
		StartCoroutine(LoadMeetingRoom());
    }

   
    IEnumerator LoadMeetingRoom()
    {
        yield return new WaitForSeconds(2f);
        //SceneManager.LoadScene("MeetingRoom");
        PhotonNetwork.LoadLevel("MeetingRoom");
    }

  

    //public void DisabledSelectBox()
    //{
    //    selectBox.gameObject.SetActive(false);
    //    cylinderHall.SetTrigger("WaitingRoomLight");
    //    StartCoroutine(CylinderSequence());
    //}

    //IEnumerator CylinderSequence()
    //{
    //    yield return new WaitForSeconds(cylinderAniTime);
    //    cylinderHall.SetTrigger("WaitingRoomMove");

    //}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            WaitingAniSequence();
            Debug.Log("f");
        }

        //Debug.Log("player count : " + PhotonNetwork.CountOfPlayers);

        #region 1. EEG 데이터 요청
        // 데이터 요청시 api를 생성한다.
        // 뇌파 데이터 api 생성시 필수 할당 데이터
        // 1. targetId : 룩시드랩스 6개 센서중의 한 개의 센서데이터
        // 2. targetSecond : 메서드 실행 시점에서 과거 n초까지의 데이터 수집
        // 3. 데이터 수집 완료시 실행할 이벤트
        API.Brainwave api_EEG = new API.Brainwave(
            obj: API.Objective.EEG,
            targetId: Looxid.Link.EEGSensorID.AF3,
            targetSecond: 10,
            targetCallBack: biGetter
            );

        //Request(api_EEG);
        #endregion

        #region 2. 안정상태 데이터 요청

        API.Brainwave api_relaxation = new API.Brainwave(
            obj: API.Objective.Relaxation,
            targetCallBack: biGetter
            );

        //Request(api_relaxation);
        #endregion

        #region 3. 집중상태 데이터 요청

        API.Brainwave api_attention = new API.Brainwave(
            obj: API.Objective.Attention,
            targetCallBack: biGetter
            );

        //Request(api_attention);
        #endregion

        #region 4. 디버깅용 랜덤 EEG

        API.Brainwave api_randomEEG = new API.Brainwave(
            obj: API.Objective.EEGRandom,
            targetCallBack: biGetter
            );

        Request(api_randomEEG);
        #endregion

        #region 5. 디버깅용 이완 랜덤 마인드값

        API.Brainwave api_randomMindRelaxation = new API.Brainwave(
            obj: API.Objective.MindRandom,
            option: 0,
            targetCallBack: biGetter
            );

        Request(api_randomMindRelaxation);
        #endregion

        #region 6. 디버깅용 집중 랜덤 마인드값

        API.Brainwave api_randomMindAttention = new API.Brainwave(
            obj: API.Objective.MindRandom,
            option: 1,
            targetCallBack: biGetter
            );

        Request(api_randomMindAttention);

        #endregion

        #region 7. 디버깅용 positive 랜덤 마인드값

        API.Brainwave api_randomMindPositive = new API.Brainwave(
            obj: API.Objective.MindRandom,
            option: 2,
            targetCallBack: biGetter
            );

        Request(api_randomMindPositive);

        #endregion

        #region 8. 디버깅용 positive 랜덤 마인드값

        API.Brainwave api_randomMindEmpathy = new API.Brainwave(
            obj: API.Objective.MindRandom,
            option: 3,
            targetCallBack: biGetter
            );

        Request(api_randomMindEmpathy);

        #endregion
    }
    /// <summary>
    /// 뇌파 데이터 관리자(BIManager)로 데이터 수집 요청을 한다.
    /// </summary>
    /// <param name="api"></param>
    private void Request(API.Brainwave api)
    {
        Manager.BIManager.Instance.Request(api);
    }

    /// <summary>
    /// 이벤트에 연결된 데이터 수집 메서드
    /// 수집 완료된 데이터를 사용한다.
    /// </summary>
    /// <param name="api"></param>
    private void Receive(API.Brainwave api)
    {
        if (api.Objective == API.Objective.EEG)
        {
            string str = "";
            str += $"Sensor ID : {api.Id.ToString()}\n";
            str += $"interval seconds : {api.Second.ToString()}\n";

            str += $"Delta : {api.Delta}\n";
            str += $"Theta : {api.Theta}\n";
            str += $"Alpha : {api.Alpha}\n";
            str += $"Beta : {api.Beta} \n";
            str += $"Gamma : {api.Gamma}\n";

            //Debug.Log(str);
        }
        else if (api.Objective == API.Objective.Relaxation)
        {
            //Debug.Log($"Relaxation : {api.Relaxation.ToString()}");
        }
        else if (api.Objective == API.Objective.Attention)
        {
            //Debug.Log($"Attention : {api.Attention.ToString()}");
        }
        else if (api.Objective == API.Objective.EEGRandom)
        {
            string str = "";
            str += $"Sensor ID : {api.Id.ToString()}\n";
            str += $"interval seconds : {api.Second.ToString()}\n";

            str += $"Delta : {api.Delta}\n";
            str += $"Theta : {api.Theta}\n";
            str += $"Alpha : {api.Alpha}\n";
            str += $"Beta : {api.Beta} \n";
            str += $"Gamma : {api.Gamma}\n";

            //Debug.Log(str);
        }
        else if (api.Objective == API.Objective.MindRandom)
        {
          

            if (api.Option == 0)
            {
                //Debug.Log($"Relaxation: {api.Relaxation.ToString()}");
                //excitementCurrentValue = api.Relaxation;
                float current = excitementCurrentValue;
                float target = api.Relaxation;

                Request(1, current, target);


            }
            else if (api.Option == 1)
            {
                //Debug.Log($"Attention : {api.Attention.ToString()}");
                //concentrationCurrentValue = api.Attention;

                float current = concentrationCurrentValue;
                float target = api.Attention;

                Request(0, current, target);

            }
            else if (api.Option == 2)
            {
                //Debug.Log($"Attention : {api.Attention.ToString()}");
                //positiveCurrentValue = api.Positiveness;

                float current = positiveCurrentValue;
                float target = api.Positiveness;

                Request(2, current, target);

            }
            else if (api.Option == 3)
            {
                //Debug.Log($"Attention : {api.Attention.ToString()}");
                //sympathyCurrentValue = api.Empathy;

                float current = sympathyCurrentValue;
                float target = api.Empathy;

                Request(3, current, target);

            }
        }
    }

    public void Request(int index, float current, float value)
    {
        if (index == 0)
        {
            API.Lerp lerp_concentration = new API.Lerp(
          _requestIndex: 0,
          _Function: functionType,
          _interval: speedInterval,
          _currValue: current,
          _targetValue: value,
          _callback: lerpGetter
          );

            Request(lerp_concentration);

        }
        else if (index == 1)
        {
            API.Lerp lerp_excitement = new API.Lerp(
          _requestIndex: 1,
          _Function: functionType,
          _interval: speedInterval,
          _currValue: current,
          _targetValue: value,
          _callback: lerpGetter
          );

            Request(lerp_excitement);
        }
        else if (index == 2)
        {
            API.Lerp lerp_positive = new API.Lerp(
          _requestIndex: 2,
          _Function: functionType,
          _interval: speedInterval,
          _currValue: current,
          _targetValue: value,
          _callback: lerpGetter
          );

            Request(lerp_positive);
        }
        else if (index == 3)
        {
            API.Lerp lerp_sympathy = new API.Lerp(
          _requestIndex: 3,
          _Function: functionType,
          _interval: speedInterval,
          _currValue: current,
          _targetValue: value,
          _callback: lerpGetter
          );

            Request(lerp_sympathy);
        }

    }

    // select 에서 슬라이더 할당할 때 사용
    #region slider??
    public void Request(int index, float value)
    {
        if (index == 0)
        {
            API.Lerp lerp_concentration = new API.Lerp(
          _requestIndex: 0,
          _Function: functionType,
          _interval: speedInterval,
          _currValue: concentrationCurrentValue,
          _targetValue: value,
          _callback: lerpGetter
          );

            Request(lerp_concentration);

        }
        else if (index == 1)
        {
            API.Lerp lerp_excitement = new API.Lerp(
          _requestIndex: 1,
          _Function: functionType,
          _interval: speedInterval,
          _currValue: excitementCurrentValue,
          _targetValue: value,
          _callback: lerpGetter
          );

            Request(lerp_excitement);
        }


    }
    #endregion

    public void Request(API.Lerp api)
    {
        Manager.LerpManager.Instance.Request(api);
    }

    public void Receive(API.Lerp api)
    {


        float concentrationValue = 0;
        float excitementValue = 0;
        float positiveValue = 0;
        float sympathyValue = 0;

        // 요청 인덱스는 int 값이기만 하면 제한없이 사용 가능합니다. (예시용으로 0, 1, 2만 넣어둠)
        if (api.RequestIndex == 0)
        {
            concentrationValue = api.Value;

            //Debug.Log("concentration : " + concentrationValue);
            // mapping parameter
            for (int i = 0; i < MappingParameter.instance.matchType.Length; i++)
            {
                // 집중으로 맵핑된 항목 찾기
                if (MappingParameter.instance.matchType[i] == 1)
                {
                    // geo
                    if (i == 0)
                    {
                        MappingParameter.instance.GetGeoValueFromLerp(concentrationValue);
                    }
                    else if (i == 1) // color
                    {
                        MappingParameter.instance.SpeedColorSetColor(concentrationValue);
                        MappingParameter.instance.LerpColorSpeedSetColor(concentrationValue);


                    }
                    else if (i == 2) // speed
                    {
                        //MappingParameter.instance.SetSpeedValueWaitingMeeting(concentrationValue);


                    }

                }
            }


        }
        else if (api.RequestIndex == 1)
        {
            excitementValue = api.Value;
            //Debug.Log("excitement : " + excitementValue);

            // mapping parameter
            for (int i = 0; i < MappingParameter.instance.matchType.Length; i++)
            {
                // 흥분으로 맵핑된 항목 찾기
                if (MappingParameter.instance.matchType[i] == 2)
                {
                    // geo
                    if (i == 0)
                    {
                        MappingParameter.instance.GetGeoValueFromLerp(excitementValue);
                    }
                    else if (i == 1) // color
                    {
                        //MappingParameter.instance.SpeedColorSetColor(excitementValue);
                        //MappingParameter.instance.LerpColorSpeedSetColor(excitementValue);
                        MappingParameter.instance.LerpColorWaitingCube(excitementValue);


                    }
                    else if (i == 2) // speed
                    {
                        //MappingParameter.instance.SetSpeedValueWaitingMeeting(excitementValue);


                    }

                }
            }
        }
        else if (api.RequestIndex == 2)
        {
            positiveValue = api.Value;
            //Debug.Log("positive : " + positiveValue);

            // mapping parameter
            for (int i = 0; i < MappingParameter.instance.matchType.Length; i++)
            {
                // positive으로 맵핑된 항목 찾기
                if (MappingParameter.instance.matchType[i] == 3)
                {
                    // geo
                    if (i == 0)
                    {
                        MappingParameter.instance.GetGeoValueFromLerp(positiveValue);
                    }
                    else if (i == 1) // color
                    {
                        //MappingParameter.instance.SpeedColorSetColor(excitementValue);
                        //MappingParameter.instance.LerpColorSpeedSetColor(excitementValue);
                        MappingParameter.instance.LerpColorWaitingCube(positiveValue);


                    }
                    else if (i == 2) // speed
                    {
                        //MappingParameter.instance.SetSpeedValueWaitingMeeting(positiveValue);


                    }

                }
            }
        }
        else if (api.RequestIndex == 3)
        {
            if (vfxOn)
            {
                sympathyValue = api.Value;
                MappingParameter.instance.SetVFXValueMeeting(sympathyValue);
            }
           
        }
    }
}
