using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Manager
{
	public class VRManager : MonoBehaviour
	{
		#region instance

		private static VRManager instance;

		public static VRManager Instance
		{
			get
			{
				if(instance == null)
				{
					instance = FindObjectOfType<VRManager>() as VRManager;
				}
				return instance;
			}
		}

		#endregion

		public UnityAction<bool> isHeadEquiped;
		public GuidePlayerMove guidePlayerMove;

		// Start is called before the first frame update
		void Start()
		{
			isHeadEquiped += CheckHeadsetEquiped;
		}

		public void CheckHeadsetEquiped(bool isEquiped)
		{
			Debug.Log($"Headset equip state : {isEquiped}");
			guidePlayerMove.headOn = isEquiped;
		}
	}
}
