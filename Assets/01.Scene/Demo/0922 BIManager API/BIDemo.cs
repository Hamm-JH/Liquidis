using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class BIDemo : MonoBehaviour
{
    /// <summary>
    /// BIManager에서 데이터 수집 완료시 데이터를 보낼 이벤트
    /// </summary>
    public UnityAction<API.Brainwave> biGetter;

    // Start is called before the first frame update
    void Start()
    {
        // 이벤트에 메서드 연결
        biGetter += Receive;
    }

	private void Update()
	{
		#region EEG 데이터 요청
		// 데이터 요청시 api를 생성한다.
		// 뇌파 데이터 api 생성시 필수 할당 데이터
		// 1. targetId : 룩시드랩스 6개 센서중의 한 개의 센서데이터
		// 2. targetSecond : 메서드 실행 시점에서 과거 n초까지의 데이터 수집
		// 3. 데이터 수집 완료시 실행할 이벤트
		API.Brainwave api = new API.Brainwave(
            obj: API.Objective.EEG,
            targetId: Looxid.Link.EEGSensorID.AF3,
            targetSecond: 10,
            targetCallBack: biGetter
            );

        //Request(api);
        #endregion

        #region 안정상태 데이터 요청

        API.Brainwave api_relaxation = new API.Brainwave(
            obj: API.Objective.Relaxation,
            targetCallBack: biGetter
            );

		Request(api_relaxation);
		#endregion

		#region 집중상태 데이터 요청

		API.Brainwave api_attention = new API.Brainwave(
            obj: API.Objective.Attention,
            targetCallBack: biGetter
            );

		//Request(api_attention);
		#endregion
	}

	/// <summary>
	/// 뇌파 데이터 관리자(BIManager)로 데이터 수집 요청을 한다.
	/// </summary>
	/// <param name="api"></param>
	private void Request(API.Brainwave api)
	{
        Manager.BIManager.Instance.Request(api);
	}

    /// <summary>
    /// 이벤트에 연결된 데이터 수집 메서드
    /// 수집 완료된 데이터를 사용한다.
    /// </summary>
    /// <param name="api"></param>
    private void Receive(API.Brainwave api)
	{
        if(api.Objective == API.Objective.EEG)
		{
            string str = "";
            str += $"Sensor ID : {api.Id.ToString()}\n";
            str += $"interval seconds : {api.Second.ToString()}\n";

            str += $"Delta : {api.Delta}\n";
            str += $"Theta : {api.Theta}\n";
            str += $"Alpha : {api.Alpha}\n";
            str += $"Beta : {api.Beta} \n";
            str += $"Gamma : {api.Gamma}\n";

            Debug.Log(str);
		}
        else if(api.Objective == API.Objective.Relaxation)
		{
            Debug.Log($"Relaxation : {api.Relaxation.ToString()}");
		}
        else if(api.Objective == API.Objective.Attention)
		{
            Debug.Log($"Attention : {api.Attention.ToString()}");
		}
    }
}
