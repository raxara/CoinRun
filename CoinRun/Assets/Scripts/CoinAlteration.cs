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

    //pour activer l'alteration, on definit aleatoirement si c'est un buff ou un debuff, puis on recupere une liste de pieces avant de modifier leur valeur
    //puis on crée une orbe qui mene a chaque piece
    void activateAlteration()
    {
        CoinPickup.CoinType excludingType = isBuff ? CoinPickup.CoinType.Gold : CoinPickup.CoinType.Copper;
        List<CoinPickup> coinList = CoinSpawner.INSTANCE.getRandomCoinList(coinCount, excludingType);
        foreach(CoinPickup coin in coinList)
        {
            if (isBuff) coin.coinType += 1;
            else coin.m_coinValue = -coin.m_coinValue;
            createTrail(coin.transform.position);
        }
    }

    //fait apparaitre une orbe
    void createTrail(Vector3 targetPos) 
    {
        GameObject instance = Instantiate(trailPrefab);
        TrailFX trailFX = instance.GetComponent<TrailFX>();
        trailFX.Init(transform.position, targetPos);
    }
}