using UnityEngine;
using CameraShake;

public class Boss : MonoBehaviour
{
    public PerlinShake.Params shakeParams;

    // Animator Methods.
    public void Footstep(){
        CameraShaker.Shake(new PerlinShake(shakeParams));
    }
}
