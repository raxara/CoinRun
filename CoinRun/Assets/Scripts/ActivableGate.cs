using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//cette classe implemente l'interface Iactivable, et lance l'animation de la porte du cimetiere via la fonction SetBool
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
