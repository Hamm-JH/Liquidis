using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inputs
{
	internal sealed class Mouse : Device
	{
		private void Update()
		{
			clickData.Set(type);
			dragData.Set(type);

			if (Input.GetMouseButtonDown(0))
			{
				clickData.DebugText = "Click Down";
				callTarget.Events.OnClick.Invoke(clickData);
			}
			else if(Input.GetMouseButton(0))
			{
				dragData.DebugText = "Dragging";
				callTarget.Events.OnDrag.Invoke(dragData);
			}
			else if(Input.GetMouseButtonUp(0))
			{
				clickData.DebugText = "Click Up";
				callTarget.Events.OnClick.Invoke(clickData);
			}
		}
	}
}
