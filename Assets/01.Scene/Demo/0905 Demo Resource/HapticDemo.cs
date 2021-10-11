using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace Demo
{
	public class HapticDemo : MonoBehaviour
	{
		public SteamVR_Input_Sources handType;
		public SteamVR_Behaviour_Pose controllerPose;
		public SteamVR_Action_Boolean trigger;
		public SteamVR_Action_Vibration vibration;

		// Start is called before the first frame update
		void Start()
		{
			trigger = Valve.VR.SteamVR_Actions.default_GrabPinch;
			vibration = Valve.VR.SteamVR_Actions.default_Haptic;
		}

		bool isExecuted = false;

		// Update is called once per frame
		void Update()
		{
			if(trigger.state)
			{
				if(isExecuted == false)
				{
					isExecuted = true;
					Debug.Log("haptic executed");
					vibration.Execute(
						secondsFromNow: 0,
						durationSeconds: Time.deltaTime,
						frequency: 50f,
						amplitude: 0.1f,
						inputSource: handType);
				}
			}
			else if(trigger.stateUp)
			{
				isExecuted = false;
			}
		}
	}
}
