using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatLerp : MonoBehaviour
{
    [Header("target resource")]
    public GameObject obj;

    public SkinnedMeshRenderer render;
    public Material mat;
    public string targetParameter;

    public bool isUseMat;

    //---------

    [Header("operating value")]
    [SerializeField] private float currentValue;
    public float targetValue;

    public float boundary;      // 러프 재시작 경계값
    public Function function;   // 러프 결정함수

    public float interval;      // 러프 타이밍 간격

    bool isRoutineRunning;      // 루틴 시동관리 변순

    public bool isReached;      // 러프 완료 확인코드


    public enum Function
	{
        Log,
        Power
	}

    // Start is called before the first frame update
    void Start()
    {
        //mat = render.material;

        //StartCoroutine(Lerp(5, 3, 2, Function.Log));s
    }

	private void OnEnable()
	{
        isRoutineRunning = true;

        StartCoroutine(Lerp());
    }

	private void OnDisable()
	{
        isRoutineRunning = false;
	}

	public IEnumerator Lerp(/*float from, float to, float second, Function func*/)
    {
        // 지정된 초(second) 동안 from에서 to로 러프 연산

        float from = 0;         // 시작값
        float to = 0;           // 목표값
        float between = 0;      // from에서 to 사이값

        float timer = 0f;       // 내부 타이머값

        //float timeValue = 0f;   // 러프 결과값
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

                    // 내부 초기화
                    // TODO : 타겟 값을 할당한다
                    from = mat.GetFloat(targetParameter);
     //               if(isUseMat)
					//{
					//}
     //               else
					//{
     //                   from = mat.GetColor("_BaseColor").b;
					//}
                    //from = obj.transform.position.y; // obj 보간할 경우에 씀

                    to = targetValue;

                    between = to - from;    // 사이값 초기화
                    second = interval;      // 시간간격 업데이트

                    timer = 0;      // 타이머 초기화
                    //timeValue = 0;  // 시간 결과값 초기화

                    func = function;    // 러프 결정함수 업데이트
				}
                else
				{
                    yield return null;
                    continue;
				}
			}
            else
			{
                // 타이머 업데이트
                timer = (timer + Time.deltaTime >= second) ? second : timer + Time.deltaTime;

                // 타임 오버시 브레이크
                if (timer >= second)    isReached = true;

                // 현재 진행된 시간만큼 러프값 계산
                float lerpValue =
                    between * (func == Function.Log ? Log(timer / second) : Power(timer / second));


                mat.SetFloat(targetParameter, from + lerpValue);

    //            // 움직일 목표에 값 할당
    //            if(isUseMat)
				//{
				//}
    //            else
				//{
    //                float blue = from + lerpValue;
    //                float red = 1 - (from + lerpValue);

    //                if(blue < 0)
				//	{
    //                    blue = 0;
    //                    red = 1;
				//	}
    //                else if(blue > 1)
				//	{
    //                    blue = 1;
    //                    red = 0;
				//	}

    //                mat.SetColor("_BaseColor", new Color(red, 0, blue));
				//}

                // obj 보간시 사용
                //obj.transform.position = new Vector3(
                //    0,
                //    from + lerpValue,
                //    0
                //);

                // 현재 값 업데이트
                // 타겟 값과 러프값을 더한 값으로 업데이트한다.
                currentValue = from + lerpValue;


                if (!isRoutineRunning) break;

                yield return null;
			}

            // isReached == false인 경우, 다시 러프값이 초기화된 경우


            // boundary보다 차이값이 많이 나는 경우
            //timer = (timer + Time.deltaTime >= second) ? second : timer + Time.deltaTime;

            // 타임오버시 브레이크
            //if (timer >= second)    isReached = true;

            // 현재 진행된 시간만큼 러프할 기반값을 계산
            //float lerpValue =
            //    between * (func == Function.Log ? Log(timer / second) : Power(timer / second));

            //Debug.Log(lerpValue);

            //Debug.Log(timer / second);
            //Debug.Log(Log(timer / second));

            //obj.transform.position = new Vector3(
            //    0,
            //    Log(timer/second),
            //    0
            //    );

            if (!isRoutineRunning) break;

            yield return null;
		}

        yield break;
	}

    public float Log(float _value)
    {
        float value = Mathf.Log((_value*63 + 1), 2) / 6;

        return value;

        Debug.Log(Mathf.Log((_value*63 + 1), 2) / 6);
    }

    public float Power(float _value)
	{
        float value = Mathf.Pow(_value * 32, 2) / Mathf.Pow(32, 2);

        return value;

        Debug.Log(value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
