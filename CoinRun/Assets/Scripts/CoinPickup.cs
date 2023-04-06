using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script utilisé sur chaque piece, avec une valeur, un type de piece, et la reference a son chunk de spawn (voir spawnArea)
public class CoinPickup : MonoBehaviour
{

    public enum CoinType
    {
        Copper,
        Silver,
        Gold
    }

    public int m_coinValue;

    [System.NonSerialized]
    public SpawnArea.Chunk chunk;

    public CoinType coinType;

    //quand le joueur touche une piece, on ajoute la valeur a son score, on libere le chunk et on detruit la piece
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreController.AddCoin(m_coinValue);
            chunk.FreeChunk();
            Destroy(this.gameObject);
        }
    }

}
