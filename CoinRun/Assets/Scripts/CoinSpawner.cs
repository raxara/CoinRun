using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//singleton manager du spawn de toutes les pieces du jeu
public class CoinSpawner : MonoBehaviour
{
    public static CoinSpawner INSTANCE;

    //pieces au demarrage du jeu et nombre maximum de pieces en meme temps en jeu
    public int startCoins = 10;

    public int maxCoins = 10;

    //prefabs des pieces (3 types differents)
    [SerializeField]
    public Coin[] coinPrefabs;

    //temps entre 2 spawn de pieces + le timer 
    public float timeBetweenSpawn = 2;

    private float timer = 0;

    //distance minimum entre 2 pieces
    public float distanceBetweenCoins = 0.5f;

    //liste de spawnareas du jeu, et liste de celles qui sont libres (il leur reste au moins une place pour une piece)
    private List<SpawnArea> spawnAreasList = new List<SpawnArea>();

    private List<SpawnArea> freeAreasList = new List<SpawnArea>();

    //ancres et variable servant a calibrer la taille du terrain et a la stocker
    [SerializeField]
    private List<Transform> calibrationAnchors;

    public static float maxDistance;

    //liste de pieces (utilisée pour selectionner les pieces pour l'alteration correspondante)
    private List<CoinPickup> coinsListTemp = new List<CoinPickup>();

    //poids total des pieces (utilisé pour faire spawner les pieces avec une probabilité differente par type de piece)
    //la probabilité baisse proportionellement a la valeur de la piece
    private float totalCoinWeight
    {
        get
        {
            float weight = 0;
            foreach (Coin coin in coinPrefabs)
            {
                weight += coin.weight;
            }
            return weight;
        }
    }

    //meme principe que pour les piece, la probabilité augmente la distance entre le joueur et la zone
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

    //nombre de pieces actuellement en jeu
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

    //assignation des variables, calcul de la distance max et spawn des pieces pour le demarrage du jeu
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

    //a chaque frame on verifie juste si il manque des pieces pour atteindre le maximum, et on en fait respawner si c'est le cas 
    //(pourrait etre remplacé par une coroutine qu'on lance a chaque collecte de piece pour eviter les appels inutiles)
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

    //calcul de la distance entre les ancres (sert a calculer le poids des spawnArea)
    private void GetMaxDistance()
    {
        maxDistance = Vector3.Distance(calibrationAnchors[0].position, calibrationAnchors[1].position);
    }

    //mise a jour de la liste de spawnAreas libres
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

    //ajout de spawnArea
    public static void AddArea(SpawnArea sa)
    {
        INSTANCE.spawnAreasList.Add(sa);
    }

    //fonction de spawn de piece : on choisit une zone aléatoire, puis un chunk aleatoire de cette zone puis on y fait apparaitre une piece au hasard
    private void SpawnCoin()
    {
        SpawnArea area = GetRandomArea();
        SpawnArea.Chunk c = area.GetRandomChunk();
        c.SpawnCoin(GetRandomCoin());
        UpdateFreeAreas(area);
    }

    //recuperation d'une liste de pieces qui ne sont pas d'un certain type, utilisée par le script CoinAlteration
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

    //sert a melanger une liste grace au polymorphisme
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

    //fonction qui sert a choisir un type de piece aleatoirement avec un systeme de poids
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

    //classe interne servant de lier un prefab piece a un poids
    [System.Serializable]
    public class Coin
    {
        public GameObject prefab;

        public float weight;
    }

    //meme logique que pour les pieces, mais avec les spawnAreas
    private SpawnArea GetRandomArea()
    {
        float curWeight = 0;
        float random = Random.value * totalAreaWeight;
        freeAreasList.Sort();
        foreach (SpawnArea sa in freeAreasList)
        {
            curWeight += sa.weight;
            if (curWeight >= random) return sa;
        }
        return null;
    }

    //fonction utilisée dans mon custom editor pour creer une spawn area via l'inspecteur
    public void CreateSpawnArea()
    {
        GameObject test = new GameObject();
        test.transform.parent = transform;
        test.name = "spawnArea";
        test.AddComponent<SpawnArea>();
        SpawnArea sa = test.GetComponent<SpawnArea>();
        spawnAreasList.Add(sa);
        freeAreasList.Add(sa);
    }
}
