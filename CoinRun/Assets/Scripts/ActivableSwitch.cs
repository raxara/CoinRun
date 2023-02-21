using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableSwitch : MonoBehaviour
{

    public GameObject activableGO;

    IActivable iActivable;

    private bool isOn = false;

    private void Start()
    {
        iActivable = activableGO.GetComponent<IActivable>();
        if (iActivable == null) Debug.LogError("IActivable not found on object " + activableGO.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isOn) return;
            isOn = true;
            SwitchOn();
        }
    }

    private void SwitchOn()
    {
        iActivable.SetActive(true);
    }
}
