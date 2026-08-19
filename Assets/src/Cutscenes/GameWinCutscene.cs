using UnityEngine;

public class GameWinCutscene : PerformCutsceneAction
{
    public override float DoAction()
    {
        GameObject.FindGameObjectWithTag("GameWinUI").GetComponent<WinTheGame>().WIN();
        return base.ActionDuration;
        //Invoke("doActioninvoked", 0.5f);
    }
    public void doActioninvoked()
    {

        GameObject.FindGameObjectWithTag("GameWinUI").GetComponent<WinTheGame>().WIN();
    }
}
