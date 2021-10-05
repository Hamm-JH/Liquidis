using Looxid.Link;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

namespace Manager
{
	public partial class BIManager : MonoBehaviour
	{
		#region Instance

		private static BIManager instance;

		public static BIManager Instance
		{
			get
			{
				if(instance == null)
				{
					instance = FindObjectOfType<BIManager>() as BIManager;
				}
				return instance;
			}
		}

		#endregion

		#region Values

		private EEGSensor sensorStatusData;

		#region legacy
		// TODO : 추후 연결정리
		[Header("demo resource")]
		//public FloatLerp sizeLerpModule;
		//public FloatLerp speedLerpModule;

		[Header("Raw Signal")]
		//public LineChart AF3Chart;
		//public LineChart AF4Chart;
		//public LineChart Fp1Chart;
		//public LineChart Fp2Chart;
		//public LineChart AF7Chart;
		//public LineChart AF8Chart;
		#endregion

		[Header("Feature Index")]
		//public EEGSensorID SelectChannel;

		private bool isSensorConnected;
		private bool isSensorNoised;

		// 센서의 개수를 지정한 변수 (초기화 대상)
		private int sensorCount;

		private LinkDataValue leftActivity;
		private LinkDataValue rightActivity;
		private LinkDataValue attention;
		private LinkDataValue relaxation;

		private LinkDataValue[] delta;
		private LinkDataValue[] theta;
		private LinkDataValue[] alpha;
		private LinkDataValue[] beta;
		private LinkDataValue[] gamma;

		public LinkDataValue[] Delta { get => delta; set => delta = value; }
		public LinkDataValue[] Theta { get => theta; set => theta = value; }
		public LinkDataValue[] Alpha { get => alpha; set => alpha = value; }
		public LinkDataValue[] Beta { get => beta; set => beta = value; }
		public LinkDataValue[] Gamma { get => gamma; set => gamma = value; }

		public bool IsSensorConnected { get => isSensorConnected;}
		public bool IsSensorNoised { get => isSensorNoised;}

		#endregion

		#region Initialize & Activate, deactivate

		// Start is called before the first frame update
		void Start()
		{
			LooxidLinkManager.Instance.SetDebug(true);
			LooxidLinkManager.Instance.Initialize();

			sensorCount = Enum.GetValues(typeof(EEGSensorID)).Length;

			leftActivity = new LinkDataValue();
			rightActivity = new LinkDataValue();
			attention = new LinkDataValue();
			relaxation = new LinkDataValue();

			Delta = InitIndexArray();
			Theta = InitIndexArray();
			Alpha = InitIndexArray();
			Beta  = InitIndexArray();
			Gamma = InitIndexArray();
		}

		/// <summary>
		/// EEG값 결과치를 저장하는 배열 초기화
		/// </summary>
		/// <returns></returns>
		private LinkDataValue[] InitIndexArray()
		{
			LinkDataValue[] array = new LinkDataValue[Enum.GetValues(typeof(EEGSensorID)).Length];

			int index = array.Length;
			for (int i = 0; i < index; i++)
			{
				array[i] = new LinkDataValue();
			}

			return array;
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

			//StartCoroutine(SetEEG());
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

		#endregion

		// Update is called once per frame
		void Update()
		{
			leftActivity.value = Mathf.Lerp((float)leftActivity.value, (float)leftActivity.target, 0.2f);
			rightActivity.value = Mathf.Lerp((float)rightActivity.value, (float)rightActivity.target, 0.2f);
			attention.value = Mathf.Lerp((float)attention.value, (float)attention.target, 0.2f);
			relaxation.value = Mathf.Lerp((float)relaxation.value, (float)relaxation.target, 0.2f);
		}

		/// <summary>
		/// 타이머값과, 센서값을 조합해 한 개의 EEG 값을 반환한다.
		/// 데이터 생성이 완료되면 api의 callback 이벤트에 api를 담아서 실행시킨다.
		/// </summary>
		/// <param name="api"></param>
		public void Request(API.Brainwave api)
		{
			//ThreadPool.QueueUserWorkItem(Request_Inside, api);

			// 센서 인덱스
			API.Objective objective = api.Objective;
			EEGSensorID id = api.Id;
			int sIndex = (int)id;

			if(objective == API.Objective.EEG)
			{
				// EEGFeatureIndex 이벤트로 할당되는 EEG.value 값은 지속적으로 동작해야함

				// 타이머별 데이터 수집코드		// 코루틴 코드 대체 (min, max 할당)
				// 여기서 EEG.min, EEG.max값 할당
				SetSingleEEG(api.Id, api.Second);

				// 수집코드 할당				// Update 코드 대체 (value 할당)
				float _delta = Mathf.Lerp((float)Delta[sIndex].value, (float)Delta[sIndex].target, 1);
				float _theta = Mathf.Lerp((float)Theta[sIndex].value, (float)Theta[sIndex].target, 1);
				float _alpha = Mathf.Lerp((float)Alpha[sIndex].value, (float)Alpha[sIndex].target, 1);
				float _beta = Mathf.Lerp((float)Beta[sIndex].value, (float)Beta[sIndex].target, 1);
				float _gamma = Mathf.Lerp((float)Gamma[sIndex].value, (float)Gamma[sIndex].target, 1);

				// 수집된 데이터 api에 할당
				api.Set(_delta, _theta, _alpha, _beta, _gamma);

				// 수집된 데이터를 콜백 이벤트에 실어보냄
				api.CallBack.Invoke(api);
			}
			else if(objective == API.Objective.Relaxation)
			{
				api.Set(objective, (float)relaxation.value);
				api.CallBack.Invoke(api);
			}
			else if(objective == API.Objective.Attention)
			{
				api.Set(objective, (float)attention.value);
				api.CallBack.Invoke(api);
			}
		}

		//private void Request_Inside(object _api)
		//{
		//	if (IsSensorConnected != true && IsSensorNoised != false) return;

		//	API.Brainwave api = _api as API.Brainwave;

		//	int sIndex = (int)api.Id;

		//	// 타이머별 데이터 수집코드		// 코루틴 코드 대체 (min, max 할당)
		//	// 여기서 EEG.min, EEG.max값 할당
		//	SetSingleEEG(api.Id, api.Second);

		//	// 수집코드 할당				// Update 코드 대체 (value 할당)
		//	float _delta = Mathf.Lerp((float)Delta[sIndex].value, (float)Delta[sIndex].target, 1);
		//	float _theta = Mathf.Lerp((float)Theta[sIndex].value, (float)Theta[sIndex].target, 1);
		//	float _alpha = Mathf.Lerp((float)Alpha[sIndex].value, (float)Alpha[sIndex].target, 1);
		//	float _beta = Mathf.Lerp((float)Beta[sIndex].value, (float)Beta[sIndex].target, 1);
		//	float _gamma = Mathf.Lerp((float)Gamma[sIndex].value, (float)Gamma[sIndex].target, 1);

		//	// 수집된 데이터 api에 할당
		//	api.Set(_delta, _theta, _alpha, _beta, _gamma);

			

		//	// 수집된 데이터를 콜백 이벤트에 실어보냄
		//	api.CallBack.Invoke(api);
		//	////Debug.Log($"Hello");
			
		//	//string str = "";
		//	//str += $"sensorID : {api.Id.ToString()}\n";
		//	//str += $"-- delta : {api.Delta}\n";
		//	//str += $"-- theta : {api.Theta}\n";
		//	//str += $"-- alpha : {api.Alpha}\n";
		//	//str += $"-- beta : {api.Beta}\n";
		//	//str += $"-- gamma : {api.Gamma}\n";

		//	//Debug.Log(str);
			
		//	//str += $"second : {api.Second}\n";
		//	//str += $"callback event : {api.CallBack}";

		//	//Debug.Log(str);
		//}

		private void SetSingleEEG(EEGSensorID id, float second)
		{
			List<EEGFeatureIndex> featureIndexList = LooxidLinkData.Instance.GetEEGFeatureIndexData(second);

			if(featureIndexList.Count > 0)
			{
				List<double> deltaScaleDataList = new List<double>();
				List<double> thetaScaleDataList = new List<double>();
				List<double> alphaScaleDataList = new List<double>();
				List<double> betaScaleDataList = new List<double>();
				List<double> gammaScaleDataList = new List<double>();

				for (int i = 0; i < featureIndexList.Count; i++)
				{
					double deltaValue = featureIndexList[i].Delta(id);
					double thetaValue = featureIndexList[i].Theta(id);
					double alphaValue = featureIndexList[i].Alpha(id);
					double betaValue = featureIndexList [i].Beta (id);
					double gammaValue = featureIndexList[i].Gamma(id);

					if (!double.IsInfinity(deltaValue) && !double.IsNaN(deltaValue)) deltaScaleDataList.Add(deltaValue);
					if (!double.IsInfinity(thetaValue) && !double.IsNaN(thetaValue)) thetaScaleDataList.Add(thetaValue);
					if (!double.IsInfinity(alphaValue) && !double.IsNaN(alphaValue)) alphaScaleDataList.Add(alphaValue);
					if (!double.IsInfinity(betaValue)  && !double.IsNaN(betaValue))  betaScaleDataList.Add(betaValue);
					if (!double.IsInfinity(gammaValue) && !double.IsNaN(gammaValue)) gammaScaleDataList.Add(gammaValue);
				}

				Delta[(int)id].SetScale(deltaScaleDataList);
				Theta[(int)id].SetScale(thetaScaleDataList);
				Alpha[(int)id].SetScale(alphaScaleDataList);
				Beta[(int)id].SetScale(betaScaleDataList);
				Gamma[(int)id].SetScale(gammaScaleDataList);
			}
		}

		/// <summary>
		/// 지속적으로 EEGScale값 생성하는 코루틴.
		/// 이걸 멀티스레딩 돌려야함.
		/// 센서의 EEG Scale 값을 구한다. (min, max 계산)
		/// </summary>
		/// <returns></returns>
		IEnumerator SetEEG()
		{
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

			while (this.gameObject.activeSelf)
			{
				// 0.1초 간격으로 대기
				yield return new WaitForSeconds(0.1f);

				// 룩시드링크에서 현재에서 과거 10초 안의 데이터를 가져온다.
				List<EEGFeatureIndex> featureIndexList = LooxidLinkData.Instance.GetEEGFeatureIndexData(10.0f);

				if(featureIndexList.Count > 0)
				{
					for (int i = 0; i < sensorCount; i++)
					{
						for (int ii = 0; ii < featureIndexList.Count; ii++)
						{
							double deltaValue = featureIndexList[ii].Delta((EEGSensorID)i);
							double thetaValue = featureIndexList[ii].Theta((EEGSensorID)i);
							double alphaValue = featureIndexList[ii].Alpha((EEGSensorID)i);
							double betaValue  = featureIndexList[ii].Beta((EEGSensorID)i);
							double gammaValue = featureIndexList[ii].Gamma((EEGSensorID)i);

							if (!double.IsInfinity(deltaValue) && !double.IsNaN(deltaValue)) deltaScaleDataList[i].Add(deltaValue);
							if (!double.IsInfinity(thetaValue) && !double.IsNaN(thetaValue)) thetaScaleDataList[i].Add(thetaValue);
							if (!double.IsInfinity(alphaValue) && !double.IsNaN(alphaValue)) alphaScaleDataList[i].Add(alphaValue);
							if (!double.IsInfinity(betaValue) && !double.IsNaN(betaValue))   betaScaleDataList[i].Add(betaValue);
							if (!double.IsInfinity(gammaValue) && !double.IsNaN(gammaValue)) gammaScaleDataList[i].Add(gammaValue);
						}

						// 센서별로 스케일 설정 (min, max 값 설정)
						Delta[i].SetScale(deltaScaleDataList[i]);
						Theta[i].SetScale(thetaScaleDataList[i]);
						Alpha[i].SetScale(alphaScaleDataList[i]);
						Beta[i].SetScale(betaScaleDataList[i]);
						Gamma[i].SetScale(gammaScaleDataList[i]);
					}
				}
			}


			yield break;
		}
	}
}
