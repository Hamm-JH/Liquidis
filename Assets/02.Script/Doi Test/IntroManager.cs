using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    public BallAniTest_Duru ball_ani;

    private static IntroManager _introManager;
    public static IntroManager instance
    {
        get
        {
            if (_introManager == null)
                return null;
            else
                return _introManager;
        }
    }

    private void Awake()
    {
        if (_introManager == null)
            _introManager = this;


    }

    public void TriggerTitleAni()
    {
        ball_ani.TitleAni();
    }
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
