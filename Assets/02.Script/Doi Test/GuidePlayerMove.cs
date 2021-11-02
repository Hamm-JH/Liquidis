using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SpatialTracking;

public class GuidePlayerMove : MonoBehaviour
{

    public float speed = 3f;
    public Animator camera_rig_ani;

    bool done = false;
    public bool headOn = false;
    bool headCheck = false;

    float headOnTime = 0f;
    public Transform camera;

    public enum nextScene
    {
        Select,
        Movie
    }
    public nextScene targetScene;
    public LoopSystem guideSound;

    // Update is called once per frame
    void Update()
    {

        if (!done)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(new Vector3(0f, 0f, 1f) * Time.deltaTime * speed);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(new Vector3(0f, 0f, -1f) * Time.deltaTime * speed);
            }
        }
       

        if(transform.position.z > 62f)
        {
            if (!done)
            {
                MoveAniStart();
                done = true;
                //return;

            }
        }

        if (!headCheck)
        {
            if (headOn)
            {
                headOnTime += Time.deltaTime;
                if (headOnTime > 2f)
                {
                    camera.GetComponent<TrackedPoseDriver>().enabled = true;
                    headCheck = true;

                }

            }
            
        }
       

    }

    void MoveAniStart()
    {
        camera_rig_ani.SetTrigger("SceneEnd");
        StartCoroutine(LoadSelectScene());
    }

    IEnumerator LoadSelectScene()
    {
        yield return new WaitForSeconds(3f);

        if(targetScene == nextScene.Select)
        {
            guideSound.MusicFadeOut();
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("03_Play_Select");

        }
        if(targetScene == nextScene.Movie)
        {
            guideSound.MusicFadeOut();
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("Movie");

        }
    }
}
