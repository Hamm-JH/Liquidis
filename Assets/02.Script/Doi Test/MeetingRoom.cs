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


    private void Start()
    {
        StartCoroutine(InitialMappingParameters());

        biGetter += Receive;
    }

    IEnumerator InitialMappingParameters()
    {
        yield return new WaitForEndOfFrame();
        MappingParameter.instance.InitialMeeintRoomParameters(meetingHead, vfxObject);
        

    }
    private void Update()
    {
        #region EEG ������ ��û
        // ������ ��û�� api�� �����Ѵ�.
        // ���� ������ api ������ �ʼ� �Ҵ� ������
        // 1. targetId : ��õ左�� 6�� �������� �� ���� ����������
        // 2. targetSecond : �޼��� ���� �������� ���� n�ʱ����� ������ ����
        // 3. ������ ���� �Ϸ�� ������ �̺�Ʈ
        API.Brainwave api = new API.Brainwave(
            obj: API.Objective.EEG,
            targetId: Looxid.Link.EEGSensorID.AF3,
            targetSecond: 10,
            targetCallBack: biGetter
            );

        Request(api);
        #endregion

        #region �������� ������ ��û

        API.Brainwave api_relaxation = new API.Brainwave(
            obj: API.Objective.Relaxation,
            targetCallBack: biGetter
            );

        Request(api_relaxation);
        #endregion

        #region ���߻��� ������ ��û

        API.Brainwave api_attention = new API.Brainwave(
            obj: API.Objective.Attention,
            targetCallBack: biGetter
            );

        Request(api_attention);
        #endregion
    }

    /// <summary>
	/// ���� ������ ������(BIManager)�� ������ ���� ��û�� �Ѵ�.
	/// </summary>
	/// <param name="api"></param>
	private void Request(API.Brainwave api)
    {
        Manager.BIManager.Instance.Request(api);
    }

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

            Debug.Log(str);
        }
        else if (api.Objective == API.Objective.Relaxation)
        {
            Debug.Log($"Relaxation : {api.Relaxation.ToString()}");
            
            // mapping parameter
        }
        else if (api.Objective == API.Objective.Attention)
        {
            Debug.Log($"Attention : {api.Attention.ToString()}");
        }
    }

}
