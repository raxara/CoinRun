using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script qui lance une coroutine de poursuite si le joueur est a portée, et une coroutine de patrouille si le joueur est trop loin
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
