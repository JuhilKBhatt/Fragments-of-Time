using UnityEngine;

public class MakeShadowLever : PerformCutsceneAction
{
    public override float DoAction()
    {
        GameObject.FindGameObjectWithTag("CutscenePlayer").GetComponent<ShadowPlayerMovement>().getCutsceneLever().GetComponent<ButtonTriggerMultiple>().changeDoors();
        return base.ActionDuration;
        //Invoke("doActioninvoked", 0.5f);
    }
    public void doActioninvoked()
    {
        GameObject.FindGameObjectWithTag("CutscenePlayer").GetComponent<ShadowPlayerMovement>().getCutsceneLever().GetComponent<ButtonTriggerMultiple>().changeDoors();
        //GameObject.FindGameObjectWithTag("HourGlass").GetComponent<BreakHourglass>().destroyGlassOther();
    }
}
