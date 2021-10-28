using Looxid.Link;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Manager
{
	public enum CollectionStatus
	{
		Stanby = 0,
		Reference = 1,
		Contents = 2,
	}

	[Serializable]
	public struct mindData
	{
		[SerializeField] int dataCount;
		[SerializeField] double dataSum;
		[SerializeField] Queue<double> datas;
		[SerializeField] double result;

		public int DataCount { get => dataCount; set => dataCount=value; }
		public double DataSum { get => dataSum; set => dataSum=value; }
		public Queue<double> Datas { get => datas; set => datas=value; }
		public double Result { get => result; set => result=value; }

		public void Add(double data, int maxCount)
		{
			datas.Enqueue(data);
			dataSum = dataSum + data;
			dataCount = datas.Count;

			int count = datas.Count;
			if(count > maxCount)
			{
				double old = datas.Dequeue();
				dataSum = dataSum - old;
				dataCount = datas.Count;
			}
		}

		public void SetResult()
		{
			result = dataSum / dataCount;
		}
	}

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

		public int eegMaximumCount = 100;

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

		[Header("Collection Switching index")]
		[SerializeField] private CollectionStatus collectionStatus;

		public CollectionStatus _CollectionStatus
		{
			get => collectionStatus;
			set
			{
				SetMindResult(collectionStatus);
				collectionStatus = value;
			}
		}

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

		[Header("집중 값")]
		[SerializeField] private double concentration;   // 집중
		[SerializeField] private double excitement;		// 흥분
		[SerializeField] private double positiveness;	// 긍부정
		[SerializeField] private double empathy;			// 공감

		private mindData[] result_concentration;   // 집중
		private mindData[] result_excitement;      // 흥분
		private mindData[] result_positiveness;    // 긍부정
		private mindData[] result_empathy;		   // 공감

		private LinkDataValue[] delta;
		private LinkDataValue[] theta;
		private LinkDataValue[] alpha;
		private LinkDataValue[] beta;
		private LinkDataValue[] gamma;

		[Header("Debug get eeg")]
		[SerializeField] private double _delta;
		[SerializeField] private double _theta;
		[SerializeField] private double _alpha;
		[SerializeField] private double _beta;
		[SerializeField] private double _gamma;

		public double Concentration { get => concentration; }
		public double Excitement { get => excitement; }
		public double Positiveness { get => positiveness; }
		public double Empathy { get => empathy; }

		// TODO 1015 : 뇌파 수집코드 추후 계산코드 배치
		// concentration[(int)CollectionStatus.Contents].Result; <- 임시 할당임
		public mindData[] Result_concentration { get => result_concentration; }
		public mindData[] Result_excitement { get => result_excitement; }
		public mindData[] Result_positiveness { get => result_positiveness; }
		public mindData[] Result_empathy { get => result_empathy; }

		public LinkDataValue[] Delta { get => delta; set => delta = value; }
		public LinkDataValue[] Theta { get => theta; set => theta = value; }
		public LinkDataValue[] Alpha { get => alpha; set => alpha = value; }
		public LinkDataValue[] Beta { get => beta; set => beta = value; }
		public LinkDataValue[] Gamma { get => gamma; set => gamma = value; }

		public bool IsSensorConnected { get => isSensorConnected;}
		public bool IsSensorNoised { get => isSensorNoised;}




		#endregion

		#region Initialize & Activate, deactivate

		private void Awake()
		{
			leftActivity = new LinkDataValue();
			rightActivity = new LinkDataValue();
			attention = new LinkDataValue();
			relaxation = new LinkDataValue();
		}

		// Start is called before the first frame update
		void Start()
		{
			LooxidLinkManager.Instance.SetDebug(true);
			LooxidLinkManager.Instance.Initialize();

			sensorCount = Enum.GetValues(typeof(EEGSensorID)).Length;
			
			result_concentration	= new mindData[Enum.GetValues(typeof(CollectionStatus)).Length];
			result_excitement		= new mindData[Enum.GetValues(typeof(CollectionStatus)).Length];
			result_positiveness		= new mindData[Enum.GetValues(typeof(CollectionStatus)).Length];
			result_empathy			= new mindData[Enum.GetValues(typeof(CollectionStatus)).Length];

			int index = Enum.GetValues(typeof(CollectionStatus)).Length;
			for (int i = 0; i < index; i++)
			{
				result_concentration[i] = new mindData();
				result_concentration[i].Datas = new Queue<double>();

				result_excitement[i] = new mindData();
				result_excitement[i].Datas = new Queue<double>();

				result_positiveness[i] = new mindData();
				result_positiveness[i].Datas = new Queue<double>();

				result_empathy[i] = new mindData();
				result_empathy[i].Datas = new Queue<double>();
			}

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

			StartCoroutine(SetEEG());
			StartCoroutine(SetMindIndex());
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

		#region Update

		// Update is called once per frame
		void Update()
		{
			//Debug.Log("1");
			leftActivity.value = Mathf.Lerp((float)leftActivity.value, (float)leftActivity.target, 0.2f);
			rightActivity.value = Mathf.Lerp((float)rightActivity.value, (float)rightActivity.target, 0.2f);
			attention.value = Mathf.Lerp((float)attention.value, (float)attention.target, 0.2f);
			relaxation.value = Mathf.Lerp((float)relaxation.value, (float)relaxation.target, 0.2f);
            //Debug.Log("H");
            UpdateMindIndex();
		}

		private void UpdateMindIndex()
		{
			UpdateConcentration();
			UpdateExcitement();
			UpdatePositiveness();
			UpdateEmpathy();
		}

		/// <summary>
		/// 집중
		/// </summary>
		private void UpdateConcentration()
		{
			//Debug.Log(attention.value);
			concentration = attention.value;
		}

		/// <summary>
		/// 흥분
		/// </summary>
		private void UpdateExcitement()
		{
			double value = (
				Gamma[(int)EEGSensorID.AF3].target +
				Gamma[(int)EEGSensorID.AF4].target +
				Gamma[(int)EEGSensorID.Fp1].target +
				Gamma[(int)EEGSensorID.Fp2].target +
				Gamma[(int)EEGSensorID.AF7].target +
				Gamma[(int)EEGSensorID.AF8].target) / 6;
			//Debug.Log("H");
			excitement = value;
		}

		/// <summary>
		/// 긍부정
		/// </summary>
		private void UpdatePositiveness()
		{
			double value = (
				Theta[(int)EEGSensorID.AF3].target +
				Theta[(int)EEGSensorID.AF4].target +
				Theta[(int)EEGSensorID.Fp1].target +
				Theta[(int)EEGSensorID.Fp2].target +
				Theta[(int)EEGSensorID.AF7].target +
				Theta[(int)EEGSensorID.AF8].target) / 6;

			positiveness = value;
		}

		/// <summary>
		/// 공감
		/// </summary>
		private void UpdateEmpathy()
		{
			double value = (
				Alpha[(int)EEGSensorID.AF3].target +
				Alpha[(int)EEGSensorID.AF4].target +
				Alpha[(int)EEGSensorID.Fp1].target +
				Alpha[(int)EEGSensorID.Fp2].target +
				Alpha[(int)EEGSensorID.AF7].target +
				Alpha[(int)EEGSensorID.AF8].target) / 6;

			empathy = value;
		}
		#endregion

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
				api.Set(objective, -1, (float)relaxation.value);
				api.CallBack.Invoke(api);
			}
			else if(objective == API.Objective.Attention)
			{
				api.Set(objective, -1, (float)attention.value);
				api.CallBack.Invoke(api);
			}
			else if(objective == API.Objective.Concentration)
			{
				api.Set(objective, -1, (float)Concentration);
				api.CallBack.Invoke(api);
			}
			else if (objective == API.Objective.Excitement)
			{
				api.Set(objective, -1, (float)Excitement);
				api.CallBack.Invoke(api);
			}
			else if (objective == API.Objective.Positiveness)
			{
				api.Set(objective, -1, (float)Positiveness);
				api.CallBack.Invoke(api);
			}
			else if (objective == API.Objective.Empathy)
			{
				api.Set(objective, -1, (float)Empathy);
				api.CallBack.Invoke(api);
			}
			else if(objective == API.Objective.EEGRandom)
			{
				float _delta = Random.value;
				float _theta = Random.value;
				float _alpha = Random.value;
				float _beta  = Random.value;
				float _gamma = Random.value;

				api.Set(_delta, _theta, _alpha, _beta, _gamma);

				api.CallBack.Invoke(api);
			}
			else if(objective == API.Objective.MindRandom)
			{
				float val = Random.value;
				
				//if(api.Option == 0)
				//{
				//	api.Set(objective, api.Option, val);
				//	api.CallBack.Invoke(api);
				//}
				//else if(api.Option == 1)
				//{
				//	api.Set(objective, api.Option, val);
				//	api.CallBack.Invoke(api);
				//}
				//else if (api.Option == 2)
				//{
				//	api.Set(objective, api.Option, val);
				//	api.CallBack.Invoke(api);
				//}
				//else if (api.Option == 3)
				//{
				//	api.Set(objective, api.Option, val);
				//	api.CallBack.Invoke(api);
				//}

				api.Set(objective, api.Option, val);
				api.CallBack.Invoke(api);
			}
			else if(objective == API.Objective.Result_Concentration)
			{
				double val1 = Result_concentration[(int)CollectionStatus.Reference].Result;
				double val2 = Result_concentration[(int)CollectionStatus.Contents].Result;
				double result = attention.target;
				api.Set(objective, api.Option, (float)result);
				api.CallBack.Invoke(api);
			}
			else if (objective == API.Objective.Result_Excitement)
			{
				double val1 = Result_excitement[(int)CollectionStatus.Reference].Result;
				double val2 = Result_excitement[(int)CollectionStatus.Contents].Result;
				double result = (val2 - val1 + 1) / 2;
				api.Set(objective, api.Option, (float)result);
				api.CallBack.Invoke(api);
			}
			else if (objective == API.Objective.Result_Positiveness)
			{
				double val1 = Result_positiveness[(int)CollectionStatus.Reference].Result;
				double val2 = Result_positiveness[(int)CollectionStatus.Contents].Result;
				double result = 0.5 + (val2 - val1) / 2;
				api.Set(objective, api.Option, (float)result);
				api.CallBack.Invoke(api);
			}
			else if (objective == API.Objective.Result_Empathy)
			{
				double val1 = Result_empathy[(int)CollectionStatus.Reference].Result;
				double val2 = Result_empathy[(int)CollectionStatus.Contents].Result;
				double result = 0.5 + (val2 - val1) / 2;
				api.Set(objective, api.Option, (float)result);
				api.CallBack.Invoke(api);
			}

		}

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
			List<Queue<double>> _deltaScaleDataList = new List<Queue<double>>();
			List<Queue<double>> _thetaScaleDataList = new List<Queue<double>>();
			List<Queue<double>> _alphaScaleDataList = new List<Queue<double>>();
			List<Queue<double>> _betaScaleDataList = new List<Queue<double>>();
			List<Queue<double>> _gammaScaleDataList = new List<Queue<double>>();

			List<List<double>> deltaScaleDataList = new List<List<double>>();
			List<List<double>> thetaScaleDataList = new List<List<double>>();
			List<List<double>> alphaScaleDataList = new List<List<double>>();
			List<List<double>> betaScaleDataList = new List<List<double>>();
			List<List<double>> gammaScaleDataList = new List<List<double>>();

			// 뇌파 센서의 수 만큼 각각의 데이터리스트를 초기화한다.
			for (int i = 0; i < Enum.GetValues(typeof(EEGSensorID)).Length; i++)
			{
				_deltaScaleDataList.Add(new Queue<double>());
				_thetaScaleDataList.Add(new Queue<double>());
				_alphaScaleDataList.Add(new Queue<double>());
				_betaScaleDataList.Add(new  Queue<double>());
				_gammaScaleDataList.Add(new Queue<double>());

				deltaScaleDataList.Add(new List<double>());
				thetaScaleDataList.Add(new List<double>());
				alphaScaleDataList.Add(new List<double>());
				betaScaleDataList.Add(new List<double>());
				gammaScaleDataList.Add(new List<double>());
			}

			while (this.gameObject.activeSelf)
			{
				// 0.1초 간격으로 대기
				yield return null;

                //Debug.Log("1");

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

							if (!double.IsInfinity(deltaValue) && !double.IsNaN(deltaValue)) _deltaScaleDataList[i].Enqueue(deltaValue);
							if (!double.IsInfinity(thetaValue) && !double.IsNaN(thetaValue)) _thetaScaleDataList[i].Enqueue(thetaValue);
							if (!double.IsInfinity(alphaValue) && !double.IsNaN(alphaValue)) _alphaScaleDataList[i].Enqueue(alphaValue);
							if (!double.IsInfinity(betaValue) && !double.IsNaN(betaValue))   _betaScaleDataList[i].Enqueue(betaValue);
							if (!double.IsInfinity(gammaValue) && !double.IsNaN(gammaValue)) _gammaScaleDataList[i].Enqueue(gammaValue);

							ControlEEGArray(_deltaScaleDataList[i], eegMaximumCount);
							ControlEEGArray(_thetaScaleDataList[i], eegMaximumCount);
							ControlEEGArray(_alphaScaleDataList[i], eegMaximumCount);
							ControlEEGArray(_betaScaleDataList[i], eegMaximumCount);
							ControlEEGArray(_gammaScaleDataList[i], eegMaximumCount);

							//if (!double.IsInfinity(deltaValue) && !double.IsNaN(deltaValue)) deltaScaleDataList[i].Add(deltaValue);
							//if (!double.IsInfinity(thetaValue) && !double.IsNaN(thetaValue)) thetaScaleDataList[i].Add(thetaValue);
							//if (!double.IsInfinity(alphaValue) && !double.IsNaN(alphaValue)) alphaScaleDataList[i].Add(alphaValue);
							//if (!double.IsInfinity(betaValue) && !double.IsNaN(betaValue))   betaScaleDataList[i].Add(betaValue);
							//if (!double.IsInfinity(gammaValue) && !double.IsNaN(gammaValue)) gammaScaleDataList[i].Add(gammaValue);
						}

						Delta[i].SetScale(_deltaScaleDataList[i].ToList());
						Theta[i].SetScale(_thetaScaleDataList[i].ToList());
						Alpha[i].SetScale(_alphaScaleDataList[i].ToList());
						Beta[i].SetScale (_betaScaleDataList[i] .ToList());
						Gamma[i].SetScale(_gammaScaleDataList[i].ToList());

						// 센서별로 스케일 설정 (min, max 값 설정)
						//Delta[i].SetScale(deltaScaleDataList[i]);
						//Theta[i].SetScale(thetaScaleDataList[i]);
						//Alpha[i].SetScale(alphaScaleDataList[i]);
						//Beta[i].SetScale(betaScaleDataList[i]);
						//Gamma[i].SetScale(gammaScaleDataList[i]);
					}
				}
			}


			yield break;
		}

		private void ControlEEGArray(Queue<double> values, int maximumCount)
        {
			int index = values.Count;

			int diff = index - maximumCount;
			if(index > maximumCount)
            {
                for (int i = 0; i < diff; i++)
                {
					values.Dequeue();
                }
            }
        }

		IEnumerator SetMindIndex()
		{
			//concentration;
			//excitement;
			//positiveness;
			//empathy;
			while(gameObject.activeSelf)
			{
				yield return null;

				// MindIndex의 값을 할당한다.
				switch (_CollectionStatus)
				{
					case CollectionStatus.Stanby:
						{
							//concentration[(int)collectionStatus]
						}
						break;

					case CollectionStatus.Reference:
						{
							result_concentration[(int)_CollectionStatus].Add(concentration, eegMaximumCount);

							result_excitement[(int)_CollectionStatus].Add(excitement, eegMaximumCount);

							result_positiveness[(int)_CollectionStatus].Add(positiveness, eegMaximumCount);

							result_empathy[(int)_CollectionStatus].Add(empathy, eegMaximumCount);

							//result_concentration[(int)_CollectionStatus].DataCount++;
							//result_concentration[(int)_CollectionStatus].DataSum += concentration;

							//result_excitement[(int)_CollectionStatus].DataCount++;
							//result_excitement[(int)_CollectionStatus].DataSum += excitement;

							//result_positiveness[(int)_CollectionStatus].DataCount++;
							//result_positiveness[(int)_CollectionStatus].DataSum += positiveness;

							//result_empathy[(int)_CollectionStatus].DataCount++;
							//result_empathy[(int)_CollectionStatus].DataSum += empathy;
						}
						break;

					case CollectionStatus.Contents:
						{
							result_concentration[(int)_CollectionStatus].Add(concentration, eegMaximumCount);

							result_excitement[(int)_CollectionStatus].Add(excitement, eegMaximumCount);

							result_positiveness[(int)_CollectionStatus].Add(positiveness, eegMaximumCount);

							result_empathy[(int)_CollectionStatus].Add(empathy, eegMaximumCount);

							SetMindResult(_CollectionStatus);
						}
						break;
				}
			}

			yield break;
		}

		private void SetMindResult(CollectionStatus _status)
		{
			result_concentration[(int)_status].SetResult();
			result_excitement[(int)_status].SetResult();
			result_positiveness[(int)_status].SetResult();
			result_empathy[(int)_status].SetResult();
		}
	}
}
