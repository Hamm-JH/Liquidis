using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class grabInteraction : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose; // controller info
    public SteamVR_Action_Boolean grabAction;

    private GameObject collidingObject;
    private GameObject objectInHand;

    private void Update()
    {
        //trigger 버튼을 누를 때
        if(grabAction.GetLastStateDown(handType))
        {
            if(collidingObject)
            {
                GrabObject();
            }
        }

        //tirgger 버튼을 놓을 때
        if(grabAction.GetLastStateUp(handType))
        {
            if(objectInHand)
            {
                RelaseObject();
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }

    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
            return;
        collidingObject = null;
    }

    //충돌 중인 개체로 체크
    private void SetCollidingObject(Collider col)
    {
        //이미 충돌 중이거나 rigidbody를 가지고 있지 않은 경우 예외처리
        if (collidingObject || !col.GetComponent<Rigidbody>())
            return;
        collidingObject = col.gameObject;
    }

    //잡기
    private void GrabObject()
    {
        objectInHand = collidingObject;
        collidingObject = null;

        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    }
    //joint 추가
    private FixedJoint AddFixedJoint()
    {
        FixedJoint joint = gameObject.AddComponent<FixedJoint>();
        joint.breakForce = 20000;
        joint.breakTorque = 20000;
        return joint;
    }
    //놓기
    private void RelaseObject()
    {
        if(GetComponent<FixedJoint>())
        {
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());

            objectInHand.GetComponent<Rigidbody>().velocity =
                controllerPose.GetVelocity();
            objectInHand.GetComponent<Rigidbody>().angularVelocity =
                controllerPose.GetAngularVelocity();
        }
        objectInHand = null;
    }
}
