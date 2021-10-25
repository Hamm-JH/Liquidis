using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingSequneceMusicQue : MonoBehaviour
{

    public AudioClip shake;
    public AudioClip shrinkBlack;
    public AudioClip[] light;
    public AudioClip face;

    int lightCount = 0;

    public AudioSource waitingSequenceSource;
    public AudioSource waitingSequenceSource_sub;


    // Start is called before the first frame update
    void Start()
    {
        
    }

   
    public void ShakePlay()
    {
        waitingSequenceSource.clip = shake;
        waitingSequenceSource.Play();
    }

    public void ShrinkBlackPlay()
    {
        waitingSequenceSource_sub.clip = shrinkBlack;
        waitingSequenceSource_sub.Play();
    }
    

    public void LightPlay() {

        if(lightCount <= 4)
        {
            waitingSequenceSource.clip = light[lightCount];
            waitingSequenceSource.Play();
        }
       

        if(lightCount < 5)
            lightCount++;
    }
    //public void Light1Play()
    //{
    //    waitingSequenceSource.clip = light[0];
    //    waitingSequenceSource.Play();
    //}

    //public void Light2Play()
    //{
    //    waitingSequenceSource.clip = light[1];
    //    waitingSequenceSource.Play();
    //}

    //public void Light3Play()
    //{
    //    waitingSequenceSource.clip = light[2];
    //    waitingSequenceSource.Play();
    //}

    //public void Light4Play()
    //{
    //    waitingSequenceSource.clip = light[3];
    //    waitingSequenceSource.Play();
    //}

    //public void Light5Play()
    //{
    //    waitingSequenceSource.clip = light[4];
    //    waitingSequenceSource.Play();
    //}

    public void FacePlay()
    {
        waitingSequenceSource.clip = face;
        waitingSequenceSource.Play();
    }
}
