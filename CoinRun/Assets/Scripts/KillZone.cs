using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//zone utilis�e par le fantome est les cours d'eau, elle teleporte le joueur a un spawnPoint et lui fait subir une penalit� de pi�ces
public class KillZone : MonoBehaviour
{

    [SerializeField]
    Transform spawnPoint;

    [SerializeField]
    Transform playerTransform;

    [SerializeField]
    int coinPenalty = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            KillPlayer(); 
        }
    }

    private void KillPlayer()
    {
        ScoreController.removeCoin(coinPenalty);
        playerTransform.position = spawnPoint.position;
    }

}
