using Looxid.Link;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class TexManager : MonoBehaviour
{
    public TexOut fps;
    public List<TexOut> eegTxts;

    /// <summary>
    /// 요청 데이터를 반환받을때 실행되는 이벤트
    /// </summary>
    public UnityAction<API.Brainwave> biGetter;

	private void Start()
	{
        // 이벤트 연결
        biGetter += Receive;
	}

	private void Update()
	{
        // 요청 예시
        int index = Enum.GetValues(typeof(EEGSensorID)).Length;
		for (int i = 0; i < index; i++)
		{
            Request(new API.Brainwave(
                targetId: (EEGSensorID)i,
                targetSecond: 10,
                targetCallBack: biGetter)
                );
        }
	}

    /// <summary>
    /// 뇌파 관리자로 데이터 요청
    /// </summary>
    /// <param name="api"></param>
    /// <param name="action"></param>
    private void Request(API.Brainwave api)
	{
        Manager.BIManager.Instance.Request(api);
	}

    /// <summary>
    /// 이벤트에 연결된 데이터 수신 메서드
    /// </summary>
    /// <param name="api"></param>
    private void Receive(API.Brainwave api)
	{
        //stacks[(int)api.Id].Push(api);

		List<string> _in = new List<string>();
		_in.Add(api.Id.ToString());
		_in.Add(api.Delta.ToString());
		_in.Add(api.Theta.ToString());
		_in.Add(api.Alpha.ToString());
		_in.Add(api.Beta.ToString());
		_in.Add(api.Gamma.ToString());

		eegTxts[(int)api.Id].Set(_in.ToArray());

		//string str = "";
		//str += $"++ sensorID : {api.Id.ToString()}\n";
		//str += $"++ delta : {api.Delta}\n";
		//str += $"++ theta : {api.Theta}\n";
		//str += $"++ alpha : {api.Alpha}\n";
		//str += $"++ beta : {api.Beta}\n";
		//str += $"++ gamma : {api.Gamma}\n";

		//Debug.Log(str);
	}

	private void OnEnable()
	{
        StartCoroutine(SetFPS(fps));
    }

	private void OnDisable()
	{
        StopAllCoroutines();
	}

    IEnumerator SetFPS(TexOut tex)
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(0.2f);

            fps.Set(((int)(1 / Time.deltaTime)).ToString());
        }

        yield break;
    }

	#region Legacy
	IEnumerator SetEEG()
	{
        // 여기선 BIManager에서 가져온 데이터(value)만 가져와서 뿌리는 역할을 담당한다.

        #region 데이터 초기화
        LinkDataValue delta = new LinkDataValue();
		LinkDataValue theta = new LinkDataValue();
		LinkDataValue alpha = new LinkDataValue();
		LinkDataValue beta  = new LinkDataValue();
		LinkDataValue gamma = new LinkDataValue();

        // 데이터 선언, 초기화
        List<List<double>> deltaScaleDataList = new List<List<double>>();
        List<List<double>> thetaScaleDataList = new List<List<double>>();
        List<List<double>> alphaScaleDataList = new List<List<double>>();
        List<List<double>> betaScaleDataList = new List<List<double>>();
        List<List<double>> gammaScaleDataList = new List<List<double>>();

        // 뇌파 센서의 수 만큼 각각의 데이터리스트를 초기화한다.
        for (int i = 0; i < Enum.GetValues(typeof(EEGSensorID)).Length; i++)
        {
            deltaScaleDataList.Add(new List<double>());
            thetaScaleDataList.Add(new List<double>());
            alphaScaleDataList.Add(new List<double>());
            betaScaleDataList.Add(new List<double>());
            gammaScaleDataList.Add(new List<double>());
        }
        #endregion

        while (LooxidLinkManager.Instance.isActiveAndEnabled)
		{
            // 0.1초 간격 대기
            yield return new WaitForSeconds(0.1f);

            // 룩시드링크에서 현재에서 과거 10초 안의 데이터를 가져온다.
            List<EEGFeatureIndex> featureIndexList = LooxidLinkData.Instance.GetEEGFeatureIndexData(10.0f);

            // 가져온 데이터가 하나라도 있을 경우
            if(featureIndexList.Count > 0)
			{
                // n초 안의 뇌파 데이터를 수집해옴.
                // 이를 아래에서 연산해 10초 안의 최소, 최대값을 계산해냄.

                // FeatureIndex 값을 실시간으로 받아오는 쪽에서는 현재 뇌파값이 어떤지 산출해냄
                // delta.target

				// 센서별로 리스트 데이터 초기화
				// AF3 = 0,
				// AF4 = 1,
				// Fp1 = 2,
				// Fp2 = 3,
				// AF7 = 4,
				// AF8 = 5
				for (int i = 0; i < Enum.GetValues(typeof(EEGSensorID)).Length; i++)
				{
					for (int ii = 0; ii < featureIndexList.Count; ii++)
					{
                        // EEGFeatureIndex 리스트의 n번째 요소의 데이터를 반복문에서 하나씩 처리한다.
                        // 센서별로 데이터 수집
                        double deltaValue = featureIndexList[ii].Delta((EEGSensorID)i);
                        double thetaValue = featureIndexList[ii].Theta((EEGSensorID)i);
                        double alphaValue = featureIndexList[ii].Alpha((EEGSensorID)i);
                        double betaValue = featureIndexList[ii].Beta((EEGSensorID)i);
                        double gammaValue = featureIndexList[ii].Gamma((EEGSensorID)i);

                        if (!double.IsInfinity(deltaValue) && !double.IsNaN(deltaValue)) deltaScaleDataList[i].Add(deltaValue);
                        if (!double.IsInfinity(thetaValue) && !double.IsNaN(thetaValue)) thetaScaleDataList[i].Add(thetaValue);
                        if (!double.IsInfinity(alphaValue) && !double.IsNaN(alphaValue)) alphaScaleDataList[i].Add(alphaValue);
                        if (!double.IsInfinity(betaValue) && !double.IsNaN(betaValue)) betaScaleDataList[i].Add(betaValue);
                        if (!double.IsInfinity(gammaValue) && !double.IsNaN(gammaValue)) gammaScaleDataList[i].Add(gammaValue);

                        //Debug.Log($"deltaValue : {deltaValue}");
                    }

					//for (int ii = 0; ii < deltaScaleDataList[i].Count; ii++)
					//{
                    //    Debug.Log($"deltaScale {ii} : {deltaScaleDataList[i][ii]}");
					//}

                    // 센서별로 스케일 설정 (정상)
                    delta.SetScale(deltaScaleDataList[i]);
                    theta.SetScale(thetaScaleDataList[i]);
                    alpha.SetScale(alphaScaleDataList[i]);
                    beta.SetScale(betaScaleDataList[i]);
                    gamma.SetScale(gammaScaleDataList[i]);

                    //delta.target = LooxidLinkUtility.Scale(delta.min, delta.max, 0.0f, 1.0f, deltaValue);
                    //theta.target = (double.IsInfinity(thetaValue) || double.IsNaN(thetaValue)) ? 0.0f : LooxidLinkUtility.Scale(theta.min, theta.max, 0.0f, 1.0f, thetaValue);
                    //alpha.target = (double.IsInfinity(alphaValue) || double.IsNaN(alphaValue)) ? 0.0f : LooxidLinkUtility.Scale(alpha.min, alpha.max, 0.0f, 1.0f, alphaValue);
                    //beta.target = (double.IsInfinity(betaValue) || double.IsNaN(betaValue)) ? 0.0f : LooxidLinkUtility.Scale(beta.min, beta.max, 0.0f, 1.0f, betaValue);
                    //gamma.target = (double.IsInfinity(gammaValue) || double.IsNaN(gammaValue)) ? 0.0f : LooxidLinkUtility.Scale(gamma.min, gamma.max, 0.0f, 1.0f, gammaValue);

                    Debug.Log($"min : {delta.min}");
                    Debug.Log($"max : {delta.max}");
                    Debug.Log($"target : {delta.target}");
                    Debug.Log($"value : {delta.value}");

                    if (featureIndexList.Count != 0)
					{
                        // 센서 단위로 데이터 세팅 완료.
                        string[] _ins = new string[7];
                        _ins[0] = ((EEGSensorID)i).ToString();
                        _ins[1] = ((float)delta.value).ToString();
                        _ins[2] = ((float)theta.value).ToString();
                        _ins[3] = ((float)alpha.value).ToString();
                        _ins[4] = ((float)beta.value).ToString();
                        _ins[5] = ((float)gamma.value).ToString();
                        _ins[6] = featureIndexList.Count.ToString();
                        //Debug.Log($"Count : {i}");
                        //Debug.Log($"ins : {_ins}");
                        //Debug.Log($"data 1 : {delta}");
						//Debug.Log($"data 2 : {theta}");
						//Debug.Log($"data 3 : {alpha}");
                        //Debug.Log($"data 4 : {beta}");
                        //Debug.Log($"data 5 : {gamma}");
                        eegTxts[i].Set(_ins);
					}
                }




                // 뇌파 센서의 수 만큼 각각의 데이터리스트를 초기화한다.
                //for (int i = 0; i < Enum.GetValues(typeof(EEGSensorID)).Length; i++)
                //{
                //    deltaScaleDataList[i].Clear();
                //    thetaScaleDataList[i].Clear();
                //    alphaScaleDataList[i].Clear();
                //    betaScaleDataList [i].Clear();
                //    gammaScaleDataList[i].Clear();
                //}
            }
        }

        yield break;
	}

    IEnumerator SetEEG(TexOut tex, EEGSensorID id)
	{
        string sensorID = "";
        float delta = 0;
        float theta = 0;
        float alpha = 0;
        float beta = 0;
        float gamma = 0;

        string[] _ins = new string[6];

        while(gameObject.activeSelf)
		{
            yield return new WaitForSeconds(0.2f);

            Debug.Log("Hello");

            _ins[0] = id.ToString();
            _ins[1] = delta.ToString();
            _ins[2] = theta.ToString();
            _ins[3] = alpha.ToString();
            _ins[4] = beta.ToString();
            _ins[5] = gamma.ToString();

            tex.Set(_ins);
		}

        yield break;
	}
	#endregion
}
