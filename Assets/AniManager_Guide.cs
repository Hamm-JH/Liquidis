using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AniManager_Guide : MonoBehaviour
{
    public Animator guideAni;
    public Animator camera_rig;
    // Start is called before the first frame update
    //void Awake()
    //{
    //    StartAni();

    //}

    public void StartAni()
    {
        guideAni.SetTrigger("start");
        camera_rig.SetTrigger("SceneStart");
    }    
   
}
