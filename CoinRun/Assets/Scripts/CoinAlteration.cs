using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinAlteration : MonoBehaviour
{
    [SerializeField]
    int coinCount = 10;

    bool isBuff;

    [SerializeField]
    GameObject trailPrefab;

    // Start is called before the first frame update
    void Start()
    {
        isBuff = (Random.value > 0.5f);
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            activateAlteration();
            Destroy(gameObject);
        }
    }

    void activateAlteration()
    {
        CoinPickup.CoinType excludingType = isBuff ? CoinPickup.CoinType.Gold : CoinPickup.CoinType.Copper;
        List<CoinPickup> coinList = CoinSpawner.INSTANCE.getRandomCoinList(coinCount, excludingType);
        Debug.Log(coinList.Count);
        foreach(CoinPickup coin in coinList)
        {
            if (isBuff) coin.coinType += 1;
            else coin.m_coinValue = -coin.m_coinValue;
            createTrail(coin.transform.position);
            Debug.Log("valeur mofidiée dans la zone : " + coin.chunk.refArea.name);
        }
    }

    void createTrail(Vector3 targetPos) 
    {
        GameObject instance = Instantiate(trailPrefab);
        TrailFX trailFX = instance.GetComponent<TrailFX>();
        trailFX.Init(transform.position, targetPos);
    }
}