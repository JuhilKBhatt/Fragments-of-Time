using UnityEngine;

public class DestroyHourglassCutscene : PerformCutsceneAction
{
    public override float DoAction()
    {
        GameObject.FindGameObjectWithTag("HourGlass").GetComponent<BreakHourglass>().destroyGlassOther();
        return 1f;
        //Invoke("doActioninvoked", 0.5f);
    }
    public void doActioninvoked()
    {

        GameObject.FindGameObjectWithTag("HourGlass").GetComponent<BreakHourglass>().destroyGlassOther();
    }
}
