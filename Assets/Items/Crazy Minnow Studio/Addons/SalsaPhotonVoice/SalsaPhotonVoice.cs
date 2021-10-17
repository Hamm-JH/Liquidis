using Photon.Voice.PUN;
using UnityEngine;
using System.Collections;

namespace CrazyMinnow.SALSA.Addons.Photon
{
    public class SalsaPhotonVoice : MonoBehaviour
    {
   		// RELEASE NOTES & TODO ITEMS:
		//		2.5.0-beta:
		//			+ initial release for SALSA LipSync Suite v2.
		//
		//			REQUIRES: SALSA LipSync Suite v2.5.0+
		// ========================================================================================
		// PURPOSE: This script provides an implementation option for local-avatar lipsync
		// 		functionality (called local-lipsync). PhotonVoice, by default, does not stream
		// 		local-avatar audio to the local-client and subsequently, there is no built-in
		//		way for SALSA to get audio analysis. This script leverages PhotonVoice processor
		//		technology to force PhotonVoice to analyze the audio stream internally and make
		//		this analysis available for SALSA, leveraging the built-in option to use
		//		external analysis. See online documentation for details.
		// ========================================================================================
		// LOCATION OF FILES:
		//		Assets\Crazy Minnow Studio\Addons\SalsaPhotonVoice
		//		Assets\Crazy Minnow Studio\Examples\Scenes      (if applicable)
		//		Assets\Crazy Minnow Studio\Examples\Scripts     (if applicable)
		// ========================================================================================
		// INSTRUCTIONS:
		//		https://crazyminnowstudio.com/docs/salsa-lip-sync/addons/photon-voice/
		//		(visit https://crazyminnowstudio.com/docs/salsa-lip-sync/ for the latest info)
		//		To extend/modify these files, copy their contents to a new set of files and
		//		use a different namespace to ensure there are no scoping conflicts if/when this
		//		add-on is updated.
		// ========================================================================================
		// SUPPORT: Contact assetsupport@crazyminnow.com. Provide:
		//		1) your purchase email and invoice number
		//		2) version numbers (OS, Unity, SALSA, etc.)
		//		3) full details surrounding the problem you are experiencing.
		//		4) relevant information for what you have tried/implemented.
		//		NOTE: Support is only provided for Crazy Minnow Studio products with valid
		//			proof of purchase.
		// ========================================================================================
		// 	KNOWN ISSUES: none.
		// ========================================================================================
		// DISCLAIMER: While every attempt has been made to ensure the safe content
		//		and operation of these files, they are provided as-is, without
		//		warranty or guarantee of any kind. By downloading and using these
		//		files you are accepting any and all risks associated and release
		//		Crazy Minnow Studio, LLC of any and all liability.
		// ========================================================================================

        [SerializeField] private Salsa salsa;
        [SerializeField] private PhotonVoiceView photonVoiceView;
        private SalsaPhotonRecorder salsaPhotonRecorder;

        private IEnumerator Start()
        {
            if (photonVoiceView == null)
               photonVoiceView = GetComponent<PhotonVoiceView>();

            if (photonVoiceView)
            {
                // wait until PhotonVoiceView is setup...
                while (!photonVoiceView.IsSetup)
                    yield return null;

                // A check to see if this is a local client avatar. If so,
                // Salsa needs to be rewired to use external analysis.
                if (photonVoiceView.IsRecorder)
                    ConfigureSalsaForLocalClient();
            }

            if (photonVoiceView == null)
                Debug.LogWarning("SALSA needs a PhotonVoiceView and none could be found.");
        }

        private void ConfigureSalsaForLocalClient()
        {
            if (salsa == null)
                salsa = GetComponent<Salsa>();

            if (salsa)
            {
                salsa.useExternalAnalysis = true;
                salsa.audioSrc = null;
                salsa.scaleExternalAnalysis = true;
                salsa.loCutoff = .05f;
                salsa.hiCutoff = .8f;

                // if SalsaPhotonRecorder is already in the scene, use it, otherwise add it.
                salsaPhotonRecorder = photonVoiceView.RecorderInUse.gameObject.GetComponent<SalsaPhotonRecorder>();
                if (!salsaPhotonRecorder)
                {
                    bool wasRecording = false;
                    if (photonVoiceView.RecorderInUse.IsRecording)
                    {
                        photonVoiceView.RecorderInUse.StopRecording();
                        wasRecording = true;
                    }

                    // Wasn't in scene, so add it and then coax Photon Recorder to fire a new message if necessary.
                    salsaPhotonRecorder = photonVoiceView.RecorderInUse.gameObject.AddComponent<SalsaPhotonRecorder>();
                    if (wasRecording)
                    {
                        // necessary to trigger PhotonVoiceCreated message from Recorder
                        photonVoiceView.RecorderInUse.StartRecording();
                    }
                }

                // Now, if we have a salsaPhotonRecorder, configure SALSA's external analysis delegate.
                if (salsaPhotonRecorder)
                    salsa.getExternalAnalysis = salsaPhotonRecorder.GetAnalysis;
                else
                    Debug.LogWarning("SALSA requires SalsaPhotonRecorder in-line with the Photon Recorder.");
            }
        }
    }
}