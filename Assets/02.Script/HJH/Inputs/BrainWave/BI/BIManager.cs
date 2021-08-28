using Looxid.Link;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
	public partial class BIManager : MonoBehaviour
	{
		private EEGSensor sensorStatusData;

		[Header("Raw Signal")]
		public LineChart AF3Chart;
		public LineChart AF4Chart;
		public LineChart Fp1Chart;
		public LineChart Fp2Chart;
		public LineChart AF7Chart;
		public LineChart AF8Chart;

		[Header("Feature Index")]
		public EEGSensorID SelectChannel;

		private LinkDataValue leftActivity;
		private LinkDataValue rightActivity;
		private LinkDataValue attention;
		private LinkDataValue relaxation;

		private LinkDataValue delta;
		private LinkDataValue theta;
		private LinkDataValue alpha;
		private LinkDataValue beta;
		private LinkDataValue gamma;

		public LinkDataValue Delta { get => delta; set => delta=value; }
		public LinkDataValue Theta { get => theta; set => theta=value; }
		public LinkDataValue Alpha { get => alpha; set => alpha=value; }
		public LinkDataValue Beta { get => beta; set => beta=value; }
		public LinkDataValue Gamma { get => gamma; set => gamma=value; }

		public double deltaV;
		public double thetaV;
		public double alphaV;
		public double betaV;
		public double gammaV;


		// Start is called before the first frame update
		void Start()
		{
			LooxidLinkManager.Instance.SetDebug(true);
			LooxidLinkManager.Instance.Initialize();

			leftActivity = new LinkDataValue();
			rightActivity = new LinkDataValue();
			attention = new LinkDataValue();
			relaxation = new LinkDataValue();

			Delta = new LinkDataValue();
			Theta = new LinkDataValue();
			Alpha = new LinkDataValue();
			Beta = new LinkDataValue();
			Gamma = new LinkDataValue();

			deltaV = 0;
			thetaV = 0;
			alphaV = 0;
			betaV = 0;
			gammaV = 0;
		}

		private void OnEnable()
		{
			LooxidLinkManager.OnLinkCoreConnected += OnLinkCoreConncetd;
			LooxidLinkManager.OnLinkHubConnected += OnLinkHubConnected;
			LooxidLinkManager.OnLinkCoreDisconnected += OnLinkCoreDisconncetd;
			LooxidLinkManager.OnLinkHubDisconnected += OnLinkHubDisconnected;
			LooxidLinkManager.OnShowSensorOffMessage += OnShowSensorOffMessage;
			LooxidLinkManager.OnHideSensorOffMessage += OnHideSensorOffMessage;
			LooxidLinkManager.OnShowNoiseSignalMessage += OnShowNoiseSignalMessage;
			LooxidLinkManager.OnHideNoiseSignalMessage += OnHideNoiseSignalMessage;

			LooxidLinkData.OnReceiveEEGSensorStatus += OnReceiveEEGSensorStatus;
			LooxidLinkData.OnReceiveEEGRawSignals += OnReceiveEEGRawSignals;
			LooxidLinkData.OnReceiveMindIndexes += OnReceiveMindIndexes;
			LooxidLinkData.OnReceiveEEGFeatureIndexes += OnReceiveEEGFeatureIndexes;

			StartCoroutine(DisplayData());
		}

		private void OnDisable()
		{
			LooxidLinkManager.OnLinkCoreConnected -= OnLinkCoreConncetd;
			LooxidLinkManager.OnLinkHubConnected -= OnLinkHubConnected;
			LooxidLinkManager.OnLinkCoreDisconnected -= OnLinkCoreDisconncetd;
			LooxidLinkManager.OnLinkHubDisconnected -= OnLinkHubDisconnected;
			LooxidLinkManager.OnShowSensorOffMessage -= OnShowSensorOffMessage;
			LooxidLinkManager.OnHideSensorOffMessage -= OnHideSensorOffMessage;
			LooxidLinkManager.OnShowNoiseSignalMessage -= OnShowNoiseSignalMessage;
			LooxidLinkManager.OnHideNoiseSignalMessage -= OnHideNoiseSignalMessage;

			LooxidLinkData.OnReceiveEEGSensorStatus -= OnReceiveEEGSensorStatus;
			LooxidLinkData.OnReceiveEEGRawSignals -= OnReceiveEEGRawSignals;
			LooxidLinkData.OnReceiveMindIndexes -= OnReceiveMindIndexes;
			LooxidLinkData.OnReceiveEEGFeatureIndexes -= OnReceiveEEGFeatureIndexes;
		}

		// Update is called once per frame
		void Update()
		{
			leftActivity.value = Mathf.Lerp((float)leftActivity.value, (float)leftActivity.target, 0.2f);
			rightActivity.value = Mathf.Lerp((float)rightActivity.value, (float)rightActivity.target, 0.2f);
			attention.value = Mathf.Lerp((float)attention.value, (float)attention.target, 0.2f);
			relaxation.value = Mathf.Lerp((float)relaxation.value, (float)relaxation.target, 0.2f);

			delta.value = Mathf.Lerp((float)delta.value, (float)delta.target, 0.2f);
			theta.value = Mathf.Lerp((float)theta.value, (float)theta.target, 0.2f);
			alpha.value = Mathf.Lerp((float)alpha.value, (float)alpha.target, 0.2f);
			beta.value = Mathf.Lerp((float)beta.value, (float)beta.target, 0.2f);
			gamma.value = Mathf.Lerp((float)gamma.value, (float)gamma.target, 0.2f);

			deltaV = delta.value;
			thetaV = theta.value;
			alphaV = alpha.value;
			betaV = beta.value;
			gammaV = gamma.value;
		}

		IEnumerator DisplayData()
		{
			// 데이터 선언, 초기화
			List<List<double>> deltaScaleDataList = new List<List<double>>();
			List<List<double>> thetaScaleDataList = new List<List<double>>();
			List<List<double>> alphaScaleDataList = new List<List<double>>();
			List<List<double>> betaScaleDataList = new List<List<double>>();
			List<List<double>> gammaScaleDataList = new List<List<double>>();

			for (int i = 0; i < Enum.GetValues(typeof(EEGSensorID)).Length; i++)
			{
				deltaScaleDataList.Add(new List<double>());
				thetaScaleDataList.Add(new List<double>());
				alphaScaleDataList.Add(new List<double>());
				betaScaleDataList.Add(new List<double>());
				gammaScaleDataList.Add(new List<double>());
			}

			while (this.gameObject.activeSelf)
			{
				yield return new WaitForSeconds(0.1f);

				List<EEGFeatureIndex> featureIndexList = LooxidLinkData.Instance.GetEEGFeatureIndexData(10.0f);

				if (featureIndexList.Count > 0)
				{
					// 센서별로 리스트 데이터 초기화
					for (int i = 0; i < Enum.GetValues(typeof(EEGSensorID)).Length; i++)
					{
						for (int ii = 0; ii < featureIndexList.Count; ii++)
						{
							// 센서별로 데이터 수집
							double deltaValue = featureIndexList[i].Delta((EEGSensorID)i);
							double thetaValue = featureIndexList[i].Theta((EEGSensorID)i);
							double alphaValue = featureIndexList[i].Alpha((EEGSensorID)i);
							double betaValue = featureIndexList[i].Beta((EEGSensorID)i);
							double gammaValue = featureIndexList[i].Gamma((EEGSensorID)i);

							if (!double.IsInfinity(deltaValue) && !double.IsNaN(deltaValue)) deltaScaleDataList[i].Add(deltaValue);
							if (!double.IsInfinity(thetaValue) && !double.IsNaN(thetaValue)) thetaScaleDataList[i].Add(thetaValue);
							if (!double.IsInfinity(alphaValue) && !double.IsNaN(alphaValue)) alphaScaleDataList[i].Add(alphaValue);
							if (!double.IsInfinity(betaValue) && !double.IsNaN(betaValue)) betaScaleDataList[i].Add(betaValue);
							if (!double.IsInfinity(gammaValue) && !double.IsNaN(gammaValue)) gammaScaleDataList[i].Add(gammaValue);
						}


						delta.SetScale(deltaScaleDataList[i]);
						theta.SetScale(thetaScaleDataList[i]);
						alpha.SetScale(alphaScaleDataList[i]);
						beta.SetScale(betaScaleDataList[i]);
						gamma.SetScale(gammaScaleDataList[i]);
					}

				}
			}
		}
	}
}
