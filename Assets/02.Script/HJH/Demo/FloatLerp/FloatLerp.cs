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
    //---------

    [Header("operating value")]
    public float currentValue;
    [SerializeField] private float targetValue;

    public float boundary;      // ���� ����� ��谪
    public Function function;   // ���� �����Լ�

    public float interval;      // ���� Ÿ�̹� ����

    bool isRoutineRunning;      // ��ƾ �õ����� ����


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
        // ������ ��(second) ���� from���� to�� ���� ����

        float from = 0;         // ���۰�
        float to = 0;           // ��ǥ��
        float between = 0;      // from���� to ���̰�

        float timer = 0f;       // ���� Ÿ�̸Ӱ�

        //float timeValue = 0f;   // ���� �����
        float second = interval;    // �ð����ݰ�

        bool isReached = false;     // ������ ���� Ȯ��

        Function func = function;
        
        if(mat == null)
		{
            mat = render.material;
		}

        while(true)
		{
            // Ư�� ���� ������ ����
            if(isReached == true)
			{
                float diff = Mathf.Abs(currentValue - targetValue);
                // ���̰��� �ּ� ��谪���� ū ��� �� �ʱ�ȭ
                if(diff > boundary)
				{
                    isReached = false;

                    // ���� �ʱ�ȭ
                    // TODO : Ÿ�� ���� �Ҵ��Ѵ�
                    from = mat.GetFloat(targetParameter);
                    //from = obj.transform.position.y; // obj ������ ��쿡 ��

                    to = targetValue;

                    between = to - from;    // ���̰� �ʱ�ȭ
                    second = interval;      // �ð����� ������Ʈ

                    timer = 0;      // Ÿ�̸� �ʱ�ȭ
                    //timeValue = 0;  // �ð� ����� �ʱ�ȭ

                    func = function;    // ���� �����Լ� ������Ʈ
				}
                else
				{
                    yield return null;
                    continue;
				}
			}
            else
			{
                // Ÿ�̸� ������Ʈ
                timer = (timer + Time.deltaTime >= second) ? second : timer + Time.deltaTime;

                // Ÿ�� ������ �극��ũ
                if (timer >= second)    isReached = true;

                // ���� ����� �ð���ŭ ������ ���
                float lerpValue =
                    between * (func == Function.Log ? Log(timer / second) : Power(timer / second));



                // ������ ��ǥ�� �� �Ҵ�
                mat.SetFloat(targetParameter, from + lerpValue);

                // obj ������ ���
                //obj.transform.position = new Vector3(
                //    0,
                //    from + lerpValue,
                //    0
                //);

                // ���� �� ������Ʈ
                // Ÿ�� ���� �������� ���� ������ ������Ʈ�Ѵ�.
                currentValue = from + lerpValue;


                if (!isRoutineRunning) break;

                yield return null;
			}

            // isReached == false�� ���, �ٽ� �������� �ʱ�ȭ�� ���


            // boundary���� ���̰��� ���� ���� ���
            //timer = (timer + Time.deltaTime >= second) ? second : timer + Time.deltaTime;

            // Ÿ�ӿ����� �극��ũ
            //if (timer >= second)    isReached = true;

            // ���� ����� �ð���ŭ ������ ��ݰ��� ���
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
