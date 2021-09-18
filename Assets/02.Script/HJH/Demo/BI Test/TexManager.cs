using Looxid.Link;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TexManager : MonoBehaviour
{
    public TexOut fps;

    //public TexOut eegAF3;
    //public TexOut eegAF4;
    //public TexOut eegFp1;
    //public TexOut eegFp2;
    //public TexOut eegAF7;
    //public TexOut eegAF8;

    public List<TexOut> eegTxts;

	private void OnEnable()
	{
        StartCoroutine(SetFPS(fps));

        StartCoroutine(SetEEG());

        //StartCoroutine(SetEEG(eegAF3, EEGSensorID.AF3));
        //StartCoroutine(SetEEG(eegAF4, EEGSensorID.AF4));
        //StartCoroutine(SetEEG(eegFp1, EEGSensorID.Fp1));
        //StartCoroutine(SetEEG(eegFp2, EEGSensorID.Fp2));
        //StartCoroutine(SetEEG(eegAF7, EEGSensorID.AF7));
        //StartCoroutine(SetEEG(eegAF8, EEGSensorID.AF8));
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

    IEnumerator SetEEG()
	{
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
                    }

                    // 센서별로 스케일 설정
                    delta.SetScale(deltaScaleDataList[i]);
                    theta.SetScale(thetaScaleDataList[i]);
                    alpha.SetScale(alphaScaleDataList[i]);
                    beta.SetScale(betaScaleDataList[i]);
                    gamma.SetScale(gammaScaleDataList[i]);


				}




                // 뇌파 센서의 수 만큼 각각의 데이터리스트를 초기화한다.
                for (int i = 0; i < Enum.GetValues(typeof(EEGSensorID)).Length; i++)
                {
                    deltaScaleDataList[i].Clear();
                    thetaScaleDataList[i].Clear();
                    alphaScaleDataList[i].Clear();
                    betaScaleDataList [i].Clear();
                    gammaScaleDataList[i].Clear();
                }
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
}
