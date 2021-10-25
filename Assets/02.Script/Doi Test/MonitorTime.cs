using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MonitorTime : MonoBehaviour
{

    float currentTime = 0;
    public float limitTime = 600f;

    private static MonitorTime _monitorTime;
 
    public static MonitorTime instance
    {
        get
        {
            if (_monitorTime == null)
                return null;
            else
                return _monitorTime;
        }
    }

    private void Awake()
    {
        if (_monitorTime == null)
            _monitorTime = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void CancelCheck()
    {
        currentTime = 0f;
    }
    // Update is called once per frame
    void Update()
    {
        if (currentTime < limitTime)
        {
            currentTime += Time.deltaTime;

        }
        else
        {
            if (MappingParameter.instance != null)
            {
                Destroy(MappingParameter.instance.gameObject);
            }

            SceneManager.LoadScene("01_Intro");
        }

    }
}
