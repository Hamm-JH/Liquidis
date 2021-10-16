using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class SelectManager : MonoBehaviour
{

    public UnityAction<API.Brainwave> biGetter;
    UnityAction<API.Lerp> lerpGetter;

    public float concentrationCurrentValue = 0f;
    public float excitementCurrentValue = 0f;

    public float speedInterval = 3f;
    API.Lerp.Function functionType;

    [Header("Animator")]
    public Animator lightgroup_ani;

    private static SelectManager _selectManager;
    public static SelectManager instance
    {
        get
        {
            if (_selectManager == null)
                return null;
            else
                return _selectManager;
        }
        
    }

    void Awake()
    {
        if (_selectManager == null)
            _selectManager = this;
    }


    public void SetIntervalType(float _interval, int type)
    {
        speedInterval = _interval;

        // 0 = linear
        // 1 = log
        // 2 = power

       
        if(type == 0)
        {
            functionType = API.Lerp.Function.Linear;

        }else if(type == 1){

            functionType = API.Lerp.Function.Log;

        }else if(type == 2)
        {
            functionType = API.Lerp.Function.Power;

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        // �̺�Ʈ�� �޼��� ����
        biGetter += Receive;

        lerpGetter += Receive;


        // animation
        StartSelectAniSequence();
    }


    void StartSelectAniSequence()
    {
        lightgroup_ani.SetTrigger("SceneStart");
    }
    private void Update()
    {
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

        //#region 1. demo ��ҿ� ���� �¿� ���� ����

        //API.Lerp lerp_concentration = new API.Lerp(
        //    _requestIndex: 0,
        //    _Function: API.Lerp.Function.Log,
        //    _interval: speedInterval,
        //    _currValue: concentrationCurrentValue,
        //    _targetValue: (Random.value * 2) - 1,
        //    _callback: lerpGetter
        //    ) ;

        //Request(lerp_concentration);

        //#endregion

        //#region 2. demo ��ҿ� ���� ���� ���� ����

        //API.Lerp lerp_excitement = new API.Lerp(
        //    _requestIndex: 1,
        //    _Function: API.Lerp.Function.Log,
        //    _interval: speedInterval,
        //    _currValue: excitementCurrentValue,
        //    _targetValue: (Random.value * 2) - 1,
        //    _callback: lerpGetter
        //    );

        //Request(lerp_excitement);

        //#endregion

        //#region 3. demo ��ҿ� ���� �յ� ���� ����

        //API.Lerp lerp_sphereFrontBack = new API.Lerp(
        //    _requestIndex: 2,
        //    _Function: API.Lerp.Function.Log,
        //    _interval: speedValue,
        //    _currValue: demoSphere.position.z,
        //    _targetValue: (Random.value * 2) - 1,
        //    _callback: lerpGetter
        //    );

        //Request(lerp_sphereFrontBack);

        //#endregion
    }

    /// <summary>
    /// ���� ������ ������(BIManager)�� ������ ���� ��û�� �Ѵ�.
    /// </summary>
    /// <param name="api"></param>
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
        if(index == 0)
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

        }else if(index == 1)
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

        // ��û �ε����� int ���̱⸸ �ϸ� ���Ѿ��� ��� �����մϴ�. (���ÿ����� 0, 1, 2�� �־��)
        if (api.RequestIndex == 0)
        {
            concentrationValue = api.Value;
            Debug.Log("concentration : " + concentrationValue);
            // mapping parameter
            for(int i=0; i<MappingParameter.instance.matchType.Length; i++)
            {
                // �������� ���ε� �׸� ã��
                if (MappingParameter.instance.matchType[i] == 1)
                {
                    // geo
                    if(i== 0)
                    {
                        MappingParameter.instance.GetGeoValueFromLerp(concentrationValue);
                    }
                    else if(i == 1) // color
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

                }
            }
        }
        else if (api.RequestIndex == 2)
        {
            //vfxValue = api.Value;
        }

    }
}
