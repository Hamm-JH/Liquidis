using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Valve.VR;

namespace Inputs
{
	internal sealed class VRController : Device
	{
		#region Def
		[SerializeField] private SteamVR_Action_Boolean menu;
		[SerializeField] private SteamVR_Action_Boolean trigger;
		[SerializeField] private SteamVR_Action_Vector2 trackpad_Pos;
		[SerializeField] private SteamVR_Action_Boolean trackpad_Click;
		[SerializeField] private SteamVR_Action_Boolean grap;
		
		// ���� ���ε�
		private void Start()
		{
			menu = Valve.VR.SteamVR_Actions.default_menu;
			
			// ���� fullPath�� �ۼ��ؼ� ��Ʈ�ѷ� ���ε�
			//trigger = SteamVR_Action_Boolean.FindExistingActionForPartialPath("/actions/default/in/InteractUI") as SteamVR_Action_Boolean;
			// ���õ� json �����Ϳ��� ������ ���������� ��Ʈ�ѷ� ���ε�
			trigger = Valve.VR.SteamVR_Actions.default_InteractUI;

			trackpad_Pos = Valve.VR.SteamVR_Actions.default_TrackpadPos;
			trackpad_Click = Valve.VR.SteamVR_Actions.default_TrackpadClick;
			grap = Valve.VR.SteamVR_Actions.default_GrabGrip;

			_status.OnReset();
		}
		#endregion

		public struct Status
		{
			/// <summary>
			/// Ŭ�����¸� �����ϴ� ����
			/// true : Ŭ�� ����
			/// false : Ŭ������ �ƴ� �Ǵ� ��
			/// </summary>
			public bool onDown;
			/// <summary>
			/// �巡�� �����ߴ��� Ȯ���ϴ� ����
			/// true : �巡�� ���۵Ǿ���, �������� ����
			/// false : �巡�� �������� �ʾҰų�, ����� ����
			/// </summary>
			public bool startDrag;

			/// <summary>
			/// ���� �巡�� ��ġ��
			/// </summary>
			public float3 before;
			public float3 after;

			public Vector2 tempTrackpadDelta;

			public void OnReset()
			{
				onDown = false;
				startDrag = false;

				before = float3.zero;
				after = float3.zero;

				tempTrackpadDelta = default(Vector2);
			}
		}

		private Status _status;

		// Update is called once per frame
		private void Update()
		{
			clickData.Set(type);
			dragData.Set(type);

			if(trigger.stateDown)
			{
				ClickDown(ref _status);
			}
			else if(trigger.state)
			{
				Dragging(ref _status);
			}
			else if(trigger.stateUp)
			{
				ClickUp(ref _status);
			}

			if(trackpad_Pos.changed)
			{
				TempDragging(ref _status);
			}
		}

		private void ClickDown(ref Status _st)
		{
			_st.onDown = true;

			clickData.DebugText = "Click Down";
			callTarget.Events.OnClick.Invoke(clickData);
		}

		private void Dragging(ref Status _st)
		{
			if(_st.onDown == true)
			{
				dragData.DebugText = "Dragging";
				callTarget.Events.OnDrag.Invoke(dragData);
			}
		}

		private void ClickUp(ref Status _st)
		{
			_st.onDown = false;

			clickData.DebugText = "Click Up";
			callTarget.Events.OnClick.Invoke(clickData);
		}

		private void TempDragging(ref Status _st)
		{
			dragData.DebugText = "Dragging";
			dragData.Delta = trackpad_Pos.delta;

			callTarget.Events.OnDrag.Invoke(dragData);

			//Debug.Log($"axis : {trackpad_Pos.axis}");
			//Debug.Log($"delta : {trackpad_Pos.delta}");
			//Debug.Log($"direction : {trackpad_Pos.direction}");
		}
	}
}
