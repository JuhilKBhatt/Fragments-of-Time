using Unity.Cinemachine;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    [SerializeField] 
    private float shakeIntensity = 1f;
    [SerializeField]
    CinemachineImpulseSource source;
    public void CameraShake()
    {
        source.GenerateImpulseWithForce(shakeIntensity);
    }
}
