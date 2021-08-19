using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inputs
{
	internal sealed class VRController : Device
	{
		// Update is called once per frame
		private void Update()
		{
			clickData.Set(type);
			dragData.Set(type);
		}
	}
}
