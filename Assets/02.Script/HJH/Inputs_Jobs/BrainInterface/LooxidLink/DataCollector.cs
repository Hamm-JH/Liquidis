using Looxid.Link;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inputs.BrainInterface.LooxidLink
{
	public class DataCollector : Template<DataCollector>
	{
		#region 변수
		public bool isDebug;

		[SerializeField] Datas data;
		#region 변수 정의

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
			LooxidLinkManager.Instance.Initialize();        // 룩시드링크 관리자 초기화

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
