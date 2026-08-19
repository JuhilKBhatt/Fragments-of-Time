using UnityEngine;

public class ChangeTimeCutscene : PerformCutsceneAction
{
    public override float DoAction()
    {
        GameObject.FindGameObjectWithTag("SceneManagerObject").GetComponent<PrefabSceneManager>().HourGlassBroken();
        return base.ActionDuration;
        //Invoke("doActioninvoked", 0.5f);
    }
    public void doActioninvoked()
    {
        GameObject.FindGameObjectWithTag("SceneManagerObject").GetComponent<PrefabSceneManager>().HourGlassBroken();
        //GameObject.FindGameObjectWithTag("HourGlass").GetComponent<BreakHourglass>().destroyGlassOther();
    }
}
