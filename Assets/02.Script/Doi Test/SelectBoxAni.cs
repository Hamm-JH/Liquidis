using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBoxAni : MonoBehaviour
{
    public Animator lightGroup_ani;

    public void TriggerLightGroup()
    {
        lightGroup_ani.SetTrigger("Start");
    }

    public void SelectAniEnd()
    {
        WaitingRoom.instance.DisabledSelectBox();
    }
}
