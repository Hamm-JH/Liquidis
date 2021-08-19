using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Inputs
{
	/// <summary>
	/// 앱 시작시 초기화할 장비 코드를 정렬한 열거변수
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
		/// 입력 이벤트 정리용 구조체
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

		// 장비코드 정렬변수
		private Dictionary<InputDevice, Device> devices;

		#endregion

		#region Unity Awake

		private void Awake()
		{
			InitDevices(Type, out devices);
		}

		/// <summary>
		/// 초기 장비 타입값에 따라 장비 인스턴스 생성
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
		/// 단일 입력장비 생성, 반환
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

		#region Unity Start (예시용 메서드 구독상태)

		private void Start()
		{
			Events.OnClick.AddListener(DebugOnClick);
			Events.OnDrag.AddListener(DebugOnDrag);
		}

		#endregion

		#region Example getting event methods

		// TODO : debugging code
		// 예시 : 클릭 발생시 실행되는 메서드
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

		// 예시 : 드래그 발생시 실행되는 메서드
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
