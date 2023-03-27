using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{

    public static CoinSpawner INSTANCE;

    public int startCoins = 10;

    public int maxCoins = 10;

    [SerializeField]
    public Coin[] coinPrefabs;

    private float totalCoinWeight
    {
        get
        {
            float weight = 0;
            foreach(Coin coin in coinPrefabs)
            {
                weight += coin.weight;
            }
            return weight;
        }
    }

    private float totalAreaWeight
    {
        get
        {
            float weight = 0;
            foreach (SpawnArea sa in freeAreasList)
            {
                weight += sa.weight;
            }
            return weight;
        }
    }

    public float timeBetweenSpawn = 2;

    public float distanceBetweenCoins = 0.5f;

    private float timer = 0;

    public Vector2 spawnArea;

    private List<SpawnArea> spawnAreasList = new List<SpawnArea>();

    private List<SpawnArea> freeAreasList = new List<SpawnArea>();

    [SerializeField]
    private List<Transform> calibrationAnchors;

    public static float maxDistance;

    private List<CoinPickup> coinsListTemp = new List<CoinPickup>();

    public int coinsInGame
    {
        get
        {
            int result = 0;
            foreach (SpawnArea sa in spawnAreasList)
            {
                result += sa.CoinsInGame;
            }
            return result;
        }
    }

    private void Awake()
    {
        INSTANCE = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (SpawnArea sa in spawnAreasList)
        {
            freeAreasList.Add(sa);
        }
        GetMaxDistance();
        for (int i = 0; i < startCoins; i++)
        {
            SpawnCoin();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (coinsInGame < maxCoins)
        {
            timer += Time.deltaTime;
            if (timer >= timeBetweenSpawn)
            {
                SpawnCoin();
                timer = 0;
            }
        }
    }

    private void DebugFreeAreaLists()
    {
        string result = " free spawn areas : ";
        foreach (SpawnArea sa in freeAreasList)
        {
            result += sa.name + ", ";
        }
        //Debug.Log(result);
    }

    private void GetMaxDistance()
    {
        maxDistance = Vector3.Distance(calibrationAnchors[0].position, calibrationAnchors[1].position);
    }

    private void UpdateFreeAreas(SpawnArea area)
    {
        if (area.hasFreeChunks && !freeAreasList.Contains(area))
        {
            freeAreasList.Add(area);
            return;
        }
        if (!area.hasFreeChunks && freeAreasList.Contains(area)) 
        {
            freeAreasList.Remove(area);
        }
    }

    public static void AddArea(SpawnArea sa)
    {
        //if (INSTANCE.spawnAreasList.Contains(sa)) return;
        INSTANCE.spawnAreasList.Add(sa);
    }

    private void SpawnCoin()
    {
        SpawnArea area = GetRandomArea();
        SpawnArea.Chunk c = area.GetRandomChunk();
        c.SpawnCoin(GetRandomCoin());
        UpdateFreeAreas(area);


        Debug.Log("spawned a coin in area " + area.name);
        DebugFreeAreaLists();
        
    }

    public List<CoinPickup> getRandomCoinList(int listSize, CoinPickup.CoinType excludingType)
    {
        coinsListTemp.Clear();
        Shuffle(spawnAreasList);
        int maxIteartionsCall = 10;
        while (coinsListTemp.Count < listSize && maxIteartionsCall > 0)
        {
            foreach(SpawnArea sa in spawnAreasList)
            {
                CoinPickup coin = sa.getRandomCoin(excludingType);
                if (coin == null) continue;
                coinsListTemp.Add(coin);
            }
            maxIteartionsCall--;
        }
        return coinsListTemp;
    }

    public void Shuffle<T>(IList<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    private GameObject GetRandomCoin()
    {
        float curWeight = 0;
        float random = Random.value * totalCoinWeight;
        foreach (Coin coin in coinPrefabs)
        {
            curWeight += coin.weight;
            if (curWeight >= random) return coin.prefab;
        }
        return null;
    }

    [System.Serializable]
    public class Coin
    {
        public GameObject prefab;

        public float weight;
    }

    
    private SpawnArea GetRandomArea()
    {
        float curWeight = 0;
        float random = Random.value * totalAreaWeight;
        freeAreasList.Sort();
        Debug.Log(freeAreasList.Count);
        foreach (SpawnArea sa in freeAreasList)
        {
            curWeight += sa.weight;
            if (curWeight >= random) return sa;
        }
        Debug.Log("blerp");
        return null;
        /*
        int index = Random.Range(0, spawnAreasList.Count);
        while (!spawnAreasList[index].hasFreeChunks)
        {
            index = (index + 1)% spawnAreasList.Count;
        }
        return spawnAreasList[index];
        */
    }
    
    

    public void CreateSpawnArea()
    {
        GameObject test = new GameObject();
        test.transform.parent = transform;
        test.name = "spawnArea";
        test.AddComponent<SpawnArea>();
        SpawnArea sa = test.GetComponent<SpawnArea>();
        spawnAreasList.Add(sa);
        freeAreasList.Add(sa);
        Debug.Log(spawnAreasList[0]);
    }

    /*
    public void DeleteSpawnArea(SpawnArea spawnArea)
    {
        freeAreasList.Add(spawnArea);
        spawnAreasList.Remove(spawnArea);
        Debug.Log("t'as tué mon gosse !!");
    }
    */
}
