using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.Events;
//using Adohi;
using Valve.VR.InteractionSystem;

namespace Manager
{
    public class SteamController : MonoBehaviour
    {
        #region Instance
        private SteamController instance;

        public SteamController Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = FindObjectOfType<SteamController>() as SteamController;
                }
                return instance;
            }
        }
        #endregion

        /// <summary>
        /// Hand 내부에서 SteamController 참조가 되지 않아 외부참조
        /// </summary>
        public Valve.VR.InteractionSystem.Hand[] Hands;

        private void Start()
        {
            //Hands = CharacterManager.Instance.CurrentPlayer.gameObject.GetComponent<Player>().hands;
        }
        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < Hands.Length; i++)
            {
                //if(Hands[i].menuAction.changed)
                //{
                //    OnMenuEvent(Hands[0].menuAction);
                //    break;
                //}

                //if(Hands[i].trackPad.changed)
                //{
                //    OnTrackpadEvent(Hands[0].trackPad);
                //    break;
                //}
                
                if(Hands[i].uiInteractAction.changed)
                {
                    OnUIInteractEvent(Hands[i].uiInteractAction, Hands[i].transform);
                    break;
                }
            }
        }

        /// <summary>
        /// 메뉴 버튼 클릭
        /// </summary>
        /// <param name="menuAction"></param>
        public void OnMenuEvent(SteamVR_Action_Boolean menuAction)
        {
            if(menuAction.state.Equals(true))
            {
                Debug.Log($"Menu action : {menuAction.state}");
            }
        }

        /// <summary>
        /// 트랙패드 위치값
        /// </summary>
        /// <param name="trackpadPos"></param>
        public void OnTrackpadEvent(SteamVR_Action_Vector2 trackpadPos)
        {
            Debug.Log($"TrackPadPos Y axis : {trackpadPos.axis.y}");
        }

        /// <summary>
        /// 트리거 클릭 이벤트
        /// </summary>
        /// <param name="uiInteract"></param>
        public void OnUIInteractEvent(SteamVR_Action_Boolean uiInteract, Transform handTransform)
        {
            if(uiInteract.state.Equals(true))
            {
                Debug.Log($"interact : {uiInteract.state}");
                //Debug.Log($"handTransform position : {handTransform.position}");
                //Debug.Log($"handTransform eulerAngle : {handTransform.eulerAngles}");
            }
        }

        


    }
}
