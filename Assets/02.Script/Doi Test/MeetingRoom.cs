using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Events;
using Photon.Pun;

public class MeetingRoom : MonoBehaviour
{
    public GameObject meetingHead;//playerHead의 본체 
    public VisualEffect vfxObject;//공감용 vfx

    public UnityAction<API.Brainwave> biGetter; //뇌파 데이터

    [Header("Ani")]
    public GameObject counterHead_ani; //카운터 헤드
    public GameObject timer; //타이머 오브젝트
    public GameObject timer_canvas;
    
    public float currentSpeedValue = 0f; //현재 속도값
    public float targetSpeedLerp = 10f; //목표속도러프
    public float speedLerp = 2f; //속도 러프
    public float[] duration; //경과시간 배열
    public GameObject volumeObj; //포스트 프로세싱
     
    public Animator mirror_room_ani; //미러룸 애니
    public Animator camera_rig_ani;

    [Header("Ani Time")]
    public float afterMirror = 2f;
    public float afterPlayerLight = 1f;
    public float afterPlayerHeadStart = 1f;
    public float timerStartTime = 5f; //타이머 시작 시간


    [Header("General Value")]
    float currentTime = 0f;//현재 시간.
    bool timeStart = false; 
    float time_5min = 300f; //5분
    bool pass5min = false;
    float time_10min = 600f;//10분
    bool pass10min = false; 
    float time_15min = 900f; //15분
    bool pass15min = false;
    public float concentrationCurrentValue = 0f;//집중현재값
    public float excitementCurrentValue = 0f;//흥분현재값
    public float positiveCurrentValue = 0f;
    public float sympathyCurrentValue = 0f;

    UnityAction<API.Lerp> lerpGetter;//러프
    public float speedInterval = 3f; //속도인터벌
    API.Lerp.Function functionType; //러프타입

    public int playerNum = 0;

    // 게임 시작하면 시간 재기, 5분, 10분이 되면 타이머 박스 애니 트리거
    // 타이머 박스 애니에 접근해서 부르기


    // 게임 강종 하면
    // TimerGameOver_Shake 하나로 끝낸다.(Trigger: GameOverShake)
    // TimerBox 애니에서 timer loop 끈다
    // 

    // 마지막 엔딩
    // speed interval current -> targetSpeed 로 lerp (N초동안 lerp)(10초)
    // geo 값 변경 -> targetValue로 바꿈(2나 3)
    // 임의의 duration 후에 (2초)
    // 동시에 MirrorRoom - Set trigger: MirrorRoomShake
    // 임의의 duration 후에 (2초)
    // Volume 에 VolumeManager에서 isEnd = true 로 targetDuration 설정 필요
    // 임의의 duration 후에 (2초)
    // CounterHead(Set tirgger: PlayerHeadOut)
    // 임의의 duration 후에 (1초)
    // CounterHead(Set trigger: CounterHeadBye)
    // 임의의 duration 후에 (2초)
    // 인트로씬 로드


    // VFX랑 같이 맵핑
    // CounterHead>MirrorGoup>Mirror1,2,3의 셰이더 프로퍼티에 접근. 
    // <Renderer>().SetFloat  "Reflection Intensity" : 0~1 .

    private static MeetingRoom _meetingRoom;
    public static MeetingRoom instance
    {
        get
        {
            if (_meetingRoom == null)
                return null;
            else
                return _meetingRoom;
        }
    }

    private void Awake()
    {
        if (_meetingRoom == null)
            _meetingRoom = this;
    }
    private void Start()
    {
        StartCoroutine(InitialMappingParameters());//?

        // 이벤트에 메서드 연결
        biGetter += Receive;

        lerpGetter += Receive;

        // ani sequence
        StartCoroutine(PlayerLightSequence());

      

        // waiting room 
        //playerNum = PhotonNetwork.LocalPlayer.ActorNumber - 1;
       
    }

  
    IEnumerator InitialMappingParameters()
    {
        yield return new WaitForEndOfFrame();
        MappingParameter.instance.InitialMeeintRoomParameters(meetingHead, vfxObject);
        

    }

   IEnumerator PlayerLightSequence()
    {
        yield return new WaitForSeconds(afterMirror);
        counterHead_ani.GetComponent<Animator>().SetTrigger("PlayerLight");

        yield return new WaitForSeconds(afterPlayerLight);
        camera_rig_ani.SetTrigger("PlayerHeadStart");

        yield return new WaitForSeconds(afterPlayerHeadStart);
        counterHead_ani.GetComponent<Animator>().SetTrigger("LightOn");
        mirror_room_ani.SetTrigger("LightOn");

        yield return new WaitForSeconds(timerStartTime);
        timer.SetActive(true);
        timer_canvas.SetActive(true);
    }

    //IEnumerator StartTimerAni()
    //{
    //    yield return new WaitForSeconds(timerStartTime);

        
    //    // timer.GetComponent<Animator>().SetTrigger("TimerLoop");
    //}


    public void SetIntervalType(float _interval)
    {
        speedInterval = _interval;

      
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndingAniStart();
        }

        #region 타이머 시간 재기
        if (timeStart)
        {
            currentTime += Time.deltaTime;

            if(currentTime >= time_5min)
            {
                if (!pass5min)
                {
                    pass5min = true;
                }
            }

            if (currentTime >= time_10min)
            {
                if (!pass10min)
                {
                    pass10min = true;
                }
            }

            if (currentTime >= time_15min)
            {
                if (!pass15min)
                {
                    pass15min = true;
                }
            }
        }

        #endregion

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

        #region 7. 디버깅용 긍부정 랜덤 마인드값

        API.Brainwave api_randomMindPositive = new API.Brainwave(
            obj: API.Objective.MindRandom,
            option: 0,
            targetCallBack: biGetter
            );

        Request(api_randomMindAttention);

        #endregion


        #region 8. 디버깅용 공감 랜덤 마인드값

        API.Brainwave api_randomMindSympathy = new API.Brainwave(
            obj: API.Objective.MindRandom,
            option: 1,
            targetCallBack: biGetter
            );

        Request(api_randomMindAttention);

        #endregion
    }
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

            //excitementCurrentValue = api.Relaxation;
            float current = excitementCurrentValue;
            float target = api.Relaxation;

            Request(1, current, target);

        }
        else if (api.Objective == API.Objective.Attention)
        {
            //Debug.Log($"Attention : {api.Attention.ToString()}");

            //concentrationCurrentValue = api.Attention;
            float current = concentrationCurrentValue;
            float target = api.Attention;

            Request(0, current, target);
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
            float concentrationValue = 0;
            float excitementValue = 0;


            if (api.Option == 0)
            {
                //Debug.Log($"Relaxation: {api.Relaxation.ToString()}");
                //excitementCurrentValue = api.Relaxation;
                //float current = excitementCurrentValue;
                //float target = api.Relaxation;

                //Request(1, current, target);

                // 여기가 몬가 잘못된 것 같은데
                Debug.Log($"Positive : {api.Attention.ToString()}");

                //positiveCurrentValue = api.Attention;

                float current = positiveCurrentValue;
                float target = api.Attention;

                Request(2, current, target);

             
            }
            else if (api.Option == 1)
            {
                //Debug.Log($"Attention : {api.Attention.ToString()}");
                //concentrationCurrentValue = api.Attention;

                //float current = concentrationCurrentValue;
                //float target = api.Attention;

                //Request(0, current, target);

                Debug.Log($"Sympathy : {api.Attention.ToString()}");

                //sympathyCurrentValue = api.Attention;

                float current = sympathyCurrentValue;
                float target = api.Attention;

                Request(3, current, target);
            }
            else if(api.Option == 2)
            {
                //// 여기가 몬가 잘못된 것 같은데
                //Debug.Log($"Positive : {api.Attention.ToString()}");

                ////positiveCurrentValue = api.Attention;

                //float current = positiveCurrentValue;
                //float target = api.Attention;

                //Request(2, current, target);

            }
            else if(api.Option == 3)
            {
                //Debug.Log($"Sympathy : {api.Attention.ToString()}");

                ////sympathyCurrentValue = api.Attention;

                //float current = sympathyCurrentValue = api.Attention;
                //float target = api.Attention;

                //Request(3, current, target);
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
        }else if (index == 2)
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

    //public void Request(int index, float value)
    //{
    //    if (index == 0)
    //    {
    //        API.Lerp lerp_concentration = new API.Lerp(
    //      _requestIndex: 0,
    //      _Function: functionType,
    //      _interval: speedInterval,
    //      _currValue: concentrationCurrentValue,
    //      _targetValue: value,
    //      _callback: lerpGetter
    //      );

    //        Request(lerp_concentration);

    //    }
    //    else if (index == 1)
    //    {
    //        API.Lerp lerp_excitement = new API.Lerp(
    //      _requestIndex: 1,
    //      _Function: functionType,
    //      _interval: speedInterval,
    //      _currValue: excitementCurrentValue,
    //      _targetValue: value,
    //      _callback: lerpGetter
    //      );

    //        Request(lerp_excitement);
    //    }
      

    //}

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
                        MappingParameter.instance.SetSpeedValueWaitingMeeting(concentrationValue);
                    }
                }
            }


        }
        else if (api.RequestIndex == 1)
        {
            excitementValue = api.Value;
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
                        MappingParameter.instance.SpeedColorSetColor(excitementValue);
                        MappingParameter.instance.LerpColorSpeedSetColor(excitementValue);


                    }
                    else if (i == 2) // speed
                    {
                        MappingParameter.instance.SetSpeedValueWaitingMeeting(excitementValue);

                    }
                }
            }
        }
        else if (api.RequestIndex == 2)
        {
            positiveCurrentValue = api.Value;
            //positiveValue = api.Value;

            Debug.Log("positive  value : " + positiveCurrentValue);
            // mapping parameter
            for (int i = 0; i < MappingParameter.instance.matchType.Length; i++)
            {
                // 긍부정으로 맵핑된 항목 찾기
                if (MappingParameter.instance.matchType[i] == 2)
                {
                    // geo
                    if (i == 0)
                    {
                        MappingParameter.instance.GetGeoValueFromLerp(positiveCurrentValue);
                    }
                    else if (i == 1) // color
                    {
                        MappingParameter.instance.SpeedColorSetColor(positiveCurrentValue);
                        MappingParameter.instance.LerpColorSpeedSetColor(positiveCurrentValue);


                    }
                    else if (i == 2) // speed
                    {
                        MappingParameter.instance.SetSpeedValueWaitingMeeting(positiveCurrentValue);

                    }
                }
            }
        }
        //공감도
        else if(api.RequestIndex == 3)
        {
            //vfxValue = api.Value;
            //sympathyValue = api.Value;
            sympathyCurrentValue = api.Value;


            DataSyncronize.instance.SetSympathy(playerNum, sympathyCurrentValue);

            //MappingParameter.instance.SetVFXValueMeeting(sympathyCurrentValue);
        }

    }
   

    public void ForceEndGame()
    {
        // 게임 강종 하면
        // TimerGameOver_Shake 하나로 끝낸다.(Trigger: GameOverShake)
        // TimerBox 애니에서 timer loop 끈다
        // 

        timer.GetComponent<Animator>().SetTrigger("GameOverShake");
        //timerBox_animation.isLooping = false;
    }

    void EndingAniStart()
    {
        Debug.Log("ending start");
        StartCoroutine(EndingAniSequence());
        timer.GetComponent<Animator>().SetTrigger("GameOverShake");
    }

    
   
    IEnumerator EndingAniSequence()
    {
        
        bool stopSpeed = true;

        currentSpeedValue = MappingParameter.instance.speedLerpInterval;

        while (stopSpeed)
        {
            currentSpeedValue += Time.deltaTime * speedLerp;

            // mapping parameter


            if(currentSpeedValue > targetSpeedLerp)
            {
                stopSpeed = false;
            }
            yield return null;
        }

        MappingParameter.instance.SetGeoValueMeeting(2f);

        yield return new WaitForSeconds(duration[0]);//?

        mirror_room_ani.SetTrigger("MirrorRoomShake");//미러룸셰이크

        yield return new WaitForSeconds(duration[1]);//미러룸셰이크>포스트프로세스

        volumeObj.GetComponent<VolumeManager_Duru>().isEnd = true;
        // target duration 설정은 volume manager 외부에서 직접 쓰기

        yield return new WaitForSeconds(duration[2]);//포스트 프로세스>플레이어헤드아웃

        counterHead_ani.GetComponent<Animator>().SetTrigger("PlayerHeadOut");//플레이어헤드아웃

        yield return new WaitForSeconds(duration[3]);//플레이어헤드아웃>카운터헤드바이

        counterHead_ani.GetComponent<Animator>().SetTrigger("CounterHeadBye");//카운터헤드바이

        yield return new WaitForSeconds(duration[4]);//카운터헤드바이>인트로씬로드

        // 인트로씬 로드
    }
}
