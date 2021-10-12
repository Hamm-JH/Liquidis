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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float y = Rt_TrackpadPos.axis.y;
        //Debug.Log("y" + y);
        targetTransform.Translate(Vector3.forward * y * moveVector);
    }
}
