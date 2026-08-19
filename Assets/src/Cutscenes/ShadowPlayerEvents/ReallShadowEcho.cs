using UnityEngine;

public class ReallShadowEcho : PerformCutsceneAction
{
    public override float DoAction()
    {
        GameObject.FindGameObjectWithTag("CutscenePlayer").GetComponent<ShadowEchoAbility>().returnToEcho();
        return base.ActionDuration;
        //Invoke("doActioninvoked", 0.5f);
    }
    public void doActioninvoked()
    {
        GameObject.FindGameObjectWithTag("CutscenePlayer").GetComponent<ShadowEchoAbility>().returnToEcho();
        //GameObject.FindGameObjectWithTag("HourGlass").GetComponent<BreakHourglass>().destroyGlassOther();
    }
}
