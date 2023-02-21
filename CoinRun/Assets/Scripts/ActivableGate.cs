using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableGate : MonoBehaviour, IActivable
{

    [SerializeField]
    private Animator gateAnimator;

    public bool isActive { get { return gateAnimator.GetBool("isOpen"); } set { } }

    public void SetActive(bool value)
    {
        gateAnimator.SetBool("isOpen", value);
    }
}
