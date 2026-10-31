using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    private float globalShakeForce = 1f;
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void Shake(CinemachineImpulseSource source )
    {   
        source.GenerateImpulseWithForce(globalShakeForce);
    }
}
