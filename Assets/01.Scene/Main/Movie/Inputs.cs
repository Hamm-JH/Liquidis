using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace MovieScene
{
    public class Inputs : MonoBehaviour
    {
        [Header("Controllers")]
        [SerializeField] private SteamVR_Input_Sources rightController;

        [Header("Inputs")]
        [SerializeField] private SteamVR_Action_Boolean rt_menu;
        [SerializeField] private SteamVR_Action_Vector2 rt_trackpos;

        [Header("Debug")]
        public float x = 0;
        public float y = 0;

        [Header("Target Code")]
        public MovieControl target;

        private void Update()
        {
            if(rt_menu.stateDown)
            {
                // ¾À Á¾·á
                target.DebugToIntro();
            }

            Vector2 pos = rt_trackpos.axis;

            x = pos.x;
            y = pos.y;

            target.UpdateX(y);
            target.UpdateY(x);
        }

    }
}
