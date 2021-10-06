using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetingRoom : MonoBehaviour
{
    public GameObject meetingHead;

    private void Start()
    {
        StartCoroutine(InitialMappingParameters());
    }

    IEnumerator InitialMappingParameters()
    {
        yield return new WaitForEndOfFrame();
        MappingParameter.instance.InitialMeeintRoomParameters(meetingHead);
        

    }
}
