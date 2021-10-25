using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetingSequenceMusicQue : MonoBehaviour
{

    public AudioSource meetingAudioSource;
    public AudioSource meetingAudioSource_sub;

    public AudioClip shakebox_meeting;
    public AudioClip face_wakeup;
    public AudioClip face_load;


    // Start is called before the first frame update
    void Start()
    {
        
    }

   public void ShakeBoxPlay()
    {
        meetingAudioSource.clip = shakebox_meeting;
        meetingAudioSource.Play();
    }

    public void FaceWakeUpPlay()
    {
        meetingAudioSource_sub.clip = face_wakeup;
        meetingAudioSource_sub.Play();
    }

    public void FaceLoad()
    {
        meetingAudioSource.clip = face_load;
        meetingAudioSource.Play();
    }
}
