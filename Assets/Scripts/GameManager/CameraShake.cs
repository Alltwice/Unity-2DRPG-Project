using UnityEngine;
using Unity.Cinemachine;
public class CameraShake : MonoBehaviour
{
    public CinemachineImpulseSource impulseSource;
    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    private void OnEnable()
    {
        GameEvent.CameraShake += DoCameraShake;
    }
    private void OnDisable()
    {
        GameEvent.CameraShake -= DoCameraShake;
    }
    public void DoCameraShake(float force)
    {
        impulseSource?.GenerateImpulseWithForce(force);
    }
}
