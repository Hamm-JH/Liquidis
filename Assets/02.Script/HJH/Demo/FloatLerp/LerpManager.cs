using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
	public class LerpManager : MonoBehaviour
	{
		#region Instance

		private static LerpManager instance;

		public static LerpManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<LerpManager>() as LerpManager;
				}
				return instance;
			}
		}

		#endregion

		// ���� ��û�� �޴´�.

		private Dictionary<int, Coroutine> indexes;

		private void Awake()
		{
			indexes = new Dictionary<int, Coroutine>();
		}

		public void Request(API.Lerp api)
		{
			// ��û �ε����� �̹� ���� �۵����� ��ƾ�� ������� (�߰� �ű��Է� ����)
			if(indexes.ContainsKey(api.RequestIndex))
			{
				//Debug.LogWarning($"index {api.RequestIndex}�� ���� ������ �������Դϴ�");
				return;
			}
			// ��û �ε����� �����ϴ� ��ƾ�� ���°�� (�ű� �Է�)
			else
			{
				//Debug.Log($"index {api.RequestIndex} �۾� ����");
				indexes.Add(api.RequestIndex, StartCoroutine(Lerp(api)));
			}
		}

		private IEnumerator Lerp(API.Lerp api)
		{
			bool isRunning = true;

			float fromValue = api.CurrentValue;		// ���۰�
			float toValue = api.TargetValue;        // ��ǥ��
			float between = toValue - fromValue;    // from���� to���̰�
			//float between = Mathf.Abs(Mathf.Abs(toValue) - Mathf.Abs(fromValue));    // from���� to���̰�

			float timer = 0f;       // ���� Ÿ�̸�
			float interval = api.IntervalTime;      // �ð����ݰ�

			API.Lerp.Function func = api.Option;	// ���� �����ɼ�

			while(isRunning)
			{
				yield return null;

				timer = timer + Time.deltaTime;

				// Ÿ�̸Ӱ� �ð����ݰ��� ������ ����
				if(timer >= interval)
				{
					timer = interval;
					isRunning = false;
				}

				// Ÿ�̸� ������Ʈ (���ݰ� ������ ���ݰ����� ����, �ƴϸ� ������Ʈ�� �Ҵ�)
				//timer = (timer + Time.deltaTime >= interval) ? interval : timer + Time.deltaTime;

				float lerpValue = between * MathFunction(api.Option, timer / interval);

				fromValue = api.CurrentValue + lerpValue;  // ������ ���� ������Ʈ
				api.Set(fromValue);                 // ������ �ܺ� �̺�Ʈ �����غ�

				api.TargetCallback.Invoke(api);		// ������ �ܺ� ������Ʈ
			}

			// ���� ����
			indexes.Remove(api.RequestIndex);
			yield break;
		}

		public float MathFunction(API.Lerp.Function function, float _value)
		{
			if(function == API.Lerp.Function.Log)
			{
				return Log(_value);
			}
			else if(function == API.Lerp.Function.Linear)
			{
				return Linear(_value);
			}
			else if(function == API.Lerp.Function.Power)
			{
				return Power(_value);
			}

			// �뵵 ���濡 ����� �⺻��(�α�) ����
			return Log(_value);
		}

		#region ������ ���� �Լ�

		/// <summary>
		/// ������ȯ
		/// </summary>
		/// <param name="_value"></param>
		/// <returns></returns>
		private float Linear(float _value)
		{
			return _value;
		}

		/// <summary>
		/// �α�6 �Լ�
		/// </summary>
		/// <param name="_value"></param>
		/// <returns></returns>
		private float Log(float _value)
		{
			float value = Mathf.Log((_value*63 + 1), 2) / 6;

			return value;

			//Debug.Log(Mathf.Log((_value*63 + 1), 2) / 6);
		}

		/// <summary>
		/// n���� �Լ�
		/// </summary>
		/// <param name="_value"></param>
		/// <returns></returns>
		private float Power(float _value)
		{
			float value = Mathf.Pow(_value * 32, 2) / Mathf.Pow(32, 2);

			return value;

			//Debug.Log(value);
		}

		#endregion
	}
}
