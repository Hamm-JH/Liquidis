using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneMenu_Duru : MonoBehaviour
{
    public GameObject selectPanel;
    public GameObject waitingPanel;
    public GameObject meetingPanel;
    public GameObject moviePanel;

    // Start is called before the first frame update
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "03_Play_Select")
        {
            selectPanel.SetActive(true);

        }
        else if (scene.name == "04_Play_WaitingRoom")
        {
            waitingPanel.SetActive(true);
        }
        else if (scene.name == "MeetingRoom")
        {
            meetingPanel.SetActive(true);
        }
        else if (scene.name == "Movie")
        {
            moviePanel.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
