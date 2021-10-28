using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(MeetingRoom.instance != null)
            {
                MeetingRoom.instance.voiceControl.DestroyPlayer();
                Destroy(MeetingRoom.instance.voiceControl.gameObject);
            }


            if(MappingParameter.instance != null)
            {
                
                Destroy(MappingParameter.instance.gameObject);
            }


            if(Manager.BIManager.Instance != null)
            {
                Destroy(Manager.BIManager.Instance.gameObject);
            }
            SceneManager.LoadScene("01_Intro");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
