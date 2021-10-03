using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CylinderAni : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DistinguishScenes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void DistinguishScenes()
    {
        Scene scene = SceneManager.GetActiveScene();
        if(scene.name == "02_Play_Guide")
        {
            gameObject.GetComponent<Animator>().SetTrigger("GuideStart");
        }
        if (scene.name == "03_Play_Select")
        {

            gameObject.GetComponent<Animator>().SetTrigger("SelectStart");
        }
        if (scene.name == "04_Play_WaitingRoom")
        {
            //
        }
    }
}
