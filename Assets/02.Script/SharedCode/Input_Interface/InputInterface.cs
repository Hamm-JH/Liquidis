using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Inputs
{
	/// <summary>
	/// �� ���۽� �ʱ�ȭ�� ��� �ڵ带 ������ ���ź���
	/// </summary>
	public enum InputDevice
	{
		ERROR = -1,
		Any = 0,
		Mouse = 1,
		VRController = 2
	}

	public sealed class InputInterface : MonoBehaviour
	{
		#region Internal Definition Area

		#region Singleton

		private static InputInterface instance;

		public static InputInterface Instance
		{
			get
			{
				if(instance == null)
				{
					instance = FindObjectOfType<InputInterface>() as InputInterface;
					if(instance == null)
					{
						EventSystem es = FindObjectOfType<EventSystem>() as EventSystem;
						if(es == null)
						{
							es = new GameObject("EventSystem").AddComponent<EventSystem>();
							es.gameObject.AddComponent<StandaloneInputModule>().enabled = true;
							es.enabled = true;

							es.gameObject.AddComponent<InputInterface>();
						}

						instance = es.gameObject.GetComponent<InputInterface>();
					}
				}
				return instance;
			}
		}

		#endregion

		#region Def Area

		/// <summary>
		/// �Է� �̺�Ʈ ������ ����ü
		/// </summary>
		[System.Serializable]
		public struct InputEvents
		{
			public UnityEvent<OnClickData> OnClick;
			public UnityEvent<OnDragData> OnDrag;
		}
		#endregion

		#region public

		[Header("Device Type")]
		public InputDevice Type;

		[Header("Events")]
		public InputEvents Events;

		#endregion

		#region private

		// ����ڵ� ���ĺ���
		private Dictionary<InputDevice, Device> devices;

		#endregion

		#region Unity Awake

		private void Awake()
		{
			InitDevices(Type, out devices);
		}

		/// <summary>
		/// �ʱ� ��� Ÿ�԰��� ���� ��� �ν��Ͻ� ����
		/// </summary>
		private void InitDevices(
			InputDevice param, 
			out Dictionary<InputDevice, Device> target)
		{
			target = new Dictionary<InputDevice, Device>();

			switch(param)
			{
				case InputDevice.Any:
					{
						target.Add(InputDevice.Mouse, InitSingleDevice(InputDevice.Mouse, "Mouse"));
						target.Add(InputDevice.VRController, InitSingleDevice(InputDevice.VRController, "VRController"));
					}
					break;

				case InputDevice.Mouse:
					{
						target.Add(InputDevice.Mouse, InitSingleDevice(InputDevice.Mouse, "Mouse"));
					}
					break;

				case InputDevice.VRController:
					{
						target.Add(InputDevice.VRController, InitSingleDevice(InputDevice.VRController, "VRController"));
					}
					break;
			}
		}

		/// <summary>
		/// ���� �Է���� ����, ��ȯ
		/// </summary>
		private Device InitSingleDevice(InputDevice param, string objName)
		{
			Device result;

			GameObject obj = new GameObject(objName);
			if(param == InputDevice.Mouse)
			{
				result = obj.AddComponent<Mouse>();
			}
			else
			{
				result = obj.AddComponent<VRController>();
			}
			result.SubscribeInputInterface(this);
			obj.transform.SetParent(gameObject.transform);

			return result;
		}

		#endregion

		#endregion

		#region Unity Start (���ÿ� �޼��� ��������)

		private void Start()
		{
			Events.OnClick.AddListener(DebugOnClick);
			Events.OnDrag.AddListener(DebugOnDrag);
		}

		#endregion

		#region Example getting event methods

		// TODO : debugging code
		// ���� : Ŭ�� �߻��� ����Ǵ� �޼���
		private void DebugOnClick(OnClickData data)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("------ Debugging OnClick Start ------");
			sb.AppendLine($"OnClick : {data.DebugText}");
			sb.AppendLine($"OnClick device type : {data.DeviceType}");
			sb.AppendLine($"OnClick time : {data.Time}");

			sb.AppendLine("------ Debugging OnClick End   ------");
			Debug.LogWarning(sb.ToString());
			sb = null;
		}

		// ���� : �巡�� �߻��� ����Ǵ� �޼���
		private void DebugOnDrag(OnDragData data)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("------ Debugging OnDrag Start ------");
			sb.AppendLine($"OnDrag : {data.DebugText}");
			sb.AppendLine($"OnDrag device type : {data.DeviceType}");
			sb.AppendLine($"OnDrag time : {data.Time}");

			sb.AppendLine("------ Debugging OnDrag End   ------");
			Debug.LogWarning(sb.ToString());
			sb = null;
		}

		#endregion
	}
}
