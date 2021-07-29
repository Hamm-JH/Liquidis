using Looxid.Link;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inputs.BrainInterface.LooxidLink
{
	public class DataCollector : Template<DataCollector>
	{
		#region ����
		public bool isDebug;

		[SerializeField] Datas data;
		#region ���� ����

		public struct Datas
		{
			public LinkDataValue leftActivity;
			public LinkDataValue rightActivity;
			public LinkDataValue attention;
			public LinkDataValue relaxation;
		}

		#endregion
		#endregion

		// Start is called before the first frame update
		void Start()
		{
			LooxidLinkManager.Instance.Initialize();        // ��õ帵ũ ������ �ʱ�ȭ

			data.leftActivity = new LinkDataValue();
			data.rightActivity = new LinkDataValue();
			data.attention = new LinkDataValue();
			data.relaxation = new LinkDataValue();
		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}
