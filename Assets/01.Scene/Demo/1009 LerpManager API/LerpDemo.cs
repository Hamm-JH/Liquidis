using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

namespace Demo
{
	public class LerpDemo : MonoBehaviour
	{
		[Header("���ÿ� ��ü 1��")]
		public Transform demoSphere;

		/// <summary>
		/// ���� �̺�Ʈ ����
		/// </summary>
		UnityAction<API.Lerp> lerpGetter;

		// Start is called before the first frame update
		void Start()
		{
			lerpGetter += Receive;
		}

		// Update is called once per frame
		void Update()
		{
			//Debug.Log(Random.value);
			//Debug.Log(Random.Range(0, 1));

			#region 1. demo ��ҿ� ���� �¿� ���� ����

			API.Lerp lerp_sphereLeftRight = new API.Lerp(
				_requestIndex: 0,
				_Function: API.Lerp.Function.Log,
				_interval: 1f,
				_currValue: demoSphere.position.x,
				_targetValue: (Random.value * 2) - 1,
				_callback: lerpGetter
				);

			Request(lerp_sphereLeftRight);

			#endregion

			#region 2. demo ��ҿ� ���� ���� ���� ����

			API.Lerp lerp_sphereUpDown = new API.Lerp(
				_requestIndex: 1,
				_Function: API.Lerp.Function.Log,
				_interval: 1f,
				_currValue: demoSphere.position.y,
				_targetValue: (Random.value * 2) - 1,
				_callback: lerpGetter
				);

			Request(lerp_sphereUpDown);

			#endregion

			#region 3. demo ��ҿ� ���� �յ� ���� ����

			API.Lerp lerp_sphereFrontBack = new API.Lerp(
				_requestIndex: 2,
				_Function: API.Lerp.Function.Log,
				_interval: 0.5f,
				_currValue: demoSphere.position.z,
				_targetValue: (Random.value * 2) - 1,
				_callback: lerpGetter
				);

			Request(lerp_sphereFrontBack);

			#endregion
		}

		public void Request(API.Lerp api)
		{
			Manager.LerpManager.Instance.Request(api);
		}

		public void Receive(API.Lerp api)
		{
			Transform demoTransform = demoSphere;
			Vector3 demo_spherePosition = demoTransform.position;
			float x = 0;
			float y = 0;
			float z = 0;

			// ��û �ε����� int ���̱⸸ �ϸ� ���Ѿ��� ��� �����մϴ�. (���ÿ����� 0, 1, 2�� �־��)
			if(api.RequestIndex == 0)
			{
				x = api.Value;
				y = demo_spherePosition.y;
				z = demo_spherePosition.z;
			}
			else if(api.RequestIndex == 1)
			{
				x = demo_spherePosition.x;
				y = api.Value;
				z = demo_spherePosition.z;
			}
			else if(api.RequestIndex == 2)
			{
				x = demo_spherePosition.x;
				y = demo_spherePosition.y;
				z = api.Value;
			}

			demoTransform.position = new Vector3(x, y, z);
		}
	}
}
