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
        // ������ ��û�� api�� �����Ѵ�.
        // ���� ������ api ������ �ʼ� �Ҵ� ������
        // 1. targetId : ��õ左�� 6�� �������� �� ���� ����������
        // 2. targetSecond : �޼��� ���� �������� ���� n�ʱ����� ������ ����
        // 3. ������ ���� �Ϸ�� ������ �̺�Ʈ
        API.Brainwave api = new API.Brainwave(
            targetId: Looxid.Link.EEGSensorID.AF3,
            targetSecond: 10,
            targetCallBack: biGetter
            );

        Request(api);
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
        string str = "";
        str += $"Sensor ID : {api.Id.ToString()}\n";
        str += $"interval seconds : {api.Second.ToString()}\n";

        str += $"Delta : {api.Delta}\n";
        str += $"Delta : {api.Theta}\n";
        str += $"Delta : {api.Alpha}\n";
        str += $"Delta : {api.Beta} \n";
        str += $"Delta : {api.Gamma}\n";

        Debug.Log(str);
    }
}
