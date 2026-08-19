using UnityEngine;

public class MakeTimeReset : PerformCutsceneAction
{
    public override float DoAction()
    {
        GameObject.FindGameObjectWithTag("CutscenePlayer").GetComponent<ShadowPlayerMovement>().getCutsceneLever().GetComponent<ButtonTriggerMultiple>().initalDoor();
        GameObject.FindGameObjectWithTag("Level4Manager").GetComponent<Level4Resetting>().ActivateGlitchCutscene();
        return base.ActionDuration;
        //Invoke("doActioninvoked", 0.5f);
    }
    public void doActioninvoked()
    {
        GameObject.FindGameObjectWithTag("CutscenePlayer").GetComponent<ShadowPlayerMovement>().getCutsceneLever().GetComponent<ButtonTriggerMultiple>().initalDoor();
        GameObject.FindGameObjectWithTag("Level4Manager").GetComponent<Level4Resetting>().ActivateGlitchCutscene();
        //GameObject.FindGameObjectWithTag("HourGlass").GetComponent<BreakHourglass>().destroyGlassOther();
    }
}
