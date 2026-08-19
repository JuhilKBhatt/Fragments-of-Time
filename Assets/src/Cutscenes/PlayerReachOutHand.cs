using UnityEngine;

public class PlayerReachOutHand : PerformCutsceneAction
{
    public override float DoAction()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetBool("ArmOut", true);

        Invoke("doActioninvoked", 2f);
        return base.ActionDuration;
    }
    public void doActioninvoked()
    {

        GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetBool("ArmOut", false);
    }
}
