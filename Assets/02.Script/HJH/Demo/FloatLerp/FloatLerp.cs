using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatLerp : MonoBehaviour
{
    // 러프를 실행할 목표 변수, 객체

    [Header("target resource")]
    public GameObject obj;

    public SkinnedMeshRenderer render;
    public Material mat;
    public string targetParameter;

    public bool isUseMat;

    //---------
    // 러프 변수

    [Header("operating value")]
    [SerializeField] private float currentValue;
    public float targetValue;

    public float boundary;      // 러프 재시작 경계값
    public Function function;   // 러프 결정함수

    public float interval;      // 러프 타이밍 간격

    bool isRoutineRunning;      // 루틴 시동관리 변수

    public bool isReached;      // 러프 완료 확인코드


    public enum Function
	{
        Linear,
        Log,
        Power
	}

    /// <summary>
    /// 이 스크립트 활성화시, 러프 코루틴을 활성화한다.
    /// </summary>
	private void OnEnable()
	{
        isRoutineRunning = true;

        StartCoroutine(Lerp());
    }

    /// <summary>
    /// 이 스크립트 비활성화시, 러프 코루틴을 비활성화한다.
    /// </summary>
	private void OnDisable()
	{
        isRoutineRunning = false;
	}

	public IEnumerator Lerp()
    {
        // 지정된 초(second) 동안 from에서 to로 러프 연산

        float from = 0;         // 시작값
        float to = 0;           // 목표값
        float between = 0;      // from에서 to 사이값

        float timer = 0f;       // 내부 타이머값

        float second = interval;    // 시간간격값

        isReached = false;     // 러프값 도달 확인

        Function func = function;
        
        if(mat == null)
		{
            mat = render.material;
		}

        while(true)
		{
            // 특정 값에 도달한 상태
            if(isReached == true)
			{
                float diff = Mathf.Abs(currentValue - targetValue);
                // 차이값이 최소 경계값보다 큰 경우 재 초기화
                if(diff > boundary)
				{
                    isReached = false;

                    // 내부 변수 초기화
                    // 시작 변수값 할당
                    from = mat.GetFloat(targetParameter);

                    to = targetValue;

                    between = to - from;    // 사이값 초기화
                    second = interval;      // 시간간격 업데이트

                    timer = 0;      // 타이머 초기화

                    func = function;    // 러프 결정함수 업데이트
				}
                else
				{
                    // 코루틴 종료 확인
                    if (!isRoutineRunning) break;
                    yield return null;
                    continue;
				}
			}
            // 러프 진행
            else
			{
                // 타이머 업데이트
                timer = (timer + Time.deltaTime >= second) ? second : timer + Time.deltaTime;

                // 타임 오버시 브레이크
                if (timer >= second)    isReached = true;

                // 현재 진행된 시간만큼 러프값 계산
                float lerpValue =
                    between * (func == Function.Log ? Log(timer / second) : Power(timer / second));


				#region 목표 변수에 러프값 할당

				mat.SetFloat(targetParameter, from + lerpValue);

				#endregion

				// 목표 변수의 현재값 업데이트
				// 타겟 값과 러프값을 더한 값으로 업데이트한다.
				currentValue = from + lerpValue;

                // 코루틴 종료 확인
                if (!isRoutineRunning) break;
                yield return null;
			}

            // 코루틴 종료 확인
            if (!isRoutineRunning) break;
            yield return null;
		}

        yield break;
	}

	#region 러프용 수학 함수

	/// <summary>
	/// 로그6 함수
	/// </summary>
	/// <param name="_value"></param>
	/// <returns></returns>
	public float Log(float _value)
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
    public float Power(float _value)
	{
        float value = Mathf.Pow(_value * 32, 2) / Mathf.Pow(32, 2);

        return value;

        //Debug.Log(value);
    }

	#endregion
}
