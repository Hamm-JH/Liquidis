using Looxid.Link;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
	public partial class BIManager : MonoBehaviour
	{
        void OnReceiveEEGSensorStatus(EEGSensor sensorStatusData)
        {
            this.sensorStatusData = sensorStatusData;
        }
        void OnReceiveMindIndexes(MindIndex mindIndexData)
        {
            leftActivity.target = double.IsNaN(mindIndexData.leftActivity) ? 0.0f : (float)LooxidLinkUtility.Scale(LooxidLink.MIND_INDEX_SCALE_MIN, LooxidLink.MIND_INDEX_SCALE_MAX, 0.0f, 1.0f, mindIndexData.leftActivity);
            rightActivity.target = double.IsNaN(mindIndexData.rightActivity) ? 0.0f : (float)LooxidLinkUtility.Scale(LooxidLink.MIND_INDEX_SCALE_MIN, LooxidLink.MIND_INDEX_SCALE_MAX, 0.0f, 1.0f, mindIndexData.rightActivity);
            attention.target = double.IsNaN(mindIndexData.attention) ? 0.0f : (float)LooxidLinkUtility.Scale(LooxidLink.MIND_INDEX_SCALE_MIN, LooxidLink.MIND_INDEX_SCALE_MAX, 0.0f, 1.0f, mindIndexData.attention);
            relaxation.target = double.IsNaN(mindIndexData.relaxation) ? 0.0f : (float)LooxidLinkUtility.Scale(LooxidLink.MIND_INDEX_SCALE_MIN, LooxidLink.MIND_INDEX_SCALE_MAX, 0.0f, 1.0f, mindIndexData.relaxation);
        }
        void OnReceiveEEGRawSignals(EEGRawSignal rawSignalData)
        {
			//if (AF3Chart != null) AF3Chart.SetValue(rawSignalData.FilteredRawSignal(EEGSensorID.AF3));
			//if (AF4Chart != null) AF4Chart.SetValue(rawSignalData.FilteredRawSignal(EEGSensorID.AF4));
			//if (Fp1Chart != null) Fp1Chart.SetValue(rawSignalData.FilteredRawSignal(EEGSensorID.Fp1));
			//if (Fp2Chart != null) Fp2Chart.SetValue(rawSignalData.FilteredRawSignal(EEGSensorID.Fp2));
			//if (AF7Chart != null) AF7Chart.SetValue(rawSignalData.FilteredRawSignal(EEGSensorID.AF7));
			//if (AF8Chart != null) AF8Chart.SetValue(rawSignalData.FilteredRawSignal(EEGSensorID.AF8));
		}
        void OnReceiveEEGFeatureIndexes(EEGFeatureIndex featureIndexData)
        {
            //double deltaValue = featureIndexData.Delta(SelectChannel);
            //double thetaValue = featureIndexData.Theta(SelectChannel);
            //double alphaValue = featureIndexData.Alpha(SelectChannel);
            //double betaValue = featureIndexData.Beta(SelectChannel);
            //double gammaValue = featureIndexData.Gamma(SelectChannel);

            double deltaValue = featureIndexData.Delta(EEGSensorID.AF3);
            double thetaValue = featureIndexData.Theta(EEGSensorID.AF3);
            double alphaValue = featureIndexData.Alpha(EEGSensorID.AF3);
            double betaValue = featureIndexData.Beta(EEGSensorID.AF3);
            double gammaValue = featureIndexData.Gamma(EEGSensorID.AF3);

            delta.target = (double.IsInfinity(deltaValue) || double.IsNaN(deltaValue)) ? 0.0f : LooxidLinkUtility.Scale(delta.min, delta.max, 0.0f, 1.0f, deltaValue);
            theta.target = (double.IsInfinity(thetaValue) || double.IsNaN(thetaValue)) ? 0.0f : LooxidLinkUtility.Scale(theta.min, theta.max, 0.0f, 1.0f, thetaValue);
            alpha.target = (double.IsInfinity(alphaValue) || double.IsNaN(alphaValue)) ? 0.0f : LooxidLinkUtility.Scale(alpha.min, alpha.max, 0.0f, 1.0f, alphaValue);
            beta.target = (double.IsInfinity(betaValue) || double.IsNaN(betaValue)) ? 0.0f : LooxidLinkUtility.Scale(beta.min, beta.max, 0.0f, 1.0f, betaValue);
            gamma.target = (double.IsInfinity(gammaValue) || double.IsNaN(gammaValue)) ? 0.0f : LooxidLinkUtility.Scale(gamma.min, gamma.max, 0.0f, 1.0f, gammaValue);



            //Debug.Log($"Hello");

            //Debug.Log(SelectChannel.ToString());
        }
    }
}
