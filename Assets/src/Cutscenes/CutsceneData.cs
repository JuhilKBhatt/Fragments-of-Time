using NUnit.Framework.Internal;
using UnityEngine;


[CreateAssetMenu(fileName = "CutsceneData", menuName = "ScriptableObjects/CutsceneData")]
public class CutsceneData : ScriptableObject
{
    [TextArea(1, 20)]
    public string[] cutsceneText;
    public bool[] isCutsceneAction;
    public cutsceneActionType[] actions;
    public GameObject[] objectsToInstantiateInScene;
    public int[] objectToAniamte;
    public PerformCutsceneAction[] thignsToDo;
}

public enum cutsceneActionType
{
    None,
    PlayerMovement,
    ObjectInstantation,
    ObjectAnimation,
    Event,
    ObjectRemoval
}
