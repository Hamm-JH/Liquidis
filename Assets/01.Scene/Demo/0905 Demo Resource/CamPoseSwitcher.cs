using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;
using Valve.VR;

namespace Demo
{
	public class CamPoseSwitcher : MonoBehaviour
	{
		/// <summary>
		/// VR 카메라 헤드 연동 코드
		/// </summary>
		public TrackedPoseDriver poseDriver;

		public SteamVR_Input_Sources handType;
		public SteamVR_Behaviour_Pose controllerPose;

		public SteamVR_Action_Boolean menuBtn;

		// Start is called before the first frame update
		void Start()
		{
			menuBtn = Valve.VR.SteamVR_Actions.default_menu;
		}

		// Update is called once per frame
		void Update()
		{
			if (menuBtn.stateDown)
			{
				poseDriver.enabled = !poseDriver.enabled;
			}
		}
	}
}
