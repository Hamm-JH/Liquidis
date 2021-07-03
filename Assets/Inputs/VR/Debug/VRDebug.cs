using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

namespace Inputs.VR.Debug
{
    public class VRDebug : MonoBehaviour
    {
        [Header("출력 텍스트")]
        public Text LText;
        public Text RText;

        [Header("컨트롤러 종류")]
        public SteamVR_Input_Sources LeftController;
        public SteamVR_Input_Sources RightController;

        [Header("왼쪽 컨트롤러 변수")]
        public SteamVR_Action_Boolean interactUI;
        public SteamVR_Action_Boolean Lt_GrabPinch;
        public SteamVR_Action_Boolean Lt_GrabGrip;
        public SteamVR_Action_Boolean Lt_TrackpadClick;
        public SteamVR_Action_Boolean Lt_TrackpadTouch;
        public SteamVR_Action_Boolean Lt_Menu;
        public SteamVR_Action_Vector2 Lt_TrackpadPos;
        public SteamVR_Action_Pose Lt_pose;

        [Header("오른쪽 컨트롤러 변수")]
        public SteamVR_Action_Boolean Rt_GrabPinch;
        public SteamVR_Action_Boolean Rt_GrabGrip;
        public SteamVR_Action_Boolean Rt_TrackpadClick;
        public SteamVR_Action_Boolean Rt_TrackpadTouch;
        public SteamVR_Action_Boolean Rt_Menu;
        public SteamVR_Action_Vector2 Rt_TrackpadPos;
        public SteamVR_Action_Pose Rt_pose;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            StringBuilder text = new StringBuilder();

            LText.text = string.Format(
                $"Lt_GrabPinch {Lt_GrabPinch.state}" +
                $"\nLt_GrabGrip {Lt_GrabGrip.state}" +
                $"\nLt_TrackpadClick {Lt_TrackpadClick.state}" +
                $"\nLt_TrackpadTouch {Lt_TrackpadTouch.state}" +
                $"\nLt_Menu {Lt_Menu.state}" +
                $"\nLt_TrackpadPos {Lt_TrackpadPos.axis}" +
                $"\nLt_pose.localPosition " +
                $"\n{Lt_pose.localPosition}" +
                $"\nLt_pose.localRotation {Lt_pose.localRotation}");

            RText.text = string.Format(
                $"Rt_GrabPinch {Rt_GrabPinch.state}" +
                $"\nRt_GrabGrip {Rt_GrabGrip.state}" +
                $"\nRt_Trackpad {Rt_TrackpadClick.state}" +
                $"\nRt_Trackpad {Rt_TrackpadTouch.state}" +
                $"\nRt_Menu {Rt_Menu.state}" +
                $"\nRt_TrackpadPos {Rt_TrackpadPos.axis}" +
                $"\nRt_pose.localPosition" +
                $"\n{Rt_pose.localPosition}" +
                $"\nRt_pose.localRotation {Rt_pose.localRotation}");
		}
    }
}
