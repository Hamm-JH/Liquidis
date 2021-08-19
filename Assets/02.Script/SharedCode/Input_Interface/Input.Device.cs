using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Inputs
{
	/// <summary>
	/// 클릭시 발생 데이터
	/// </summary>
	public struct OnClickData
	{
		private string debugText;

		// def
		private string deviceType;
		private DateTime time;

		public string DebugText { get => debugText; set => debugText=value; }
		public string DeviceType { get => deviceType; set => deviceType=value; }
		public DateTime Time { get => time; set => time=value; }


		public void Set(InputDevice dev)
		{
			if(dev == InputDevice.VRController)
			{
				deviceType = "VRController";
			}
			else
			{
				deviceType = "Mouse";
			}

			time = DateTime.Now;
		}

	}

	/// <summary>
	/// 드래그시 발생 데이터
	/// </summary>
	public struct OnDragData
	{
		private string debugText;

		// def
		private string deviceType;
		private DateTime time;

		public string DebugText { get => debugText; set => debugText=value; }
		public string DeviceType { get => deviceType; set => deviceType=value; }
		public DateTime Time { get => time; set => time=value; }

		public void Set(InputDevice dev)
		{
			if(dev == InputDevice.VRController)
			{
				deviceType = "VRController";
			}
			else
			{
				deviceType = "Mouse";
			}

			time = DateTime.Now;
		}
	}

	public abstract class Device : MonoBehaviour
	{
		protected InputInterface callTarget;

		protected OnClickData clickData;
		protected OnDragData dragData;

		protected InputDevice type;

		private void Awake()
		{
			if(this.GetType() == typeof(VRController))
			{
				type = InputDevice.VRController;
			}
			else
			{
				type = InputDevice.Mouse;
			}
		}

		/// <summary>
		/// 초기화 : 이벤트 발생 타이밍 전달변수 초기화 메서드
		/// </summary>
		/// <param name="resource"></param>
		public void SubscribeInputInterface(InputInterface resource)
		{
			callTarget = resource;

			clickData = new OnClickData();
			dragData = new OnDragData();
		}
	}
}
