using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class GuideCanvasController : MonoBehaviour
{
    public GameObject targetCanvas;

    public SteamVR_Input_Sources rc;

    public SteamVR_Action_Boolean rt_menu;

    // Update is called once per frame
    void Update()
    {
        if(rt_menu.stateDown)
        {
            targetCanvas.SetActive(!targetCanvas.activeSelf);
        }
    }
}
