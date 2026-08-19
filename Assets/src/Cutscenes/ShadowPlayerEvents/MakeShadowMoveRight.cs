using UnityEngine;

public class MakeShadowMoveRight : PerformCutsceneAction
{
    public override float DoAction()
    {
        GameObject.FindGameObjectWithTag("CutscenePlayer").GetComponent<ShadowPlayerMovement>().move(1);
        return base.ActionDuration;
        //Invoke("doActioninvoked", 0.5f);
    }
    public void doActioninvoked()
    {
        GameObject.FindGameObjectWithTag("CutscenePlayer").GetComponent<ShadowPlayerMovement>().move(1);
        //GameObject.FindGameObjectWithTag("HourGlass").GetComponent<BreakHourglass>().destroyGlassOther();
    }
}
