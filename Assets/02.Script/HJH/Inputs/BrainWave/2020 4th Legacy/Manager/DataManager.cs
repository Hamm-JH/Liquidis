using Looxid.Link;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class DataManager : MonoBehaviour
    {
        #region Singleton

        private static DataManager _instance;

        public static DataManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(DataManager)) as DataManager;
                    if (_instance == null)
                    {
                        _instance = new GameObject("DataManager").AddComponent<DataManager>();
                        DontDestroyOnLoad(_instance.gameObject);
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region 변수 선언부
        [SerializeField] private DataCollector collector;
        private List<EEG.EEGData> eegDataList;
        private Data.CollectionStatus collectionStatus;
        private int collectionBoundary;
        
        List<List<double>> deltaScaleDataList;
        List<List<double>> thetaScaleDataList;
        List<List<double>> alphaScaleDataList;
        List<List<double>> betaScaleDataList;
        List<List<double>> gammaScaleDataList;

        public DataCollector Collector
        {
            get => collector;
            private set => collector = value;
        }

        public List<EEG.EEGData> EEGDataList
        {
            get => eegDataList;
            private set => eegDataList = value;
        }

        public Data.CollectionStatus CollectionStatus
        {
            get => collectionStatus;
            set
            {
                collectionStatus = value;
                Collector.DataCollectionStatus = value;
            }
        }

        public int CollectionBoundary
        {
            get => collectionBoundary;
            set
            {
                collectionBoundary = value;
                for (int i = 0; i < EEGDataList.Count; i++)
                {
                    EEGDataList[i].CollectionBoundary = value;
                }
            }
        }
        #endregion

        #region Awake : initialize values

        private void Awake()
        {
            EEGDataList = new List<EEG.EEGData>();
        }

        #endregion

        #region Start : Enum 요소 개수를 참조해 리스트 요소 할당

        // Start is called before the first frame update
        void Start()
        {
            deltaScaleDataList = new List<List<double>>();
            thetaScaleDataList = new List<List<double>>();
            alphaScaleDataList = new List<List<double>>();
            betaScaleDataList  = new List<List<double>>();
            gammaScaleDataList = new List<List<double>>();

            for (int i = 0; i < Enum.GetValues(typeof(EEGSensorID)).Length; i++)
            {
                deltaScaleDataList.Add(new List<double>());
                thetaScaleDataList.Add(new List<double>());
                alphaScaleDataList.Add(new List<double>());
                betaScaleDataList.Add(new List<double>());
                gammaScaleDataList.Add(new List<double>());
            }

            // 센서의 개수만큼 
            for (int i = 0; i < Enum.GetValues(typeof(Looxid.Link.EEGSensorID)).Length; i++)
            {
                EEG.EEGData eegData = gameObject.AddComponent<EEG.EEGData>();
                EEGDataList.Add(eegData);
            }

            CollectionBoundary = collectionBoundary;
        }

        #endregion

        #region Enum 요소 번호에 위치한 리스트에 수집 데이터 저장

        /// <summary>
        /// 센서에서 데이터가 발생할 때마다 데이터 수집
        /// </summary>
        /// <param name="_EEGSensorID"> 센서의 ID </param>
        /// <param name="_EEGOrder"> 센서에서 수집한 데이터클래스 </param>
        public void GetEEGData(Looxid.Link.EEGSensorID _EEGSensorID, EEG.EEGOrder _EEGOrder)
        {
            EEGDataList[(int)_EEGSensorID].GetEEGData(_EEGOrder);
        }

        #endregion

        //===============================================================================

        #region TimeStamp X : MindIndex 데이터 요청

        /// <summary>
        /// 현재 컨텐츠 상황에서 수집한 데이터의 MindIndex 데이터 요청
        /// </summary>
        /// <param name="mindIndex"></param>
        /// <returns></returns>
        public double GetMindIndex(Data.MindIndex mindIndex)
        {
            double index;
            double?[] indexArray;
            //double?[] indexArray = GetIndexResultArray(mindIndex, DataManager.Instance.CollectionStatus);

            switch(mindIndex)
            {
                case Data.MindIndex.Excitement:
                    index = 100 - Collector.GetData(Data.MindIndex.Relaxation);
                    return index;

                case Data.MindIndex.Positivity:
                case Data.MindIndex.Empathy:
                    indexArray = GetIndexResultArray(mindIndex, CollectionStatus);
                    index = 0;

                    if (indexArray == null) return 0;

                    for (int i = 0; i < indexArray.Length; i++)
                    {
                        if (indexArray[i] == null) continue;

                        index += (double)indexArray[i];
                    }

                    index = index / indexArray.Length;
                    return index;


                case Data.MindIndex.Attention:
                    index = Collector.GetData(Data.MindIndex.Attention);
                    return index;

                case Data.MindIndex.Relaxation:
                    index = Collector.GetData(Data.MindIndex.Relaxation);
                    return index;
            }

            return 0;
        }

        /// <summary>
        /// 특정 컨텐츠에 수집한 데이터의 MindIndex 데이터 요청
        /// </summary>
        /// <param name="mindIndex"></param>
        /// <param name="_collectionStatus"></param>
        /// <returns></returns>
        public double GetMindIndex(Data.MindIndex mindIndex, Data.CollectionStatus _collectionStatus)
        {
            double?[] indexArray;
            //double?[] indexArray = GetIndexResultArray(mindIndex, DataManager.Instance.CollectionStatus);
            double index;

            switch (mindIndex)
            {
                case Data.MindIndex.Excitement:
                    index = 100 - Collector.GetData(Data.MindIndex.Relaxation);
                    return index;

                case Data.MindIndex.Positivity:
                    indexArray = GetIndexResultArray(mindIndex, _collectionStatus);
                    index = 0;

                    if (indexArray == null) return 0;

                    for (int i = 0; i < indexArray.Length; i++)
                    {
                        if (indexArray[i] == null) continue;

                        index += (double)indexArray[i];
                    }

                    index = index / indexArray.Length;
                    return index;

                case Data.MindIndex.Empathy:
                    indexArray = GetIndexResultArray(mindIndex, _collectionStatus);
                    index = 0;

                    if (indexArray == null) return 0;

                    for (int i = 0; i < indexArray.Length; i++)
                    {
                        if (indexArray[i] == null) continue;

                        index += (double)indexArray[i];
                    }

                    index = index / indexArray.Length;
                    return index;

                case Data.MindIndex.Attention:
                    index = Collector.GetData(Data.MindIndex.Attention);
                    return index;

                case Data.MindIndex.Relaxation:
                    index = Collector.GetData(Data.MindIndex.Relaxation);
                    return index;
            }

            return 0;
        }

        /// <summary>
        /// TimeStamp X : 수집하고자 하는 뇌파검출 결과치 요청
        /// </summary>
        /// <param name="mindIndex"></param>
        /// <param name="collectionSceneStatus"></param>
        /// <returns></returns>
        public double?[] GetIndexResultArray(Data.MindIndex mindIndex, Data.CollectionStatus collectionSceneStatus)
        {
            double?[] resultArray;
            //double?[] resultArray = new double?[Enum.GetValues(typeof(Looxid.Link.EEGSensorID)).Length];

            switch (mindIndex)
            {
                // 흥분도 값은 링크에서 제공하는 값으로 대체
                //case Data.MindIndex.Excitement:
                //    return resultArray;

                case Data.MindIndex.Positivity:
                    resultArray = new double?[3];

                    resultArray[0] = EEGDataList[(int)Looxid.Link.EEGSensorID.AF4].GetPositivityData(collectionSceneStatus);
                    resultArray[1] = EEGDataList[(int)Looxid.Link.EEGSensorID.Fp2].GetPositivityData(collectionSceneStatus);
                    resultArray[2] = EEGDataList[(int)Looxid.Link.EEGSensorID.AF8].GetPositivityData(collectionSceneStatus);

                    return resultArray;

                case Data.MindIndex.Empathy:
                    resultArray = new double?[3];

                    resultArray[0] = EEGDataList[(int)Looxid.Link.EEGSensorID.AF4].GetEmpathyData(collectionSceneStatus);
                    resultArray[1] = EEGDataList[(int)Looxid.Link.EEGSensorID.Fp2].GetEmpathyData(collectionSceneStatus);
                    resultArray[2] = EEGDataList[(int)Looxid.Link.EEGSensorID.AF8].GetEmpathyData(collectionSceneStatus);

                    return resultArray;
            }

            return null;
        }

        #endregion

        //===============================================================================

        #region TimeStamp 0 : MindIndex 데이터 요청

        public double GetMindIndex(Data.MindIndex mindIndex, float timeStamp)
        {
            double index;
            double?[] indexArray;
            List<EEGFeatureIndex> featureIndexList = LooxidLinkData.Instance.GetEEGFeatureIndexData(timeStamp);

            if(featureIndexList.Count > 0)
            {
                switch(mindIndex)
                {
                    // Def set
                    case Data.MindIndex.Excitement:
                        index = 100 - Collector.GetData(Data.MindIndex.Relaxation);
                        return index;

                    case Data.MindIndex.Positivity:
                    case Data.MindIndex.Empathy:
                        indexArray = GetTimeStampIndexResultArray(mindIndex, featureIndexList, CollectionStatus);
                        index = 0;

                        if (indexArray == null) return 0;

                        for (int i = 0; i < indexArray.Length; i++)
                        {
                            if (indexArray[i] == null) continue;

                            index += (double)indexArray[i];
                        }

                        index = index / indexArray.Length;
                        return index;

                    // Def set
                    case Data.MindIndex.Attention:
                        index = Collector.GetData(Data.MindIndex.Attention);
                        return index;

                    // Def set
                    case Data.MindIndex.Relaxation:
                        index = Collector.GetData(Data.MindIndex.Relaxation);
                        return index;
                }
                
                return 0;
            }
            else
            {
                return 0;
            }
        }

        public double GetMindIndex(Data.MindIndex mindIndex, float timeStamp, Data.CollectionStatus _collectionStatus)
        {
            double index;
            double?[] indexArray;
            List<EEGFeatureIndex> featureIndexList = LooxidLinkData.Instance.GetEEGFeatureIndexData(timeStamp);

            if (featureIndexList.Count > 0)
            {
                switch (mindIndex)
                {
                    // Def set
                    case Data.MindIndex.Excitement:
                        index = 100 - Collector.GetData(Data.MindIndex.Relaxation);
                        return index;

                    case Data.MindIndex.Positivity:
                    case Data.MindIndex.Empathy:
                        indexArray = GetTimeStampIndexResultArray(mindIndex, featureIndexList, _collectionStatus);
                        index = 0;

                        if (indexArray == null) return 0;

                        for (int i = 0; i < indexArray.Length; i++)
                        {
                            if (indexArray[i] == null) continue;

                            index += (double)indexArray[i];
                        }

                        index = index / indexArray.Length;
                        return index;

                    // Def set
                    case Data.MindIndex.Attention:
                        index = Collector.GetData(Data.MindIndex.Attention);
                        return index;

                    // Def set
                    case Data.MindIndex.Relaxation:
                        index = Collector.GetData(Data.MindIndex.Relaxation);
                        return index;
                }

                return 0;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// TimeStamp 0 : 수집하고자 하는 뇌파검출 결과치 요청
        /// </summary>
        /// <param name="mindIndex"></param>
        /// <param name="_featureIndex"></param>
        /// <param name="collectionStatus"></param>
        /// <returns></returns>
        public double?[] GetTimeStampIndexResultArray(Data.MindIndex mindIndex, List<EEGFeatureIndex> _featureIndex, Data.CollectionStatus collectionStatus)
        {
            double?[] referenceIndexList;
            double?[] resultArray;
            LinkDataValue[] EEGIndexList;

            //double deltaValue = 0;
            double thetaValue = 0;
            double alphaValue = 0;
            //double betaValue = 0;
            //double gammaValue = 0;

            switch (mindIndex)
            {
                case Data.MindIndex.Positivity:
                    {
                        if ((int)collectionStatus < 2) return null;

                        thetaValue = 0;
                    
                        resultArray = new double?[3];
                        EEGIndexList = new LinkDataValue[3];

                        // 최대/최소 값을 구하는 EEG Index List 초기화
                        for (int i = 0; i < EEGIndexList.Length; i++)
                        {
                            EEGIndexList[i] = new LinkDataValue();
                        }

                        // 센서 열거 변수 영역 내에서 검출 필요 데이터에만 접근하는 반복문
                        for (int i = 0; i < Enum.GetValues(typeof(EEGSensorID)).Length; i++)
                        {
                            switch((EEGSensorID)i)
                            {
                                case EEGSensorID.AF4:
                                case EEGSensorID.Fp2:
                                case EEGSensorID.AF8:
                                    for (int ii = 0; ii < _featureIndex.Count; ii++)
                                    {
                                        thetaValue = _featureIndex[i].Theta((EEGSensorID)i);

                                        if (!double.IsInfinity(thetaValue) && !double.IsNaN(thetaValue)) thetaScaleDataList[i].Add(thetaValue);
                                    }

                                    if((EEGSensorID)i == EEGSensorID.AF4) EEGIndexList[0].SetScale(thetaScaleDataList[i]);
                                    else if((EEGSensorID)i == EEGSensorID.Fp2) EEGIndexList[1].SetScale(thetaScaleDataList[i]);
                                    else if((EEGSensorID)i == EEGSensorID.AF8) EEGIndexList[2].SetScale(thetaScaleDataList[i]);
                                    break;
                            }
                        }

                        // 비교 대상 백분위값 도출
                        for (int i = 0; i < resultArray.Length; i++)
                        {
                            switch(i)
                            {
                                case 0:     thetaValue = _featureIndex[(int)EEGSensorID.AF4].Theta(EEGSensorID.AF4); break;
                                case 1:     thetaValue = _featureIndex[(int)EEGSensorID.Fp2].Theta(EEGSensorID.Fp2); break;
                                case 2:     thetaValue = _featureIndex[(int)EEGSensorID.AF8].Theta(EEGSensorID.AF8); break;
                            }
                        
                            resultArray[i] = (double.IsInfinity(thetaValue) || double.IsNaN(thetaValue))
                                ? 0.0f : LooxidLinkUtility.Scale(EEGIndexList[i].min, EEGIndexList[i].max, 0.0f, 100.0f, thetaValue);
                        }

                        // 레퍼런스 데이터 가져오기
                        referenceIndexList = GetReferenceIndexArray(Data.EEG.Theta, collectionStatus, EEGSensorID.AF4, EEGSensorID.Fp2, EEGSensorID.AF8);

                        // 레퍼런스 데이터와 비교대상 변수 비교, 결과치 재할당
                        for (int i = 0; i < resultArray.Length; i++)
                        {
                            double referenceIndex = (referenceIndexList[i] == null) ? 0.0 : (double)referenceIndexList[i];
                            double collectionIndex = (resultArray[i] == null) ? 0.0 : (double)resultArray[i];

                            if (collectionIndex > referenceIndex)
                            {
                                resultArray[i] = 50 - (collectionIndex - referenceIndex) / referenceIndex * 100;
                            }
                            else if (collectionIndex < referenceIndex)
                            {
                                resultArray[i] = 50 + (referenceIndex - collectionIndex) / referenceIndex * 100;
                            }
                            else
                            {
                                resultArray[i] = 50;
                            }
                        }

                        return resultArray;
                    }

                case Data.MindIndex.Empathy:
                    {
                        if ((int)collectionStatus < 2) return null;

                        alphaValue = 0;
                    
                        resultArray = new double?[3];
                        EEGIndexList = new LinkDataValue[3];

                        // 최대/최소 값을 구하는 EEG Index List 초기화
                        for (int i = 0; i < EEGIndexList.Length; i++)
                        {
                            EEGIndexList[i] = new LinkDataValue();
                        }

                        // 센서 열거 변수 영역 내에서 검출 필요 데이터에만 접근하는 반복문
                        for (int i = 0; i < Enum.GetValues(typeof(EEGSensorID)).Length; i++)
                        {
                            switch ((EEGSensorID)i)
                            {
                                case EEGSensorID.AF4:
                                case EEGSensorID.Fp2:
                                case EEGSensorID.AF8:
                                    for (int ii = 0; ii < _featureIndex.Count; ii++)
                                    {
                                        alphaValue = _featureIndex[i].Theta((EEGSensorID)i);

                                        if (!double.IsInfinity(alphaValue) && !double.IsNaN(alphaValue)) thetaScaleDataList[i].Add(alphaValue);
                                    }

                                    if ((EEGSensorID)i == EEGSensorID.AF4) EEGIndexList[0].SetScale(thetaScaleDataList[i]);
                                    else if ((EEGSensorID)i == EEGSensorID.Fp2) EEGIndexList[1].SetScale(thetaScaleDataList[i]);
                                    else if ((EEGSensorID)i == EEGSensorID.AF8) EEGIndexList[2].SetScale(thetaScaleDataList[i]);
                                    break;
                            }
                        }

                        // 비교 대상 백분위값 도출
                        for (int i = 0; i < resultArray.Length; i++)
                        {
                            switch (i)
                            {
                                case 0: alphaValue = _featureIndex[(int)EEGSensorID.AF4].Theta(EEGSensorID.AF4); break;
                                case 1: alphaValue = _featureIndex[(int)EEGSensorID.Fp2].Theta(EEGSensorID.Fp2); break;
                                case 2: alphaValue = _featureIndex[(int)EEGSensorID.AF8].Theta(EEGSensorID.AF8); break;
                            }

                            resultArray[i] = (double.IsInfinity(alphaValue) || double.IsNaN(alphaValue))
                                ? 0.0f : LooxidLinkUtility.Scale(EEGIndexList[i].min, EEGIndexList[i].max, 0.0f, 100.0f, alphaValue);
                        }

                        // 레퍼런스 데이터 가져오기
                        referenceIndexList = GetReferenceIndexArray(Data.EEG.Alpha, collectionStatus, EEGSensorID.AF4, EEGSensorID.Fp2, EEGSensorID.AF8);

                        // 레퍼런스 데이터와 비교대상 변수 비교, 결과치 재할당
                        for (int i = 0; i < resultArray.Length; i++)
                        {
                            double referenceIndex = (referenceIndexList[i] == null) ? 0.0 : (double)referenceIndexList[i];
                            double collectionIndex = (resultArray[i] == null) ? 0.0 : (double)resultArray[i];

                            if (collectionIndex > referenceIndex)
                            {
                                resultArray[i] = 50 - (collectionIndex - referenceIndex) / referenceIndex * 100;
                            }
                            else if (collectionIndex < referenceIndex)
                            {
                                resultArray[i] = 50 + (referenceIndex - collectionIndex) / referenceIndex * 100;
                            }
                            else
                            {
                                resultArray[i] = 50;
                            }
                        }

                        return resultArray;
                    }
            }

            return null;
        }

        /// <summary>
        /// TimeStamp를 포함한 MindIndex 결과치를 요청받을때,
        /// EEG 데이터 리스트에서 필요한 레퍼런스 데이터 리스트를 받는다.
        /// </summary>
        /// <param name="eeg"></param>
        /// <param name="collectionStatus"></param>
        /// <param name="sensorIDs"></param>
        /// <returns></returns>
        private double?[] GetReferenceIndexArray(Data.EEG eeg, Data.CollectionStatus collectionStatus, params EEGSensorID[] sensorIDs)
        {
            double?[] result = new double?[sensorIDs.Length];

            for (int i = 0; i < sensorIDs.Length; i++)
            {
                result[i] = EEGDataList[(int)sensorIDs[i]].GetEEGPercentData(eeg, collectionStatus);
            }

            return result;
        }

        #endregion

        //===============================================================================

        /*
         * Gamma : Gamma 구현
         * O 1 : 1 매칭
         * ? n : n 매칭
         * ? n : m 매칭
         * 공감도에 대한 고민 ( 네트워킹 / 뇌파검출이 엮이는 타이밍 ? )
         */
    }
}
