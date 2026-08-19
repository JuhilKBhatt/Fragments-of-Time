using UnityEngine;

public class PlayAnimationOnShadow : PerformCutsceneAction
{
    public override float DoAction()
    {
        GameObject.FindGameObjectWithTag("CutscenePlayer").GetComponent<Animator>().SetTrigger("ReachOutHand");
        return base.ActionDuration;
        //Invoke("doActioninvoked", 0.5f);
    }
    public void doActioninvoked()
    {
        GameObject.FindGameObjectWithTag("CutscenePlayer").GetComponent<Animator>().SetTrigger("ReachOutHand");
        //GameObject.FindGameObjectWithTag("HourGlass").GetComponent<BreakHourglass>().destroyGlassOther();
    }
}
