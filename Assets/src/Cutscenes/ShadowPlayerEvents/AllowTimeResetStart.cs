using UnityEngine;

public class AllowTimeResetStart : PerformCutsceneAction
{
    public override float DoAction()
    {
        GameObject.FindGameObjectWithTag("Level4Manager").GetComponent<Level4Resetting>().shouldReset = true;
        return base.ActionDuration;
        //Invoke("doActioninvoked", 0.5f);
    }
    public void doActioninvoked()
    {

        GameObject.FindGameObjectWithTag("Level4Manager").GetComponent<Level4Resetting>().shouldReset = true;
        //GameObject.FindGameObjectWithTag("HourGlass").GetComponent<BreakHourglass>().destroyGlassOther();
    }
}
