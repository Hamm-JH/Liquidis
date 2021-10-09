using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace API
{
	using UnityEngine.Events;

	public class Lerp : _API
	{
		public enum Function
		{
			Linear = 0,
			Log = 1,
			Power = 2
		}

		public Lerp(int _requestIndex, Function _Function, float _interval,
			float _currValue, float _targetValue, UnityAction<Lerp> _callback)
		{
			requestIndex = _requestIndex;
			option = _Function;
			intervalTime = _interval;
			currentValue = _currValue;
			targetValue = _targetValue;
			targetCallback = _callback;

			value = currentValue;

			if(intervalTime <= 0)
			{
				throw new System.Exception("[API.Lerp] ���� ������ �Ҵ� ���� : �ð����ݰ��� �ּ��� 0 �ʰ����� �մϴ�.");
			}
			if(targetCallback == null)
			{
				throw new System.Exception("[API.Lerp] �ݹ� �Ҵ� ���� : �ݹ� �̺�Ʈ�� ������ �Ҵ�Ǿ�� �մϴ�.");
			}
		}

		public void Set(float _value)
		{
			value = _value;
		}

		/// <summary>
		/// ���� ��û �ε���
		/// </summary>
		private int requestIndex;
		/// <summary>
		/// ���� ������ ����
		/// </summary>
		private Function option;
		/// <summary>
		/// curr -> target ������Ʈ ������ �ð����ݰ�
		/// </summary>
		private float intervalTime;
		/// <summary>
		/// ������ ������Ʈ �̺�Ʈ
		/// </summary>
		private UnityAction<Lerp> targetCallback;

		/// <summary>
		/// ��û ����� �����Ͱ�
		/// </summary>
		private float currentValue;
		/// <summary>
		/// ��û ����� ��ǥ��
		/// </summary>
		private float targetValue;
		/// <summary>
		/// ������Ʈ�� ������
		/// </summary>
		private float value;

		public int RequestIndex { get => requestIndex; }
		public Function Option { get => option; }
		public float IntervalTime { get => intervalTime; }
		public UnityAction<Lerp> TargetCallback { get => targetCallback; }


		public float CurrentValue { get => currentValue; }
		public float TargetValue { get => targetValue; }
		public float Value { get => value; }
	}
}
