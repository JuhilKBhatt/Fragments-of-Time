using UnityEngine;

public class FadeOutObject : PerformCutsceneAction
{
    public override float DoAction()
    {
        return base.ActionDuration;
    }
    public void doActioninvoked()
    {
    }
}
