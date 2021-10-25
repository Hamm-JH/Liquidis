using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class LoopSystem : MonoBehaviour
{
    
    public AudioClip targetClip;
    //public targetClipState currentClip;
   

    [SerializeField]
    private AudioSource sourceA;
    [SerializeField]
    private AudioSource sourceB;


    [SerializeField]
    private AudioMixerGroup groupA;

    [SerializeField]
    private AudioMixerGroup groupB;

    float curTime = 0f;
    private float duration = 0f;
    private bool startPlay = false;

    public enum loopState
    {
        loopA,
        loopB
    }
   

    public loopState currentState;

    private void Start()
    {
        duration = targetClip.length;

        currentState = loopState.loopA;

        sourceA.clip = targetClip;
        sourceB.clip = targetClip;

        
    }


    private void Update()
    {
        if (startPlay)
        {
            
            if (curTime < duration - 1f)
            {
                curTime += Time.deltaTime;
            
            }
            else
            {
                if (currentState == loopState.loopA)
                    currentState = loopState.loopB;
                else if (currentState == loopState.loopB)
                    currentState = loopState.loopA;


                PlayLoop(currentState);

                curTime = 0;
            }
        }



    }

    void PlayLoop(loopState state)
    {
        switch (state)
        {
            case loopState.loopA:
                StartCoroutine("PauseLoop", sourceB);
                sourceA.clip = targetClip;
                sourceA.outputAudioMixerGroup = groupA;
                Debug.Log("play loop A");
                sourceA.Play();
                break;
            case loopState.loopB:
                StartCoroutine("PauseLoop", sourceA);
                sourceB.clip = targetClip;
                sourceB.outputAudioMixerGroup = groupB;
                Debug.Log("play loop B");

                sourceB.Play();

                break;

        }


    }

    IEnumerator PauseLoop(AudioSource target)
    {
        yield return new WaitForSeconds(1f);
        target.Pause();
       
    }

    IEnumerator StopLoop(AudioSource target)
    {
        yield return new WaitForSeconds(1f);
        target.Pause();
        startPlay = false;
        curTime = 0f;
    }
    public void StopLooping()
    {
        switch (currentState)
        {
            case loopState.loopA:
                StartCoroutine("StopLoop", sourceA);
                break;
            case loopState.loopB:
                StartCoroutine("StopLoop", sourceB);
                break;

        }

    }
    public void StartLooping()
    {
        PlayLoop(currentState);
        startPlay = true;
    }
    void ChangeState()
    {
        if (currentState == loopState.loopA)
            currentState = loopState.loopB;
        else if (currentState == loopState.loopB)
            currentState = loopState.loopA;

    }

    //public void ChangeClip(targetClipState _target, AudioClip _targetClip)
    //{
    //    switch (_target)
    //    {
    //        case targetClipState.HEALTHY_A:
    //            currentClip = targetClipState.HEALTHY_A;
    //            duration = HEALTHY_A;
    //            break;
           
    //    }

    //    targetClip = _targetClip;
    //    ChangeState();
    //    PlayLoop(currentState);
    //    curTime = 0;
    //}


}
