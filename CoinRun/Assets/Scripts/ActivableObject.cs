using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Cette classe implemente Iactivable et fais apparaitre un object via la fonction SetActive
public class ActivableObject : MonoBehaviour, IActivable
{

    [SerializeField]
    private GameObject objectToActivate;

    public bool isActive { get { return objectToActivate.activeSelf; } set { } }

    public void SetActive(bool value)
    {
        objectToActivate.SetActive(value);
    }
}