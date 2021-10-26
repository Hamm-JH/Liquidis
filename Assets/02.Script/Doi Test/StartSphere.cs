using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;


public class StartSphere : MonoBehaviour
{
    float speed = 1f;
    public BallAniTest_Duru ball_ani;

    public float playXpos_debug = 8.6f;
    public float movieXpos_debug = -8.6f;

    public float playXpos_vr = 8.6f;
    public float movieXpos_vr = -8.6f;


    float playXpos = 8.6f;
    float movieXpos = -8.6f;

    public bool firstEnter = false;
    private float enterTime = 0f;
    private float limitTime = 3f;

    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Vibration vibration;

    bool isExecuted = false;


    public enum mode
    {
        debug,
        VR
    }
    public mode currentMode;

    private void Start()
    {
        if(currentMode == mode.debug)
        {
            playXpos = playXpos_debug;
            movieXpos = movieXpos_debug;

        }
        else
        {
            playXpos = playXpos_vr;
            movieXpos = movieXpos_vr;
        }

        vibration = Valve.VR.SteamVR_Actions.default_Haptic;

    }

    public void SetSphereSpeed(float _speed)
    {
        speed = _speed;

    }

    private void OnMouseDown()
    {
        ball_ani.TitleAni();
    }
    private void OnMouseDrag()
    {
        MoveSphere();
    }

    public void MoveSphere()
    {
        float xPos = Input.mousePosition.x / Screen.width;

        //Debug.Log(xPos);
        xPos = xPos * 17f;
        xPos -= 8.5f;

        xPos = Mathf.Clamp(xPos, movieXpos, playXpos);

        transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
    }

    private void Update()
    {
        if(transform.position.x >= playXpos || transform.position.x <= movieXpos)
        {
            if (enterTime < limitTime)
            {
                enterTime += Time.deltaTime;
                // 공의 움직임 속도를 늦춘다.####### called by frame 주의 ########
                SetSphereSpeed(0.5f);
                Debug.Log("cur Time : " + enterTime);

                // haptic

                vibration.Execute(
                        secondsFromNow: 0,
                        durationSeconds: Time.deltaTime,
                        frequency: 0.1f,
                        amplitude: 0.3f,
                        inputSource: handType);

                if (isExecuted == false)
                {
                    isExecuted = true;
                    Debug.Log("haptic executed");
                    
                }
            }
            else
            {
                if (transform.position.x >= playXpos)
                {
                    IntroManager.instance.introSound.MusicFadeOut();
                    StartCoroutine(LoadSelectGuide());

                }
                else if(transform.position.x <= movieXpos)
                {
                    IntroManager.instance.introSound.MusicFadeOut();
                    StartCoroutine(LoadMovieGuide());

                }


            }

        }
        else
        {
            enterTime = 0f;
            isExecuted = false;

        }


       

    }
    IEnumerator LoadSelectGuide()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("02_Play_Guide");

    }
    IEnumerator LoadMovieGuide()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("02_Movie_Guide");

    }
}
