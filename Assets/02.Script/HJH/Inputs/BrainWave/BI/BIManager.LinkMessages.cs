using Looxid.Link;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
	public partial class BIManager : MonoBehaviour
	{
        /// <summary>
        /// Core 앱과 연결 완료시 실행
        /// </summary>
        void OnLinkCoreConncetd()
        {
            Debug.Log("Core connected");
        }

        /// <summary>
        /// Hub와 연결 완료시 실행
        /// </summary>
        void OnLinkHubConnected()
        {
            Debug.Log("Hub connected");
        }

        /// <summary>
        /// Core와 연결 끊기면 실행
        /// </summary>
        void OnLinkCoreDisconncetd()
        {
            Debug.Log("Core disconnected");
        }

        /// <summary>
        /// Hub와 연결 끊기면 실행
        /// </summary>
        void OnLinkHubDisconnected()
        {
            Debug.Log("Hub disconnected");
        }

        /// <summary>
        /// Sensor 메시지가 보여야 할때 실행
        /// </summary>
        void OnShowSensorOffMessage()
        {
            Debug.Log("Sensor off");
        }

        /// <summary>
        /// Sensor 메시지가 숨어야 할때 실행
        /// </summary>
        void OnHideSensorOffMessage()
        {
            Debug.Log("Sensor on");
        }

        /// <summary>
        /// Noise 감지시 실행
        /// </summary>
        void OnShowNoiseSignalMessage()
        {
            Debug.Log("noise detected");
        }

        /// <summary>
        /// Noise 감지 패널 꺼야할때 실행
        /// </summary>
        void OnHideNoiseSignalMessage()
        {
            Debug.Log("noise not detected");
        }
    }
}
