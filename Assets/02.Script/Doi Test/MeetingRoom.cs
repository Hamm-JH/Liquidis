using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Events;

public class MeetingRoom : MonoBehaviour
{
    public GameObject meetingHead;
    public VisualEffect vfxObject;

    public UnityAction<API.Brainwave> biGetter;

    [Header("Ani")]
    public GameObject counterHead_ani;
    public GameObject timer;
    public float timerStartTime = 2f;
    public float currentSpeedValue = 0f;
    public float targetSpeedLerp = 10f;
    public float speedLerp = 2f;
    public float[] duration;
    public GameObject volumeObj;

    public Animator mirror_room_ani;
    public AnimationClip timerBox_animation;
   

    [Header("General Value")]
    float currentTime = 0f;
    bool timeStart = false;
    float time_5min = 300f;
    bool pass5min = false;
    float time_10min = 600f;
    bool pass10min = false;
    float time_15min = 900f;
    bool pass15min = false;
    public float concentrationCurrentValue = 0f;
    public float excitementCurrentValue = 0f;

    UnityAction<API.Lerp> lerpGetter;
    public float speedInterval = 3f;
    API.Lerp.Function functionType;


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

    private void Start()
    {
        StartCoroutine(InitialMappingParameters());

        // 이벤트에 메서드 연결
        biGetter += Receive;

        lerpGetter += Receive;

        // ani sequence
        counterHead_ani.GetComponent<Animator>().SetTrigger("LightOn");
        StartCoroutine(StartTimerAni());
    }

    IEnumerator InitialMappingParameters()
    {
        yield return new WaitForEndOfFrame();
        MappingParameter.instance.InitialMeeintRoomParameters(meetingHead, vfxObject);
        

    }

    public void SetIntervalType(float _interval, int type)
    {
        speedInterval = _interval;

        // 0 = linear
        // 1 = log
        // 2 = power


        if (type == 0)
        {
            functionType = API.Lerp.Function.Linear;

        }
        else if (type == 1)
        {

            functionType = API.Lerp.Function.Log;

        }
        else if (type == 2)
        {
            functionType = API.Lerp.Function.Power;

        }
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
            float concentrationValue = 0;
            float excitementValue = 0;


            if (api.Option == 0)
            {
                //Debug.Log($"Relaxation: {api.Relaxation.ToString()}");
                excitementCurrentValue = api.Relaxation;
                float current = excitementCurrentValue;
                float target = api.Relaxation;

                Request(1, current, target);


            }
            else if (api.Option == 1)
            {
                //Debug.Log($"Attention : {api.Attention.ToString()}");
                concentrationCurrentValue = api.Attention;

                float current = concentrationCurrentValue;
                float target = api.Attention;

                Request(0, current, target);

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

    }

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

    public void Request(API.Lerp api)
    {
        Manager.LerpManager.Instance.Request(api);
    }

    public void Receive(API.Lerp api)
    {


        float concentrationValue = 0;
        float excitementValue = 0;

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

                }
            }
        }
        else if (api.RequestIndex == 2)
        {
            //vfxValue = api.Value;
        }

    }
    IEnumerator StartTimerAni()
    {
        yield return new WaitForSeconds(timerStartTime);

        timer.SetActive(true);
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
        Debug.LogError("ending start");
        StartCoroutine(EndingAniSequence());
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

        yield return new WaitForSeconds(duration[0]);

        mirror_room_ani.SetTrigger("MirrorRoomShake");

        yield return new WaitForSeconds(duration[1]);

        volumeObj.GetComponent<VolumeManager_Duru>().isEnd = true;
        // target duration 설정은 volume manager 외부에서 직접 쓰기

        yield return new WaitForSeconds(duration[2]);

        counterHead_ani.GetComponent<Animator>().SetTrigger("PlayerHeadOut");

        yield return new WaitForSeconds(duration[3]);

        counterHead_ani.GetComponent<Animator>().SetTrigger("CounterHeadBye");

        yield return new WaitForSeconds(duration[4]);

        // 인트로씬 로드
    }
}
