using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Inputs
{
	/// <summary>
	/// Ŭ���� �߻� ������
	/// </summary>
	public struct OnClickData
	{
		private string debugText;

		// def
		private InputDevice deviceType;
		private DateTime time;

		public string DebugText { get => debugText; set => debugText=value; }
		public InputDevice DeviceType { get => deviceType; set => deviceType=value; }
		public DateTime Time { get => time; set => time=value; }


		public void Set(InputDevice dev)
		{
			deviceType = dev;

			time = DateTime.Now;
		}

	}

	/// <summary>
	/// �巡�׽� �߻� ������
	/// </summary>
	public struct OnDragData
	{
		private string debugText;
		private Vector2 delta;

		public string DebugText { get => debugText; set => debugText=value; }
		public Vector2 Delta { get => delta; set => delta=value; }

		// -----

		// def
		private InputDevice deviceType;
		private DateTime time;

		public InputDevice DeviceType { get => deviceType; set => deviceType=value; }
		public DateTime Time { get => time; set => time=value; }

		public void Set(InputDevice dev)
		{
			deviceType = dev;

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
		/// �ʱ�ȭ : �̺�Ʈ �߻� Ÿ�̹� ���޺��� �ʱ�ȭ �޼���
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
