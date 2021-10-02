using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatLerp : MonoBehaviour
{
    // ������ ������ ��ǥ ����, ��ü

    [Header("target resource")]
    public GameObject obj;

    public SkinnedMeshRenderer render;
    public Material mat;
    public string targetParameter;

    public bool isUseMat;

    //---------
    // ���� ����

    [Header("operating value")]
    [SerializeField] private float currentValue;
    public float targetValue;

    public float boundary;      // ���� ����� ��谪
    public Function function;   // ���� �����Լ�

    public float interval;      // ���� Ÿ�̹� ����

    bool isRoutineRunning;      // ��ƾ �õ����� ����

    public bool isReached;      // ���� �Ϸ� Ȯ���ڵ�


    public enum Function
	{
        Linear,
        Log,
        Power
	}

    /// <summary>
    /// �� ��ũ��Ʈ Ȱ��ȭ��, ���� �ڷ�ƾ�� Ȱ��ȭ�Ѵ�.
    /// </summary>
	private void OnEnable()
	{
        isRoutineRunning = true;

        StartCoroutine(Lerp());
    }

    /// <summary>
    /// �� ��ũ��Ʈ ��Ȱ��ȭ��, ���� �ڷ�ƾ�� ��Ȱ��ȭ�Ѵ�.
    /// </summary>
	private void OnDisable()
	{
        isRoutineRunning = false;
	}

	public IEnumerator Lerp()
    {
        // ������ ��(second) ���� from���� to�� ���� ����

        float from = 0;         // ���۰�
        float to = 0;           // ��ǥ��
        float between = 0;      // from���� to ���̰�

        float timer = 0f;       // ���� Ÿ�̸Ӱ�

        float second = interval;    // �ð����ݰ�

        isReached = false;     // ������ ���� Ȯ��

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

                    // ���� ���� �ʱ�ȭ
                    // ���� ������ �Ҵ�
                    from = mat.GetFloat(targetParameter);

                    to = targetValue;

                    between = to - from;    // ���̰� �ʱ�ȭ
                    second = interval;      // �ð����� ������Ʈ

                    timer = 0;      // Ÿ�̸� �ʱ�ȭ

                    func = function;    // ���� �����Լ� ������Ʈ
				}
                else
				{
                    // �ڷ�ƾ ���� Ȯ��
                    if (!isRoutineRunning) break;
                    yield return null;
                    continue;
				}
			}
            // ���� ����
            else
			{
                // Ÿ�̸� ������Ʈ
                timer = (timer + Time.deltaTime >= second) ? second : timer + Time.deltaTime;

                // Ÿ�� ������ �극��ũ
                if (timer >= second)    isReached = true;

                // ���� ����� �ð���ŭ ������ ���
                float lerpValue =
                    between * (func == Function.Log ? Log(timer / second) : Power(timer / second));


				#region ��ǥ ������ ������ �Ҵ�

				mat.SetFloat(targetParameter, from + lerpValue);

				#endregion

				// ��ǥ ������ ���簪 ������Ʈ
				// Ÿ�� ���� �������� ���� ������ ������Ʈ�Ѵ�.
				currentValue = from + lerpValue;

                // �ڷ�ƾ ���� Ȯ��
                if (!isRoutineRunning) break;
                yield return null;
			}

            // �ڷ�ƾ ���� Ȯ��
            if (!isRoutineRunning) break;
            yield return null;
		}

        yield break;
	}

	#region ������ ���� �Լ�

	/// <summary>
	/// �α�6 �Լ�
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
    /// n���� �Լ�
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
