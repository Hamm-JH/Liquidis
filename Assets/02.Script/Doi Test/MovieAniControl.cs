using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieAniControl : MonoBehaviour
{
    Animator animator;
    public AudioClip[] blockA;
    public AudioClip[] blockB;
    public AudioClip[] blockC;
    public AudioClip[] blockD;
    public AudioClip[] blockE;
    public AudioClip[] blockF;

    public AudioSource ejHead;
    public AudioSource jyHead;



    static int EJ = 0;
    static int JY = 1;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void BlockAPlay()
    {
        ejHead.clip = blockA[EJ];
        jyHead.clip = blockA[JY];

        ejHead.Play();
        jyHead.Play();


    }

    public void BlockBPlay()
    {
        ejHead.clip = blockB[EJ];
        jyHead.clip = blockB[JY];

        ejHead.Play();
        jyHead.Play();


    }

    public void BlockCPlay()
    {
        ejHead.clip = blockC[EJ];
        jyHead.clip = blockC[JY];

        ejHead.Play();
        jyHead.Play();


    }

    public void BlockDPlay()
    {
        ejHead.clip = blockD[EJ];

        ejHead.Play();


    }

    public void BlockEPlay()
    {
        ejHead.clip = blockE[EJ];
        jyHead.clip = blockE[JY];

        ejHead.Play();
        jyHead.Play();


    }

    public void BlockFPlay()
    {
        ejHead.clip = blockF[EJ];
        jyHead.clip = blockF[JY];

        ejHead.Play();
        jyHead.Play();


    }
}
