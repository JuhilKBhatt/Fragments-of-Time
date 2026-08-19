using UnityEngine;

public class MakeShadowStopMoving : PerformCutsceneAction
{
    public override float DoAction()
    {
        GameObject.FindGameObjectWithTag("CutscenePlayer").GetComponent<ShadowPlayerMovement>().move(0);
        return base.ActionDuration;
        //Invoke("doActioninvoked", 0.5f);
    }
    public void doActioninvoked()
    {
        GameObject.FindGameObjectWithTag("CutscenePlayer").GetComponent<ShadowPlayerMovement>().move(0);
        //GameObject.FindGameObjectWithTag("HourGlass").GetComponent<BreakHourglass>().destroyGlassOther();
    }
}
