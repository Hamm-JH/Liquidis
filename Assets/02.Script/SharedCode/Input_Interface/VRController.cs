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
		
		// 변수 바인딩
		private void Start()
		{
			menu = Valve.VR.SteamVR_Actions.default_menu;
			
			// 직접 fullPath를 작성해서 컨트롤러 바인딩
			//trigger = SteamVR_Action_Boolean.FindExistingActionForPartialPath("/actions/default/in/InteractUI") as SteamVR_Action_Boolean;
			// 세팅된 json 데이터에서 정렬한 변수값으로 컨트롤러 바인딩
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
			/// 클릭상태를 정의하는 변수
			/// true : 클릭 시작
			/// false : 클릭중이 아님 또는 끝
			/// </summary>
			public bool onDown;
			/// <summary>
			/// 드래그 시작했는지 확인하는 변수
			/// true : 드래그 시작되었고, 진행중인 상태
			/// false : 드래그 시작하지 않았거나, 종료된 상태
			/// </summary>
			public bool startDrag;

			/// <summary>
			/// 이전 드래그 위치값
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
