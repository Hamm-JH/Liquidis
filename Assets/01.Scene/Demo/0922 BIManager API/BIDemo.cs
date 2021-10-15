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
		#region 1. EEG 데이터 요청
		// 데이터 요청시 api를 생성한다.
		// 뇌파 데이터 api 생성시 필수 할당 데이터
		// 1. targetId : 룩시드랩스 6개 센서중의 한 개의 센서데이터
		// 2. targetSecond : 메서드 실행 시점에서 과거 n초까지의 데이터 수집
		// 3. 데이터 수집 완료시 실행할 이벤트
		API.Brainwave api_EEG = new API.Brainwave(
            obj: API.Objective.EEG,
            targetId: Looxid.Link.EEGSensorID.AF3,
            targetSecond: 10,
            targetCallBack: biGetter
            );

        //Request(api_EEG);
        #endregion

        #region 2. 안정상태 데이터 요청

        API.Brainwave api_relaxation = new API.Brainwave(
            obj: API.Objective.Relaxation,
            targetCallBack: biGetter
            );

		//Request(api_relaxation);
		#endregion

		#region 3. 집중상태 데이터 요청

		API.Brainwave api_attention = new API.Brainwave(
            obj: API.Objective.Attention,
            targetCallBack: biGetter
            );

		//Request(api_attention);
        #endregion

        #region 4. 디버깅용 랜덤 EEG

        API.Brainwave api_randomEEG = new API.Brainwave(
            obj: API.Objective.EEGRandom,
            targetCallBack: biGetter
            );

        Request(api_randomEEG);
        #endregion

        #region 5. 디버깅용 이완 랜덤 마인드값

        API.Brainwave api_randomMindRelaxation = new API.Brainwave(
            obj: API.Objective.MindRandom,
            option: 0,
            targetCallBack: biGetter
            );

        Request(api_randomMindRelaxation);
        #endregion

        #region 6. 디버깅용 집중 랜덤 마인드값

        API.Brainwave api_randomMindAttention = new API.Brainwave(
            obj: API.Objective.MindRandom,
            option: 1,
            targetCallBack: biGetter
            );

        Request(api_randomMindAttention);

        #endregion

        #region 11. Concentration 집중
        API.Brainwave api1 = new API.Brainwave(
            obj: API.Objective.Concentration,
            targetCallBack: biGetter
            );

        Request(api1);
        #endregion

        #region 12. Excitement 흥분
        API.Brainwave api2 = new API.Brainwave(
            obj: API.Objective.Excitement,
            targetCallBack: biGetter
            );

        Request(api2);
        #endregion

        #region 13. Positiveness 긍부정
        API.Brainwave api3 = new API.Brainwave(
            obj: API.Objective.Positiveness,
            targetCallBack: biGetter
            );

        Request(api3);
        #endregion

        #region 14. Empathy 공감
        API.Brainwave api4 = new API.Brainwave(
            obj: API.Objective.Empathy,
            targetCallBack: biGetter
            );

        Request(api4);
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
        else if (api.Objective == API.Objective.Concentration)
        {
            //Debug.Log($"Concentration : {api..ToString()}");
        }
        else if (api.Objective == API.Objective.Excitement)
        {
            //Debug.Log($"Excitement : {api..ToString()}");
        }
        else if (api.Objective == API.Objective.Positiveness)
        {
            //Debug.Log($"Positiveness : {api..ToString()}");
        }
        else if (api.Objective == API.Objective.Empathy)
        {
            //Debug.Log($"Empathy : {api..ToString()}");
        }
        else if(api.Objective == API.Objective.EEGRandom)
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
        else if(api.Objective == API.Objective.MindRandom)
		{
            if(api.Option == 0)
			{
                Debug.Log($"Relaxation: {api.Relaxation.ToString()}");
			}
            else if(api.Option == 1)
			{
                Debug.Log($"Attention : {api.Attention.ToString()}");
            }
		}
    }
}
