using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MeetingRoom : MonoBehaviour
{
    public GameObject meetingHead;
    public VisualEffect vfxObject;


    private void Start()
    {
        StartCoroutine(InitialMappingParameters());
    }

    IEnumerator InitialMappingParameters()
    {
        yield return new WaitForEndOfFrame();
        MappingParameter.instance.InitialMeeintRoomParameters(meetingHead, vfxObject);
        

    }
}
