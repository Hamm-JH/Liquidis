using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EEG
{
    /// <summary>
    /// 센서별 EEG 데이터 수집 클래스
    /// </summary>
    public class EEGData : MonoBehaviour
    {
        #region 변수 선언부
        private int collectionBoundary;

        public int CollectionBoundary
        {
            get => collectionBoundary;
            set => collectionBoundary = value;
        }

        public List<List<List<double>>> EEGList;

        private List<List<double>> deltaDataList;
        private List<List<double>> thetaDataList;
        private List<List<double>> alphaDataList;
        private List<List<double>> betaDataList;
        private List<List<double>> gammaDataList;

        private List<List<double>> EEGResultList;

        private List<double> deltaResultList;
        private List<double> thetaResultList;
        private List<double> alphaResultList;
        private List<double> betaResultList;
        private List<double> gammaResultData;

        private List<List<double>> EEGPercentList;

        private List<double> deltaPercentList;
        private List<double> thetaPercentList;
        private List<double> alphaPercentList;
        private List<double> betaPercentList;
        private List<double> gammaPercentData;
        #endregion

        #region Awake : initialize values
        private void Awake()
        {
            EEGList = new List<List<List<double>>>();
            EEGResultList = new List<List<double>>();
            EEGPercentList = new List<List<double>>();

            //==========

            deltaDataList = new List<List<double>>();
            thetaDataList = new List<List<double>>();
            alphaDataList = new List<List<double>>();
            betaDataList = new List<List<double>>();
            gammaDataList = new List<List<double>>();

            EEGList.Add(deltaDataList);
            EEGList.Add(thetaDataList);
            EEGList.Add(alphaDataList);
            EEGList.Add(betaDataList);
            EEGList.Add(gammaDataList);

            //==========

            deltaResultList = new List<double>();
            thetaResultList = new List<double>();
            alphaResultList = new List<double>();
            betaResultList = new List<double>();
            gammaResultData = new List<double>();

            EEGResultList.Add(deltaResultList);
            EEGResultList.Add(thetaResultList);
            EEGResultList.Add(alphaResultList);
            EEGResultList.Add(betaResultList);
            EEGResultList.Add(gammaResultData);

            //==========

            deltaPercentList = new List<double>();
            thetaPercentList = new List<double>();
            alphaPercentList = new List<double>();
            betaPercentList = new List<double>();
            gammaPercentData = new List<double>();

            EEGPercentList.Add(deltaPercentList);
            EEGPercentList.Add(thetaPercentList);
            EEGPercentList.Add(alphaPercentList);
            EEGPercentList.Add(betaPercentList);
            EEGPercentList.Add(gammaPercentData);
        }
        #endregion

        #region Start : Enum 요소 개수를 참조해 리스트 요소 할당
        // Start is called before the first frame update
        void Start()
        {
            // Data.CollectionStatus에 따라 리스트 생성한다.
            for (int i = 0; i < Enum.GetValues(typeof(Data.CollectionStatus)).Length; i++)
            {
                deltaDataList.Add(new List<double>());
                thetaDataList.Add(new List<double>());
                alphaDataList.Add(new List<double>());
                betaDataList.Add(new List<double>());
                gammaDataList.Add(new List<double>());

                deltaResultList.Add(0);
                thetaResultList.Add(0);
                alphaResultList.Add(0);
                betaResultList.Add(0);
                gammaResultData.Add(0);

                deltaPercentList.Add(0);
                thetaPercentList.Add(0);
                alphaPercentList.Add(0);
                betaPercentList.Add(0);
                gammaPercentData.Add(0);
            }
        }
        #endregion


        /// <summary>
        /// 데이터가 발생할 때마다 데이터 수집
        /// </summary>
        /// <param name="_EEGOrder"> 데이터가 발생한 타이밍에 수집된 데이터를 모은 인스턴스 </param>
        public void GetEEGData(EEG.EEGOrder _EEGOrder)
        {
            for (int i = 0; i < Enum.GetValues(typeof(Data.EEG)).Length; i++)
            {
                if(EEGList[i][(int)_EEGOrder.collectionStatus].Count >= CollectionBoundary)
                {
                    EEGResultList[i][(int)_EEGOrder.collectionStatus] -= EEGList[i][(int)_EEGOrder.collectionStatus][0];
                    EEGList[i][(int)_EEGOrder.collectionStatus].RemoveAt(0);
                }
                EEGList[i][(int)_EEGOrder.collectionStatus].Add(_EEGOrder.eegDataArray[i]);
                EEGResultList[i][(int)_EEGOrder.collectionStatus] += _EEGOrder.eegDataArray[i];
                // TODO : 평균치 실시간 계산
                SetEEGPercentData(_EEGOrder);
            }
        }

        private void SetEEGPercentData(EEG.EEGOrder _EEGOrder)
        {
            for (int i = 0; i < Enum.GetValues(typeof(Data.EEG)).Length; i++)
            {
                if(EEGList[i][(int)_EEGOrder.collectionStatus].Count > 0)
                {
                    EEGPercentList[i][(int)_EEGOrder.collectionStatus] = 
                        ValidatePercentData( EEGResultList[i][(int)_EEGOrder.collectionStatus] / EEGList[i][(int)_EEGOrder.collectionStatus].Count * 100);
                }
            }
        }

        public double GetEEGPercentData(Data.EEG eeg, Data.CollectionStatus collectionStatus)
        {
            return EEGPercentList[(int)eeg][(int)collectionStatus];
        }

        /// <summary>
        /// 백분율 데이터 유효성 검사/반환
        /// </summary>
        private double ValidatePercentData(double data)
        {
            double result = Mathf.Epsilon + data;

            if (data >= 0 && data <= 100)
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        //==================================================

        /// <summary>
        /// 흥분도 계산, 반환
        /// </summary>
        /// <param name="collectionStatus"></param>
        /// <returns></returns>
        public double? GetExcitementData(Data.CollectionStatus collectionStatus)
        {
            if ((int)collectionStatus < 2) return null;

            if(EEGList[(int)Data.EEG.Gamma][(int)Data.CollectionStatus.Reference].Count > 0
                && EEGPercentList[(int)Data.EEG.Gamma][(int)Data.CollectionStatus.Reference] != double.NaN)
            {
                if (EEGList[(int)Data.EEG.Gamma][(int)collectionStatus].Count > 0
                    && EEGPercentList[(int)Data.EEG.Gamma][(int)collectionStatus] != double.NaN)
                {
                    double result = (100 - Math.Abs(EEGPercentList[(int)Data.EEG.Gamma][(int)collectionStatus] - EEGPercentList[(int)Data.EEG.Gamma][(int)Data.CollectionStatus.Reference])
                        / EEGPercentList[(int)Data.EEG.Gamma][(int)Data.CollectionStatus.Reference]);
                    return ValidatePercentData(result);
                }
            }

            return null;
        }


        /// <summary>
        /// 특정 센서의 긍부정도 값을 받는다.
        /// </summary>
        /// <param name="collectionStatus"></param>
        /// <returns></returns>
        public double? GetPositivityData(Data.CollectionStatus collectionStatus)
        {
            if ((int)collectionStatus < 2) return null;

            if(EEGList[(int)Data.EEG.Theta][(int)Data.CollectionStatus.Reference].Count > 0
                && EEGPercentList[(int)Data.EEG.Theta][(int)Data.CollectionStatus.Reference] != double.NaN)
            {
                if(EEGList[(int)Data.EEG.Theta][(int)collectionStatus].Count > 0
                    && EEGPercentList[(int)Data.EEG.Theta][(int)collectionStatus] != double.NaN)
                {
                    double referenceIndex = EEGPercentList[(int)Data.EEG.Theta][(int)Data.CollectionStatus.Reference];
                    double collectionIndex = EEGPercentList[(int)Data.EEG.Theta][(int)collectionStatus];
                    double? resultIndex = 0;

                    if(collectionIndex > referenceIndex)
                    {
                        resultIndex = 50 - (collectionIndex - referenceIndex) / referenceIndex * 100;
                    }
                    else if(collectionIndex < referenceIndex)
                    {
                        resultIndex = 50 + (referenceIndex - collectionIndex) / referenceIndex * 100;
                    }
                    else
                    {
                        resultIndex = 50;
                    }
                    return resultIndex;
                }
            }
            return 0;
        }

        public double? GetEmpathyData(Data.CollectionStatus collectionStatus)
        {
            if ((int)collectionStatus < 2) return null;

            if (EEGList[(int)Data.EEG.Alpha][(int)Data.CollectionStatus.Reference].Count > 0
                && EEGPercentList[(int)Data.EEG.Alpha][(int)Data.CollectionStatus.Reference] != double.NaN)
            {
                if (EEGList[(int)Data.EEG.Alpha][(int)collectionStatus].Count > 0
                    && EEGPercentList[(int)Data.EEG.Alpha][(int)collectionStatus] != double.NaN)
                {
                    double referenceIndex = EEGPercentList[(int)Data.EEG.Alpha][(int)Data.CollectionStatus.Reference];
                    double collectionIndex = EEGPercentList[(int)Data.EEG.Alpha][(int)collectionStatus];
                    double? resultIndex = 0;

                    if (collectionIndex > referenceIndex)
                    {
                        resultIndex = 50 - (collectionIndex - referenceIndex) / referenceIndex * 100;
                    }
                    else if (collectionIndex < referenceIndex)
                    {
                        resultIndex = 50 + (referenceIndex - collectionIndex) / referenceIndex * 100;
                    }
                    else
                    {
                        resultIndex = 50;
                    }
                    return resultIndex;
                }
            }

            return 0;
        }


    }
}
