using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Events;
using Photon.Pun;

public class MeetingRoom : MonoBehaviour
{
    public GameObject meetingHead;//playerHead�� ��ü 
    public VisualEffect vfxObject;//������ vfx

    public UnityAction<API.Brainwave> biGetter; //���� ������

    [Header("Ani")]
    public GameObject counterHead_ani; //ī���� ���
    public GameObject timer; //Ÿ�̸� ������Ʈ
    public GameObject timer_canvas;
    
    public float currentSpeedValue = 0f; //���� �ӵ���
    public float targetSpeedLerp = 10f; //��ǥ�ӵ�����
    public float speedLerp = 2f; //�ӵ� ����
    public float[] duration; //����ð� �迭
    public GameObject volumeObj; //����Ʈ ���μ���
     
    public Animator mirror_room_ani; //�̷��� �ִ�
    public Animator camera_rig_ani;

    [Header("Ani Time")]
    public float afterMirror = 2f;
    public float afterPlayerLight = 1f;
    public float afterPlayerHeadStart = 1f;
    public float timerStartTime = 5f; //Ÿ�̸� ���� �ð�


    [Header("General Value")]
    float currentTime = 0f;//���� �ð�.
    bool timeStart = false; 
    float time_5min = 300f; //5��
    bool pass5min = false;
    float time_10min = 600f;//10��
    bool pass10min = false; 
    float time_15min = 900f; //15��
    bool pass15min = false;
    public float concentrationCurrentValue = 0f;//�������簪
    public float excitementCurrentValue = 0f;//������簪
    public float positiveCurrentValue = 0f;
    public float sympathyCurrentValue = 0f;

    UnityAction<API.Lerp> lerpGetter;//����
    public float speedInterval = 3f; //�ӵ����͹�
    API.Lerp.Function functionType; //����Ÿ��

    public int playerNum = 0;

    // ���� �����ϸ� �ð� ���, 5��, 10���� �Ǹ� Ÿ�̸� �ڽ� �ִ� Ʈ����
    // Ÿ�̸� �ڽ� �ִϿ� �����ؼ� �θ���


    // ���� ���� �ϸ�
    // TimerGameOver_Shake �ϳ��� ������.(Trigger: GameOverShake)
    // TimerBox �ִϿ��� timer loop ����
    // 

    // ������ ����
    // speed interval current -> targetSpeed �� lerp (N�ʵ��� lerp)(10��)
    // geo �� ���� -> targetValue�� �ٲ�(2�� 3)
    // ������ duration �Ŀ� (2��)
    // ���ÿ� MirrorRoom - Set trigger: MirrorRoomShake
    // ������ duration �Ŀ� (2��)
    // Volume �� VolumeManager���� isEnd = true �� targetDuration ���� �ʿ�
    // ������ duration �Ŀ� (2��)
    // CounterHead(Set tirgger: PlayerHeadOut)
    // ������ duration �Ŀ� (1��)
    // CounterHead(Set trigger: CounterHeadBye)
    // ������ duration �Ŀ� (2��)
    // ��Ʈ�ξ� �ε�


    // VFX�� ���� ����
    // CounterHead>MirrorGoup>Mirror1,2,3�� ���̴� ������Ƽ�� ����. 
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

        // �̺�Ʈ�� �޼��� ����
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

        #region Ÿ�̸� �ð� ���
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

        #region 1. EEG ������ ��û
        // ������ ��û�� api�� �����Ѵ�.
        // ���� ������ api ������ �ʼ� �Ҵ� ������
        // 1. targetId : ��õ左�� 6�� �������� �� ���� ����������
        // 2. targetSecond : �޼��� ���� �������� ���� n�ʱ����� ������ ����
        // 3. ������ ���� �Ϸ�� ������ �̺�Ʈ
        API.Brainwave api_EEG = new API.Brainwave(
            obj: API.Objective.EEG,
            targetId: Looxid.Link.EEGSensorID.AF3,
            targetSecond: 10,
            targetCallBack: biGetter
            );

        //Request(api_EEG);
        #endregion

        #region 2. �������� ������ ��û

        API.Brainwave api_relaxation = new API.Brainwave(
            obj: API.Objective.Relaxation,
            targetCallBack: biGetter
            );

        //Request(api_relaxation);
        #endregion

        #region 3. ���߻��� ������ ��û

        API.Brainwave api_attention = new API.Brainwave(
            obj: API.Objective.Attention,
            targetCallBack: biGetter
            );

        //Request(api_attention);
        #endregion

        #region 4. ������ ���� EEG

        API.Brainwave api_randomEEG = new API.Brainwave(
            obj: API.Objective.EEGRandom,
            targetCallBack: biGetter
            );

        Request(api_randomEEG);
        #endregion

        #region 5. ������ �̿� ���� ���ε尪

        API.Brainwave api_randomMindRelaxation = new API.Brainwave(
            obj: API.Objective.MindRandom,
            option: 0,
            targetCallBack: biGetter
            );

        Request(api_randomMindRelaxation);
        #endregion

        #region 6. ������ ���� ���� ���ε尪

        API.Brainwave api_randomMindAttention = new API.Brainwave(
            obj: API.Objective.MindRandom,
            option: 1,
            targetCallBack: biGetter
            );

        Request(api_randomMindAttention);

        #endregion

        #region 7. ������ ����� ���� ���ε尪

        API.Brainwave api_randomMindPositive = new API.Brainwave(
            obj: API.Objective.MindRandom,
            option: 0,
            targetCallBack: biGetter
            );

        Request(api_randomMindAttention);

        #endregion


        #region 8. ������ ���� ���� ���ε尪

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
    /// �̺�Ʈ�� ����� ������ ���� �޼���
    /// ���� �Ϸ�� �����͸� ����Ѵ�.
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

                // ���Ⱑ �� �߸��� �� ������
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
                //// ���Ⱑ �� �߸��� �� ������
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

        // ��û �ε����� int ���̱⸸ �ϸ� ���Ѿ��� ��� �����մϴ�. (���ÿ����� 0, 1, 2�� �־��)
        if (api.RequestIndex == 0)
        {
            concentrationValue = api.Value;
            // mapping parameter
            for (int i = 0; i < MappingParameter.instance.matchType.Length; i++)
            {
                // �������� ���ε� �׸� ã��
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
                // ������� ���ε� �׸� ã��
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
                // ��������� ���ε� �׸� ã��
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
        //������
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
        // ���� ���� �ϸ�
        // TimerGameOver_Shake �ϳ��� ������.(Trigger: GameOverShake)
        // TimerBox �ִϿ��� timer loop ����
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

        mirror_room_ani.SetTrigger("MirrorRoomShake");//�̷������ũ

        yield return new WaitForSeconds(duration[1]);//�̷������ũ>����Ʈ���μ���

        volumeObj.GetComponent<VolumeManager_Duru>().isEnd = true;
        // target duration ������ volume manager �ܺο��� ���� ����

        yield return new WaitForSeconds(duration[2]);//����Ʈ ���μ���>�÷��̾����ƿ�

        counterHead_ani.GetComponent<Animator>().SetTrigger("PlayerHeadOut");//�÷��̾����ƿ�

        yield return new WaitForSeconds(duration[3]);//�÷��̾����ƿ�>ī����������

        counterHead_ani.GetComponent<Animator>().SetTrigger("CounterHeadBye");//ī����������

        yield return new WaitForSeconds(duration[4]);//ī����������>��Ʈ�ξ��ε�

        // ��Ʈ�ξ� �ε�
    }
}
