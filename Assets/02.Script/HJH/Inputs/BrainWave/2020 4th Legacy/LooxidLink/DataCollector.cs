using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Looxid.Link
{
    //public enum Tab2DVisualizer
    //{
    //    SENSOR_STATUS = 0,
    //    MIND_INDEX = 1,
    //    FEATURE_INDEX = 2,
    //    RAW_SIGNAL = 3
    //}

    public class DataCollector : MonoBehaviour
    {
        #region Singleton

        private static DataCollector _instance;

        public static DataCollector Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(DataCollector)) as DataCollector;
                    if (_instance == null)
                    {
                        _instance = new GameObject("DataCollector").AddComponent<DataCollector>();
                        DontDestroyOnLoad(_instance.gameObject);
                    }
                }
                return _instance;
            }
        }

        #endregion


        #region 변수 선언부
        [SerializeField] private Manager.DataManager dataManager;
        
        #region Debug
        [Header("Debug")]
        public bool isDebug;

        public GameObject debugBoards;

        public Valve.VR.InteractionSystem.Hand leftHand;
        public Valve.VR.InteractionSystem.Hand rightHand;
        #endregion

        private Data.CollectionStatus dataCollectionStatus;

        public Manager.DataManager DataManager
        {
            get => dataManager;
            private set => dataManager = value;
        }

        public Data.CollectionStatus DataCollectionStatus
        {
            get => dataCollectionStatus;
            set
            {
                dataCollectionStatus = value;
                eegOrder.collectionStatus = value;      // EEG 명령코드 collection 상태변경
            }
        }

        private EEG.EEGOrder eegOrder;  // EEG 명령코드

        private double[] eegValue;

        [Header("Raw Signal")]
        public double[] AF3_RawData;
        public double[] AF4_RawData;
        public double[] Fp1_RawData;
        public double[] Fp2_RawData;
        public double[] AF7_RawData;
        public double[] AF8_RawData;

        private EEGSensor sensorStatusData;
        private EEGRawSignal rawSignalData;

        private LinkDataValue leftActivity;
        private LinkDataValue rightActivity;
        private LinkDataValue attention;
        private LinkDataValue relaxation;

        private LinkDataValue[] delta;
        private LinkDataValue[] theta;
        private LinkDataValue[] alpha;
        private LinkDataValue[] beta;
        private LinkDataValue[] gamma;
        #endregion

        #region [Unity] Start Method

        private void Awake()
        {
            eegOrder = new EEG.EEGOrder();
            eegValue = new double[5];
        }

        void Start()
        {
            if(isDebug == true)
            {
                debugBoards.SetActive(true);
            }
            else
            {
                debugBoards.SetActive(false);
            }

            if(DataCollector.Instance != null)
            {
                Debug.Log("DataCollector instantiated");
            }

            LooxidLinkManager.Instance.SetDebug(true);      // TODO lst : debug on off 확인
            LooxidLinkManager.Instance.Initialize();        // TODO lst : LooxidLinkManager 초기화

            leftActivity = new LinkDataValue();
            rightActivity = new LinkDataValue();
            attention = new LinkDataValue();
            relaxation = new LinkDataValue();

            delta = new LinkDataValue[Enum.GetValues(typeof(EEGSensorID)).Length];
            theta = new LinkDataValue[Enum.GetValues(typeof(EEGSensorID)).Length];
            alpha = new LinkDataValue[Enum.GetValues(typeof(EEGSensorID)).Length];
            beta  = new LinkDataValue[Enum.GetValues(typeof(EEGSensorID)).Length];
            gamma = new LinkDataValue[Enum.GetValues(typeof(EEGSensorID)).Length];

            for (int i = 0; i < Enum.GetValues(typeof(EEGSensorID)).Length; i++)
            {
                delta[i] = new LinkDataValue();
                theta[i] = new LinkDataValue();
                alpha[i] = new LinkDataValue();
                beta[i]  = new LinkDataValue();
                gamma[i] = new LinkDataValue();
            }
        }

        #endregion

        #region OnEnable, OnDisable 이벤트

        /// <summary>
        /// Scene이 활성화 되었을때, 이벤트에 메서드를 연결한다.
        /// </summary>
        void OnEnable()
        {
            LooxidLinkManager.OnLinkCoreConnected += OnLinkCoreConncetd;                // 링크 코어가 연결되었을때 실행되는 메서드
            LooxidLinkManager.OnLinkHubConnected += OnLinkHubConnected;                 // 링크 허브가 연결되었을때 실행되는 메서드
            LooxidLinkManager.OnLinkCoreDisconnected += OnLinkCoreDisconncetd;          // 링크 코어가 끊겼을때 실행되는 메서드
            LooxidLinkManager.OnLinkHubDisconnected += OnLinkHubDisconnected;           // 링크 허브가 끊겼을때 실행되는 메서드
            LooxidLinkManager.OnShowSensorOffMessage += OnShowSensorOffMessage;         // 센서 연결 끊김 메세지가 뜰때 실행되는 메서드
            LooxidLinkManager.OnHideSensorOffMessage += OnHideSensorOffMessage;         // 센서 연결 끊김 메세지가 사라질때 실행되는 메서드
            LooxidLinkManager.OnShowNoiseSignalMessage += OnShowNoiseSignalMessage;     // 노이즈가 감지됨 메세지가 뜰때 실행되는 메서드
            LooxidLinkManager.OnHideNoiseSignalMessage += OnHideNoiseSignalMessage;     // 노이즈가 감지됨 메세지가 사라질때 실행되는 메서드

            LooxidLinkData.OnReceiveEEGSensorStatus += OnReceiveEEGSensorStatus;        // EEG 센서의 상태를 수신하는 메서드
            LooxidLinkData.OnReceiveEEGRawSignals += OnReceiveEEGRawSignals;            // TODO lst : * Raw signal을 받는 메서드 *
            LooxidLinkData.OnReceiveMindIndexes += OnReceiveMindIndexes;                // 마음 상태인식을 하는 메서드
            LooxidLinkData.OnReceiveEEGFeatureIndexes += OnReceiveEEGFeatureIndexes;    // EEG의 특징 값을 추출하는 메서드

            StartCoroutine(DisplayData());
        }

        /// <summary>
        /// Scene이 비활성화 되었을때, 이벤트에 메서드를 분리한다.
        /// </summary>
        void OnDisable()
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

        #region LooxidLinkManager : 장비 연결 이벤트에 연결된 메서드 설명

        #region LooxidLinkManager : 링크 코어, 허브 연결 또는 분리시 실행되는 메서드

        /// <summary>
        /// 링크 코어가 연결되었을때 실행되는 메서드
        /// </summary>
        void OnLinkCoreConncetd()
        {
            
        }

        /// <summary>
        /// 링크 허브가 연결되었을때 실행되는 메서드
        /// </summary>
        void OnLinkHubConnected()
        {
            
        }

        /// <summary>
        /// 링크 코어가 끊겼을때 실행되는 메서드
        /// </summary>
        void OnLinkCoreDisconncetd()
        {
            
        }

        /// <summary>
        /// 링크 허브가 끊겼을때 실행되는 메서드
        /// </summary>
        void OnLinkHubDisconnected()
        {
            
        }

        #endregion

        #region LooxidLinkManager : 센서 연결 또는 끊김시 실행되는 메서드

        /// <summary>
        /// 센서 연결 끊김 메세지가 뜰때 실행되는 메서드
        /// </summary>
        void OnShowSensorOffMessage()
        {
            
        }

        /// <summary>
        /// 센서 연결 끊김 메세지가 사라질때 실행되는 메서드
        /// </summary>
        void OnHideSensorOffMessage()
        {

        }

        #endregion

        #region LooxidLinkManager : 노이즈 감지에 관해 실행되는 메서드

        /// <summary>
        /// 노이즈가 감지됨 메세지가 뜰때 실행되는 메서드
        /// </summary>
        void OnShowNoiseSignalMessage()
        {
            
        }

        /// <summary>
        /// 노이즈가 감지됨 메세지가 사라질때 실행되는 메서드
        /// </summary>
        void OnHideNoiseSignalMessage()
        {
            
        }

        #endregion

        #endregion

        #region LooxidLinkData : 데이터 연결 이벤트에 연결된 메서드 설명

        /// <summary>
        /// EEG 센서의 상태를 수신하는 메서드
        /// </summary>
        /// <param name="sensorStatusData"></param>
        void OnReceiveEEGSensorStatus(EEGSensor sensorStatusData)
        {
            this.sensorStatusData = sensorStatusData;

            //sensorStatusData.IsSensorOn(EEGSensorID.AF3);     // Ex)
        }

        /// <summary>
        /// 마음 상태인식을 하는 메서드
        /// </summary>
        /// <param name="mindIndexData"></param>
        void OnReceiveMindIndexes(MindIndex mindIndexData)
        {
            leftActivity.target = double.IsNaN(mindIndexData.leftActivity) ? 0.0f : (float)LooxidLinkUtility.Scale(LooxidLink.MIND_INDEX_SCALE_MIN, LooxidLink.MIND_INDEX_SCALE_MAX, 0.0f, 100.0f, mindIndexData.leftActivity);
            rightActivity.target = double.IsNaN(mindIndexData.rightActivity) ? 0.0f : (float)LooxidLinkUtility.Scale(LooxidLink.MIND_INDEX_SCALE_MIN, LooxidLink.MIND_INDEX_SCALE_MAX, 0.0f, 100.0f, mindIndexData.rightActivity);
            attention.target = double.IsNaN(mindIndexData.attention) ? 0.0f : (float)LooxidLinkUtility.Scale(LooxidLink.MIND_INDEX_SCALE_MIN, LooxidLink.MIND_INDEX_SCALE_MAX, 0.0f, 100.0f, mindIndexData.attention);
            relaxation.target = double.IsNaN(mindIndexData.relaxation) ? 0.0f : (float)LooxidLinkUtility.Scale(LooxidLink.MIND_INDEX_SCALE_MIN, LooxidLink.MIND_INDEX_SCALE_MAX, 0.0f, 100.0f, mindIndexData.relaxation);
        }

        /// <summary>
        /// TODO lst : * Raw signal을 받는 메서드 *
        /// </summary>
        /// <param name="rawSignalData"></param>
        void OnReceiveEEGRawSignals(EEGRawSignal rawSignalData)
        {
            AF3_RawData = rawSignalData.FilteredRawSignal(EEGSensorID.AF3);
            AF4_RawData = rawSignalData.FilteredRawSignal(EEGSensorID.AF4);
            Fp1_RawData = rawSignalData.FilteredRawSignal(EEGSensorID.Fp1);
            Fp2_RawData = rawSignalData.FilteredRawSignal(EEGSensorID.Fp2);
            AF7_RawData = rawSignalData.FilteredRawSignal(EEGSensorID.AF7);
            AF8_RawData = rawSignalData.FilteredRawSignal(EEGSensorID.AF8);
        }

        /// <summary>
        /// EEG의 특징 값을 추출하는 메서드
        /// </summary>
        /// <param name="featureIndexData"></param>
        void OnReceiveEEGFeatureIndexes(EEGFeatureIndex featureIndexData)
        {
            SendEEGData(featureIndexData, EEGSensorID.AF3);
            SendEEGData(featureIndexData, EEGSensorID.AF4);
            SendEEGData(featureIndexData, EEGSensorID.Fp1);
            SendEEGData(featureIndexData, EEGSensorID.Fp2);
            SendEEGData(featureIndexData, EEGSensorID.AF7);
            SendEEGData(featureIndexData, EEGSensorID.AF8);
        }

        /// <summary>
        /// 센서별 EEG 데이터 
        /// </summary>
        /// <param name="featureIndexData">  </param>
        /// <param name="sensorID">  </param>
        private void SendEEGData(EEGFeatureIndex featureIndexData, EEGSensorID sensorID)
        {
            if ((int)dataCollectionStatus > 0)
            {
                //eegValue[0] = featureIndexData.Delta(sensorID);
                //eegValue[1] = featureIndexData.Theta(sensorID);
                //eegValue[2] = featureIndexData.Alpha(sensorID);
                //eegValue[3] = featureIndexData.Beta(sensorID);
                //eegValue[4] = featureIndexData.Gamma(sensorID);

                double deltaValue = featureIndexData.Delta(sensorID);
                double thetaValue = featureIndexData.Theta(sensorID);
                double alphaValue = featureIndexData.Alpha(sensorID);
                double betaValue = featureIndexData.Beta(sensorID);
                double gammaValue = featureIndexData.Gamma(sensorID);

                //TODO : 값 보간 재확인 필요
                eegValue[0] = (double.IsInfinity(deltaValue) || double.IsNaN(deltaValue)) ? 0.0f : LooxidLinkUtility.Scale(delta[(int)sensorID].min, delta[(int)sensorID].max, 0.0f, 100.0f, deltaValue);
                eegValue[1] = (double.IsInfinity(thetaValue) || double.IsNaN(thetaValue)) ? 0.0f : LooxidLinkUtility.Scale(theta[(int)sensorID].min, theta[(int)sensorID].max, 0.0f, 100.0f, thetaValue);
                eegValue[2] = (double.IsInfinity(alphaValue) || double.IsNaN(alphaValue)) ? 0.0f : LooxidLinkUtility.Scale(alpha[(int)sensorID].min, alpha[(int)sensorID].max, 0.0f, 100.0f, alphaValue);
                eegValue[3] = (double.IsInfinity(betaValue)  || double.IsNaN(betaValue))  ? 0.0f : LooxidLinkUtility.Scale(beta[(int)sensorID].min, beta[(int)sensorID].max, 0.0f, 100.0f, betaValue);
                eegValue[4] = (double.IsInfinity(gammaValue) || double.IsNaN(gammaValue)) ? 0.0f : LooxidLinkUtility.Scale(gamma[(int)sensorID].min, gamma[(int)sensorID].max, 0.0f, 100.0f, gammaValue);

                //eegValue[0] = (double.IsInfinity(eegValue[0]) || double.IsNaN(eegValue[0])) ? 0.0f : eegValue[0];
                //eegValue[1] = (double.IsInfinity(eegValue[1]) || double.IsNaN(eegValue[1])) ? 0.0f : eegValue[1];
                //eegValue[2] = (double.IsInfinity(eegValue[2]) || double.IsNaN(eegValue[2])) ? 0.0f : eegValue[2];
                //eegValue[3] = (double.IsInfinity(eegValue[3]) || double.IsNaN(eegValue[3])) ? 0.0f : eegValue[3];
                //eegValue[4] = (double.IsInfinity(eegValue[4]) || double.IsNaN(eegValue[4])) ? 0.0f : eegValue[4];

                //eegValue = GetEEGPercentData(eegValue);

                eegOrder.eegDataArray[(int)Data.EEG.Delta] = eegValue[0];
                eegOrder.eegDataArray[(int)Data.EEG.Theta] = eegValue[1];
                eegOrder.eegDataArray[(int)Data.EEG.Alpha] = eegValue[2];
                eegOrder.eegDataArray[(int)Data.EEG.Beta] = eegValue[3];
                eegOrder.eegDataArray[(int)Data.EEG.Gamma] = eegValue[4];

                dataManager.GetEEGData(sensorID, eegOrder);
            }
        }

        private double[] GetEEGPercentData(double[] eegValue)
        {
            double[] result = new double[5];

            double sum = Math.Abs(eegValue[0] + eegValue[1] + eegValue[2] + eegValue[3] + eegValue[4]);

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Math.Abs(eegValue[i]) / sum * 100;
            }

            return result;
        }

        IEnumerator DisplayData()
        {
            // 데이터 선언, 초기화
            List<List<double>> deltaScaleDataList = new List<List<double>>();
            List<List<double>> thetaScaleDataList = new List<List<double>>();
            List<List<double>> alphaScaleDataList = new List<List<double>>();
            List<List<double>> betaScaleDataList  = new List<List<double>>();
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

                if(featureIndexList.Count > 0)
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
                            double betaValue  = featureIndexList[i].Beta((EEGSensorID)i);
                            double gammaValue = featureIndexList[i].Gamma((EEGSensorID)i);

                            if (!double.IsInfinity(deltaValue) && !double.IsNaN(deltaValue)) deltaScaleDataList[i].Add(deltaValue);
                            if (!double.IsInfinity(thetaValue) && !double.IsNaN(thetaValue)) thetaScaleDataList[i].Add(thetaValue);
                            if (!double.IsInfinity(alphaValue) && !double.IsNaN(alphaValue)) alphaScaleDataList[i].Add(alphaValue);
                            if (!double.IsInfinity(betaValue) && !double.IsNaN(betaValue)) betaScaleDataList[i].Add(betaValue);
                            if (!double.IsInfinity(gammaValue) && !double.IsNaN(gammaValue)) gammaScaleDataList[i].Add(gammaValue);
                        }


                        delta[i].SetScale(deltaScaleDataList[i]);
                        theta[i].SetScale(thetaScaleDataList[i]);
                        alpha[i].SetScale(alphaScaleDataList[i]);
                        beta[i].SetScale(betaScaleDataList[i]);
                        gamma[i].SetScale(gammaScaleDataList[i]);
                    }

                }
            }
        }

        #endregion

        /// <summary>
        /// 룩시드링크에서 제공하는 데이터 수집
        /// Attention ( 집중도 )
        /// Relaxation ( 이완도 )
        /// 수집 가능
        /// </summary>
        /// <param name="mindIndex"></param>
        /// <returns></returns>
        public double GetData(Data.MindIndex mindIndex)
        {
            switch(mindIndex)
            {
                case Data.MindIndex.Attention:
                    return attention.value;

                case Data.MindIndex.Relaxation:
                    return relaxation.value;
            }
            return 0;
        }

        #region Debug, Update

        void Update()
        {
            leftActivity.value = Mathf.Lerp((float)leftActivity.value, (float)leftActivity.target, 0.2f);
            rightActivity.value = Mathf.Lerp((float)rightActivity.value, (float)rightActivity.target, 0.2f);
            attention.value = Mathf.Lerp((float)attention.value, (float)attention.target, 0.2f);
            relaxation.value = Mathf.Lerp((float)relaxation.value, (float)relaxation.target, 0.2f);
        }

        #endregion
    }
}