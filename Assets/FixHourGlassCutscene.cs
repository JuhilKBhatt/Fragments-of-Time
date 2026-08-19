using UnityEngine;

public class FixHourGlassCutscene : PerformCutsceneAction
{
    public override float DoAction()
    {
        GameObject.FindGameObjectWithTag("HourGlass").GetComponent<FixHourglass>().FixHourGlass();
        return base.ActionDuration;
        //Invoke("doActioninvoked", 0.5f);
    }
    public void doActioninvoked()
    {
        GameObject.FindGameObjectWithTag("HourGlass").GetComponent<FixHourglass>().FixHourGlass();
        //GameObject.FindGameObjectWithTag("HourGlass").GetComponent<BreakHourglass>().destroyGlassOther();
    }
}
