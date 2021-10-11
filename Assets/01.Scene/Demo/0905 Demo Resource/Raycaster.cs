using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace Demo
{
	public class Raycaster : MonoBehaviour
	{
		public SteamVR_Input_Sources handType;
		public SteamVR_Behaviour_Pose controllerPose;
		public SteamVR_Action_Boolean trigger;

		//-----

		public Transform targetTransform;

		public GameObject laser;
		public Transform laserTransform;

		public enum scene
        {
			INTRO,
			GUIDE,
			SELECT,
			WAITING,
			MEETING
        }

		public scene currentScene;

		// Start is called before the first frame update
		void Start()
		{
			trigger = Valve.VR.SteamVR_Actions.default_GrabPinch;

			laserTransform = laser.transform;
		}

		bool isSelected = false;

		private void Update()
		{
			RaycastHit hit;

			//if(Physics.Raycast(laserTransform.position, laserTransform.forward, out hit, 100, LayerMask.NameToLayer("Interactable")))
			if (Physics.Raycast(laserTransform.position, laserTransform.forward, out hit, 1000))
			{
				//Debug.Log(LayerMask.LayerToName(hit.transform.gameObject.layer));
				laser.transform.localScale = new Vector3(1, 1, hit.distance);
				laser.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_BaseColor", SetColor(true));

				if (trigger.stateDown)
				{
					targetTransform = hit.transform;
					isSelected = true;

					Collider _coll;
                    if (targetTransform.TryGetComponent<Collider>(out _coll))
                    {
                        _coll.enabled = false;
                    }

                    if (currentScene == scene.INTRO)
                    {
						if(_coll.gameObject.tag == "sphere")
							IntroManager.instance.TriggerTitleAni();
                    }
				}
			}
			else
			{
				laser.transform.localScale = new Vector3(1, 1, 1000);
				laser.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_BaseColor", SetColor(false));
			}

			if (trigger.state && isSelected == true)
			{
				if (hit.transform != null)
				{
					if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "TriggerQuad" && targetTransform != null)
					{
						float xPos = 0f;
						xPos = hit.point.x;

						Debug.Log(LayerMask.LayerToName(hit.transform.gameObject.layer));

						xPos = Mathf.Clamp(xPos, -8.8f, 8.8f);
						targetTransform.position = new Vector3(
							xPos,
							targetTransform.position.y,
							hit.point.z
							);

					}
				}
			}
			else if (trigger.stateUp && isSelected == true)
			{
				isSelected = false;

				Collider _coll;
				if (targetTransform != null && targetTransform.TryGetComponent<Collider>(out _coll))
				{
					_coll.enabled = true;
				}

				targetTransform = null;
			}

			{
				//if(Physics.Raycast(targetTransform.position, targetTransform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
				//{
				//    Debug.DrawRay(targetTransform.position, targetTransform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
				//    Debug.Log("Did hit");
				//}
				//else
				//{
				//    Debug.DrawRay(targetTransform.position, targetTransform.TransformDirection(Vector3.forward) * 1000, Color.white);
				//    Debug.Log("Did not hit");
				//}
			}
		}

		private Color SetColor(bool isHit)
		{
			if (isHit)
			{
				return Color.yellow;
			}
			else
			{
				return Color.black;
			}
		}
	}
}
