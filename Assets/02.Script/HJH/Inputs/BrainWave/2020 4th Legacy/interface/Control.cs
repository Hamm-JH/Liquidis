using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Control : MonoBehaviour
{
    #region Singleton

    private static Control _instance;

    public static Control Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(Control)) as Control;
                if (_instance == null)
                {
                    Debug.Log("Control 관리 인스턴스가 현재 Hierarchy에 없습니다.");
                }
            }
            return _instance;
        }
    }

    #endregion

    #region 변수

    private Data.CollectionStatus collectionStatus;

    private int collectionBoundary;

    public Data.CollectionStatus CollectionStatus
    {
        get => collectionStatus;
        set
        {
            collectionStatus = value;
            Manager.DataManager.Instance.CollectionStatus = value;
        }
    }

    public int CollectionBoundary
    {
        get => collectionBoundary;
        set
        {
            collectionBoundary = value;
            Manager.DataManager.Instance.CollectionBoundary = value;
        }
    }

    #endregion

    #region Event

    public delegate double getMindIndex(Data.Index dataIndex);

    protected delegate double callMindIndex(Data.MindIndex mindIndex);
    protected delegate double callMindIndexWithStatus(Data.MindIndex mindIndex, Data.CollectionStatus collectionStatus);

    protected delegate double callTimestampMindIndex(Data.MindIndex mindIndex, float timeStamp);
    protected delegate double callTimestampMindIndexWithStatus(Data.MindIndex mindIndex, float timeStamp, Data.CollectionStatus collectionStatus);

    public getMindIndex GetMindIndex;

    protected callMindIndex CallMindIndex;
    protected callMindIndexWithStatus CallMindIndexWithStatus;

    // TODO lst : 작업 요망
    protected callTimestampMindIndex CallTimestampMindIndex;
    protected callTimestampMindIndexWithStatus CallTimestampMindIndexWithStatus;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //if (Manager.DataManager.Instance.)
        CollectionStatus = Data.CollectionStatus.Await;
        CollectionBoundary = 150;

        GetMindIndex = ReceiveMindIndex;

        CallMindIndex = Manager.DataManager.Instance.GetMindIndex;
        CallMindIndexWithStatus = Manager.DataManager.Instance.GetMindIndex;

        CallTimestampMindIndex = Manager.DataManager.Instance.GetMindIndex;
        CallTimestampMindIndexWithStatus = Manager.DataManager.Instance.GetMindIndex;

    }

    public double ReceiveMindIndex(Data.Index dataIndex)
    {
        double result = 0;

        // CollectionStatus를 받지 않은 상태인 경우
        if(dataIndex.collectionStatus == Data.CollectionStatus.Null)
        {
            // timeStamp가 할당된 경우
            if(dataIndex.timeStamp != null)
            {
                result = CallTimestampMindIndex.Invoke(dataIndex.mindIndex, (float)dataIndex.timeStamp);
            }
            // timeStamp가 할당되지 않은 경우
            else
            {
                result = CallMindIndex.Invoke(dataIndex.mindIndex);
            }
        }
        // CollectionStatus를 받은 상태인 경우
        else
        {
            // timeStamp가 할당된 경우
            if (dataIndex.timeStamp != null)
            {
                result = CallTimestampMindIndexWithStatus.Invoke(dataIndex.mindIndex, (float)dataIndex.timeStamp, dataIndex.collectionStatus);
            }
            // timeStamp가 할당되지 않은 경우
            else
            {
                result = CallMindIndexWithStatus.Invoke(dataIndex.mindIndex, dataIndex.collectionStatus);
            }
        }

        return result;
    }
}
