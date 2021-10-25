 using Valve.VR;

public class SteamInputModule : VRInputModule
{

	public SteamVR_Input_Sources m_Source = SteamVR_Input_Sources.RightHand;
	public SteamVR_Action_Boolean m_Click = null;

    public bool isCheckTimer = false;

    private bool isDown = false;
    private bool isUp = false;

    public override void Process()
    {
        base.Process();

        // Press
        if (m_Click.GetStateDown(m_Source))
        {
            Press();
            isDown = true;
        }
        else
        {
            isDown = false;
        }

        // Release
        if (m_Click.GetStateUp(m_Source))
        {
            Release();
            isUp = true;
        }
        else
        {
            isUp = false;
        }

        if(isCheckTimer && (isDown || isUp))
        {
            MonitorTime.instance.CancelCheck();
        }
    }
    
}
