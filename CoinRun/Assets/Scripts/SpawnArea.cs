using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//
[ExecuteInEditMode]
public class SpawnArea : MonoBehaviour, IComparable<SpawnArea>
{

    //taille de la spawnArea
    public Vector2 spawnArea = new Vector2(25, 25);

    //offset utilisé pour le spawn des pieces
    public float spawnOffset = 0.5f;

    //nombre de colonnes/lignes de chunks la spawnArea (dans ce cas la 5x5)
    public int chunkNumber = 5;

    //nombre total de chunks
    public int chunkCount;

    //liste des chunks
    public Chunk[,] chunksArray;

    //nombres de pieces dans la spawnArea
    [System.NonSerialized]
    public int CoinsInGame;

    //liste de chunks sans pieces
    private List<Chunk> freeChunksList = new List<Chunk>();

    //liste de pieces, utilisée pour la coin alteration
    private List<CoinPickup> coinsListTemp = new List<CoinPickup>();

    //distance du joueur a la spawnArea, utilisée dans le calcul du poids
    public float distFromPlayer { 
        get
        {
            return Vector3.Distance(transform.position, PlayerController.INSTANCE.transform.position);
        } 
    }

    //calcul du poids de la spawnArea
    public float weight { get {
            float t = Mathf.InverseLerp(0, CoinSpawner.maxDistance, distFromPlayer);
            return Mathf.Lerp(1, 10, t);
        } 
    }

    //la spawn area a-t-elle au moins un chunk sans piece ?
    public bool hasFreeChunks { get  { return Mathf.Pow(chunkNumber, 2) - CoinsInGame > 0; } }

    //assignation des differentes variables
    private void Awake()
    {
        CoinSpawner.AddArea(this);
        chunksArray = new Chunk[chunkNumber, chunkNumber];
        for (int i = 0; i < chunkNumber; i++)
        {
            for (int j = 0; j < chunkNumber; j++)
            {
                chunksArray[i, j] = new Chunk(new Vector2Int(i, j), this);
                freeChunksList.Add(chunksArray[i, j]);
            }
        }
        chunkCount = (int)Mathf.Pow(chunkNumber, 2);
    }

    //calcul d'un position dans le monde par rapport aux coordonnées du chunk dans la spawnArea
    public Vector3 ChunkCoordsToWorldPos(Vector2Int coord)
    {
        float chunkSizeX = spawnArea.x / chunkNumber;
        float chunkSizeY = spawnArea.y / chunkNumber;
        Vector3 offset = new Vector3(spawnArea.x / 2, 0f, spawnArea.y / 2);
        return transform.position + new Vector3(chunkSizeX / 2 + coord.x * chunkSizeX, 0, chunkSizeY / 2 + coord.y * chunkSizeY) - offset;
    }

    //recuperation d'un chunk aleatoire dans la liste de chunks libres
    public Chunk GetRandomChunk()
    {
        int rnd = UnityEngine.Random.Range(0, freeChunksList.Count - 1);
        return freeChunksList[rnd];
    }

    //affichage d'un wireframe representant les chunks de la spawnArea
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        float chunkSizeX = spawnArea.x / chunkNumber;
        float chunkSizeY = spawnArea.y / chunkNumber;
        Vector3 offset = new Vector3(spawnArea.x / 2, 0f, spawnArea.y / 2);
        for (int i = 0; i < chunkNumber; i++)
        {
            for (int j = 0; j < chunkNumber; j++)
            {
                Gizmos.DrawWireCube(transform.position + new Vector3(chunkSizeX/2 + i * chunkSizeX, 0, chunkSizeY/2 + j * chunkSizeY) - offset, new Vector3(chunkSizeX, 0.5f, chunkSizeY));
            }
        }
    }

    //recuperation d'une piece au hasard dans la spawnArea en excluant un type de piece
    public CoinPickup getRandomCoin(CoinPickup.CoinType excludingType)
    {
        Chunk tempChunk;
        coinsListTemp.Clear();
        for (int i = 0; i < chunkNumber; i++)
        {
            for (int j = 0; j < chunkNumber; j++)
            {
                tempChunk = chunksArray[i, j];
                if (!tempChunk.containsCoins) continue;
                if (tempChunk.coinController.coinType == excludingType) continue;
                coinsListTemp.Add(tempChunk.coinController);
            }
        }
        if (coinsListTemp.Count == 0) return null;
        int index = UnityEngine.Random.Range(0, coinsListTemp.Count - 1);
        return coinsListTemp[index];
    }

    //implementation d'une fonction de IComparable
    public int CompareTo(SpawnArea other)
    {
        return weight.CompareTo(other.weight);
    }

    public class Chunk
    {

        //reference de la piece sur le chunk
        public CoinPickup coinController;

        //position "dans le monde"
        public Vector3 worldPos;

        //coordonnées dans la spawnArea
        public Vector2Int areaCoord;

        //spawnArea parente
        public SpawnArea refArea;

        //le chunk a-t-il une piece sur lui
        public bool containsCoins
        {
            get
            {
                return coinController != null;
            }
        }

        //constructeur
        public Chunk(Vector2Int coord, SpawnArea area)
        {
            areaCoord = coord;
            refArea = area;
            worldPos = refArea.ChunkCoordsToWorldPos(areaCoord);
        }

        //on fait apparaitre une piece sur le chunk
        public void SpawnCoin(GameObject objToSpawn)
        {
            GameObject newObj = Instantiate(objToSpawn, worldPos, Quaternion.identity, refArea.transform);
            coinController = newObj.GetComponent<CoinPickup>();
            coinController.chunk = this;
            refArea.CoinsInGame++;
            refArea.freeChunksList.Remove(this);
        }

        //on libere le chunk
        public void FreeChunk()
        {
            coinController = null;
            refArea.CoinsInGame--;
            refArea.freeChunksList.Add(this);
        }

    }
}


