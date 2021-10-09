using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class BIDemo : MonoBehaviour
{
    /// <summary>
    /// BIManager���� ������ ���� �Ϸ�� �����͸� ���� �̺�Ʈ
    /// </summary>
    public UnityAction<API.Brainwave> biGetter;

    // Start is called before the first frame update
    void Start()
    {
        // �̺�Ʈ�� �޼��� ����
        biGetter += Receive;
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
        if(api.Objective == API.Objective.EEG)
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
        else if(api.Objective == API.Objective.Relaxation)
		{
            Debug.Log($"Relaxation : {api.Relaxation.ToString()}");
		}
        else if(api.Objective == API.Objective.Attention)
		{
            Debug.Log($"Attention : {api.Attention.ToString()}");
		}
        else if(api.Objective == API.Objective.EEGRandom)
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
        else if(api.Objective == API.Objective.MindRandom)
		{
            if(api.Option == 0)
			{
                Debug.Log($"Relaxation: {api.Relaxation.ToString()}");
			}
            else if(api.Option == 1)
			{
                Debug.Log($"Attention : {api.Attention.ToString()}");
            }
		}
    }
}
