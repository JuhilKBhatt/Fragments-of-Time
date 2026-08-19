using UnityEngine;

public class PerformCutsceneAction : MonoBehaviour
{
    [SerializeField]
    public float ActionDuration;
    public virtual float DoAction()
    {
        Debug.Log("We doing something");
        return 1;
    }
}
