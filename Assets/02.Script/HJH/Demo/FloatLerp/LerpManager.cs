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

		// 러프 요청을 받는다.

		private Dictionary<int, Coroutine> indexes;

		private void Awake()
		{
			indexes = new Dictionary<int, Coroutine>();
		}

		public void Request(API.Lerp api)
		{
			// 요청 인덱스에 이미 먼저 작동중인 루틴이 있을경우 (중간 신규입력 차단)
			if(indexes.ContainsKey(api.RequestIndex))
			{
				//Debug.LogWarning($"index {api.RequestIndex}에 대한 러프가 진행중입니다");
				return;
			}
			// 요청 인덱스에 대응하는 루틴이 없는경우 (신규 입력)
			else
			{
				//Debug.Log($"index {api.RequestIndex} 작업 시작");
				indexes.Add(api.RequestIndex, StartCoroutine(Lerp(api)));
			}
		}

		private IEnumerator Lerp(API.Lerp api)
		{
			bool isRunning = true;

			float fromValue = api.CurrentValue;		// 시작값
			float toValue = api.TargetValue;        // 목표값
			float between = toValue - fromValue;    // from에서 to사이값
			//float between = Mathf.Abs(Mathf.Abs(toValue) - Mathf.Abs(fromValue));    // from에서 to사이값

			float timer = 0f;       // 내부 타이머
			float interval = api.IntervalTime;      // 시간간격값

			API.Lerp.Function func = api.Option;	// 러프 구현옵션

			while(isRunning)
			{
				yield return null;

				timer = timer + Time.deltaTime;

				// 타이머가 시간간격값을 넘으면 종료
				if(timer >= interval)
				{
					timer = interval;
					isRunning = false;
				}

				// 타이머 업데이트 (간격값 넘으면 간격값으로 고정, 아니면 업데이트값 할당)
				//timer = (timer + Time.deltaTime >= interval) ? interval : timer + Time.deltaTime;

				float lerpValue = between * MathFunction(api.Option, timer / interval);

				fromValue = api.CurrentValue + lerpValue;  // 러프값 내부 업데이트
				api.Set(fromValue);                 // 러프값 외부 이벤트 전달준비

				api.TargetCallback.Invoke(api);		// 데이터 외부 업데이트
			}

			// 러프 종료
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

			// 용도 변경에 대비한 기본값(로그) 세팅
			return Log(_value);
		}

		#region 러프용 수학 함수

		/// <summary>
		/// 선형변환
		/// </summary>
		/// <param name="_value"></param>
		/// <returns></returns>
		private float Linear(float _value)
		{
			return _value;
		}

		/// <summary>
		/// 로그6 함수
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
		/// n제곱 함수
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
