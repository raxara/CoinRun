using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{

    public enum CoinType
    {
        Copper,
        Silver,
        Gold
    }

    [SerializeField]
    private int m_coinValue;

    [System.NonSerialized]
    public SpawnArea.Chunk chunk;

    public CoinType coinType;

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
