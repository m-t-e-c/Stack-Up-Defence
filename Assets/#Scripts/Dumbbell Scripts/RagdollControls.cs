using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollControls : MonoBehaviour
{
    public void ToggleRagdoll(Transform x, bool y)
    {
        Rigidbody[] rbs = x.GetComponentsInChildren<Rigidbody>();
        Rigidbody charRb = x.GetComponent<Rigidbody>();
        Animator anim = x.GetComponent<Animator>();

        foreach(Rigidbody rb in rbs) rb.isKinematic = y;
        if(anim != null) anim.enabled = y;
        if(charRb != null) charRb.isKinematic = !y;
    }
}
