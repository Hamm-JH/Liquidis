using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


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

    public Renderer ejHead_renderer;
    public Renderer jyHead_renderer;

    public VisualEffect ejVFX;
    public VisualEffect jyVFX;



    static int EJ = 0;
    static int JY = 1;

    [Header("Shader Value")]
    public float geoHigh = 1.0f;
    public float geoLow = 0.1f;
    public float geoAmount = 0.003f;
    float currentEJGeoValue = 0f;
    float currentJYGeoValue = 0f;


    //public float colorHigh = 1.0f;
    //public float colorLow = 0.1f;
    //float currentEJColorValue = 0f;
    //float currentJYColorValue = 0f;


    //Color lerpColor = new Color(0f, 0f, 0f);

    //public Color colorA = new Color(0f, 0f, 0f);
    //public Color colorB = new Color(1f, 1f, 1f);

    public float vfxHigh = 1.0f;
    public float vfxLow = 0.2f;


    void Start()
    {
        animator = GetComponent<Animator>();

        InitialValues();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    BlockAPlay();

        //}

        if (Input.GetKeyDown(KeyCode.A))
        {
            SetEJGeoHigh();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            SetEJGeoLow();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            SetJYGeoHigh();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SetJYGeoLow();
        }


      
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



    void InitialValues()
    {
        currentEJGeoValue = ejHead_renderer.material.GetFloat("_Noise");
        currentJYGeoValue = jyHead_renderer.material.GetFloat("_Noise");

        //currentEJColorValue = ejHead_renderer.material.GetVector("_TextureColor");
        //currentJYColorValue = jyHead_renderer.material.GetVector("_TextureColor");


    }
    // set geo value
    public void SetEJGeoHigh()
    {
        StartCoroutine(LerpGeo(geoHigh, EJ));
    }

    public void SetEJGeoLow()
    {
        StartCoroutine(LerpGeo(geoLow, EJ));

    }

    public void SetJYGeoHigh()
    {
        StartCoroutine(LerpGeo(geoHigh, JY));

    }

    public void SetJYGeoLow()
    {
        StartCoroutine(LerpGeo(geoLow, JY));

    }

    IEnumerator LerpGeo(float targetValue, int target)
    {
        if(target == 0) {
            if (currentEJGeoValue < targetValue)
            {
                while (currentEJGeoValue <= targetValue)
                {
                    ejHead_renderer.material.SetFloat("_Noise", currentEJGeoValue);
                    currentEJGeoValue += geoAmount;
                    yield return null;

                }
            }
            else
            {
                while (currentEJGeoValue >= targetValue)
                {
                    ejHead_renderer.material.SetFloat("_Noise", currentEJGeoValue);
                    currentEJGeoValue -= geoAmount;
                    yield return null;

                }
            }
        } else if (target == 1)
        {
            if (currentJYGeoValue < targetValue)
            {
                while (currentJYGeoValue <= targetValue)
                {
                    jyHead_renderer.material.SetFloat("_Noise", currentJYGeoValue);
                    currentJYGeoValue += geoAmount;
                    yield return null;

                }
            }
            else
            {
                while (currentJYGeoValue >= targetValue)
                {
                    jyHead_renderer.material.SetFloat("_Noise", currentJYGeoValue);
                    currentJYGeoValue -= geoAmount;
                    yield return null;

                }
            }
        }
       
    }

    // set color
    //public void SetEJColorHigh()
    //{
    //    lerpColor = Color.Lerp(colorA, colorB, colorHigh);
    //    ejHead_renderer.material.SetVector("_TextureColor", lerpColor);
    //}

    //public void SetEJColorLow()
    //{
    //    lerpColor = Color.Lerp(colorA, colorB, colorLow);
    //    ejHead_renderer.material.SetVector("_TextureColor", lerpColor);
    //}

    //public void SetJYColorHigh()
    //{
    //    lerpColor = Color.Lerp(colorA, colorB, colorHigh);
    //    jyHead_renderer.material.SetVector("_TextureColor", lerpColor);
    //}

    //public void SetJYColorLow()
    //{
    //    lerpColor = Color.Lerp(colorA, colorB, colorLow);
    //    jyHead_renderer.material.SetVector("_TextureColor", lerpColor);
    //}


    //IEnumerator Lerpcolor(float targetValue, int target)
    //{
    //    if (target == 0)
    //    {
    //        if (currentEJColorValue < targetValue)
    //        {
    //            while (currentEJColorValue <= targetValue)
    //            {
    //                ejHead_renderer.material.SetFloat("_TextureColor", currentEJColorValue);
    //                currentEJColorValue += 0.03f;
    //                yield return null;

    //            }
    //        }
    //        else
    //        {
    //            while (currentEJColorValue <= targetValue)
    //            {
    //                ejHead_renderer.material.SetFloat("_Texturecolor", currentEJColorValue);
    //                currentEJColorValue -= 0.03f;
    //                yield return null;

    //            }
    //        }
    //    }
    //    else if (target == 1)
    //    {
    //        if (currentJYColorValue < targetValue)
    //        {
    //            while (currentJYColorValue <= targetValue)
    //            {
    //                jyHead_renderer.material.SetFloat("_TextureColor", currentJYColorValue);
    //                currentJYColorValue += 0.03f;
    //                yield return null;

    //            }
    //        }
    //        else
    //        {
    //            while (currentJYColorValue <= targetValue)
    //            {
    //                jyHead_renderer.material.SetFloat("_TextureColor", currentJYColorValue);
    //                currentJYColorValue -= 0.03f;
    //                yield return null;

    //            }
    //        }
    //    }

    //}


    public void SetEJVfxHigh()
    {
        ejVFX.SetFloat("SpawnRate", vfxHigh);
    }
    public void SetEJVfxLow()
    {
        ejVFX.SetFloat("SpawnRate", vfxLow);
    }

    public void SetJYVfxHigh()
    {
        jyVFX.SetFloat("SpawnRate", vfxHigh);
    }
    public void SetJYVfxLow()
    {
        jyVFX.SetFloat("SpawnRate", vfxLow);
    }
}
