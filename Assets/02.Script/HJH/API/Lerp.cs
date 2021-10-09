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
				throw new System.Exception("[API.Lerp] 러프 생성자 할당 에러 : 시간간격값은 최소한 0 초과여야 합니다.");
			}
			if(targetCallback == null)
			{
				throw new System.Exception("[API.Lerp] 콜백 할당 에러 : 콜백 이벤트는 무조건 할당되어야 합니다.");
			}
		}

		public void Set(float _value)
		{
			value = _value;
		}

		/// <summary>
		/// 러프 요청 인덱스
		/// </summary>
		private int requestIndex;
		/// <summary>
		/// 러프 움직임 설정
		/// </summary>
		private Function option;
		/// <summary>
		/// curr -> target 업데이트 사이의 시간간격값
		/// </summary>
		private float intervalTime;
		/// <summary>
		/// 데이터 업데이트 이벤트
		/// </summary>
		private UnityAction<Lerp> targetCallback;

		/// <summary>
		/// 요청 당시의 데이터값
		/// </summary>
		private float currentValue;
		/// <summary>
		/// 요청 당시의 목표값
		/// </summary>
		private float targetValue;
		/// <summary>
		/// 업데이트된 데이터
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
