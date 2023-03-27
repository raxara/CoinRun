using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostDetectionZone : MonoBehaviour
{

    [SerializeField]
    GhostScript ghostScript;

    [SerializeField]
    GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        ghostScript.FocusPlayer(player);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        ghostScript.StartPatrolling();
    }
}
