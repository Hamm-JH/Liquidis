using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXControl_Test : MonoBehaviour
{
    public VisualEffect vfx;
    public Gradient g;
    GradientColorKey[] colorKey;
    GradientAlphaKey[] alphaKey;


    // Start is called before the first frame update
    void Start()
    {
        g = new Gradient();
        colorKey = new GradientColorKey[4];
        colorKey[0].color = Color.red;//현재 칼라값 넣어준다.
        colorKey[0].time = 0.0f; //칼라값의 위치.(좌측)
        colorKey[0].color = Color.red;//현재 칼라값 넣어준다.
        colorKey[0].time = 0.1f; //칼라값의 위치.(좌측)
        colorKey[1].color = Color.blue;//현재 칼라값 넣어준다.
        colorKey[1].time = 0.9f; //칼라값의 위치.우측)
        colorKey[1].color = Color.blue;//현재 칼라값 넣어준다.
        colorKey[1].time = 1f; //칼라값의 위치.우측)

        alphaKey = new GradientAlphaKey[4];
        alphaKey[0].alpha = 0.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 1f;
        alphaKey[1].time = 0.1f;
        alphaKey[2].alpha = 1f;
        alphaKey[2].time = 0.9f;
        alphaKey[3].alpha = 0.0f;
        alphaKey[3].time = 1.0f;
        g.SetKeys(colorKey, alphaKey);
    }
        // Update is called once per frame
        void Update()
    {
        SetColorGradient();
    }
    void SetColorGradient()
    {
        vfx.SetGradient("ColorGradient", g);
        

    }
}
