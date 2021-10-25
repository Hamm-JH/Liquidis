using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TrackpadMove : MonoBehaviour
{
    public SteamVR_Input_Sources rightController;

    public SteamVR_Action_Vector2 Rt_TrackpadPos;

    public Transform targetTransform;

    [Range(0, 1)]
    public float moveVector;

    public bool isCheckTimer;

    private float prevY = 0f;

    // Update is called once per frame
    void Update()
    {
        float y = Rt_TrackpadPos.axis.y;
        //Debug.Log("y" + y);
        targetTransform.Translate(Vector3.forward * y * moveVector);

        if(isCheckTimer && prevY != y)
        {
            MonitorTime.instance.CancelCheck();
        }

        prevY = y;
    }
}
