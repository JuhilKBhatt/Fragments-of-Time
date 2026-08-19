using UnityEngine;

[CreateAssetMenu(fileName = "PlayerEchoData", menuName = "ScriptableObjects/EchoData")]
public class EchoData : ScriptableObject
{
    public bool hasEcho;
    public Vector3 savedPosition;
    public int savedAnimationHash;
}