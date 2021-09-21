using Looxid.Link;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
	public partial class BIManager : MonoBehaviour
	{
        /// <summary>
        /// Core �۰� ���� �Ϸ�� ����
        /// </summary>
        void OnLinkCoreConncetd()
        {
            Debug.Log("Core connected");
        }

        /// <summary>
        /// Hub�� ���� �Ϸ�� ����
        /// </summary>
        void OnLinkHubConnected()
        {
            Debug.Log("Hub connected");
        }

        /// <summary>
        /// Core�� ���� ����� ����
        /// </summary>
        void OnLinkCoreDisconncetd()
        {
            Debug.Log("Core disconnected");
        }

        /// <summary>
        /// Hub�� ���� ����� ����
        /// </summary>
        void OnLinkHubDisconnected()
        {
            Debug.Log("Hub disconnected");
        }

        /// <summary>
        /// Sensor �޽����� ������ �Ҷ� ����
        /// </summary>
        void OnShowSensorOffMessage()
        {
            Debug.Log("Sensor off");
            isSensorConnected = false;
        }

        /// <summary>
        /// Sensor �޽����� ����� �Ҷ� ����
        /// </summary>
        void OnHideSensorOffMessage()
        {
            Debug.Log("Sensor on");
            isSensorConnected = true;
        }

        /// <summary>
        /// Noise ������ ����
        /// </summary>
        void OnShowNoiseSignalMessage()
        {
            Debug.Log("noise detected");
            isSensorNoised = true;
        }

        /// <summary>
        /// Noise ���� �г� �����Ҷ� ����
        /// </summary>
        void OnHideNoiseSignalMessage()
        {
            Debug.Log("noise not detected");
            isSensorNoised = false;
        }
    }
}
