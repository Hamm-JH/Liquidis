using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AniManager_Guide : MonoBehaviour
{
    public Animator guideAni;
    public Animator camera_rig;

    bool cameraAniStart = false;
    float currentTime = 0f;
    float targetTime = 3f;
    float zTarget = 62f;


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

    private void Update()
    {
        if (!cameraAniStart)
        {
            if(camera_rig.gameObject.transform.position.z >= zTarget)
            {
                currentTime += Time.deltaTime;

                if(currentTime > targetTime)
                {
                    camera_rig.SetTrigger("SceneEnd");
                    cameraAniStart = true;
                }
            }
            else
            {
                currentTime = 0f;
            }
        }
    }
}
