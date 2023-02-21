using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[ExecuteInEditMode]
public class SpawnArea : MonoBehaviour, IComparable<SpawnArea>
{

    public Vector2 spawnArea = new Vector2(25, 25);

    public float spawnOffset = 0.5f;

    public int chunkNumber = 5;

    public int chunkCount;

    public Chunk[,] chunksArray;

    [System.NonSerialized]
    public int CoinsInGame;

    private List<Chunk> freeChunksList = new List<Chunk>();

    private List<CoinPickup> coinsListTemp = new List<CoinPickup>();

    public float distFromPlayer { 
        get
        {
            return Vector3.Distance(transform.position, PlayerController.INSTANCE.transform.position);
        } 
    }

    public float weight { get {
            float t = Mathf.InverseLerp(0, CoinSpawner.maxDistance, distFromPlayer);
            return Mathf.Lerp(1, 10, t);
        } 
    }

    public bool hasFreeChunks { get  { return Mathf.Pow(chunkNumber, 2) - CoinsInGame > 0; } }

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

    public Vector3 ChunkCoordsToWorldPos(Vector2Int coord)
    {
        float chunkSizeX = spawnArea.x / chunkNumber;
        float chunkSizeY = spawnArea.y / chunkNumber;
        Vector3 offset = new Vector3(spawnArea.x / 2, 0f, spawnArea.y / 2);
        return transform.position + new Vector3(chunkSizeX / 2 + coord.x * chunkSizeX, 0, chunkSizeY / 2 + coord.y * chunkSizeY) - offset;
    }

    public Chunk GetRandomChunk()
    {
        int rnd = UnityEngine.Random.Range(0, freeChunksList.Count - 1);
        return freeChunksList[rnd];
    }

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
                Debug.Log(tempChunk.coinController.coinType);
                if (tempChunk.coinController.coinType == excludingType) continue;
                coinsListTemp.Add(tempChunk.coinController);
            }
        }
        if (coinsListTemp.Count == 0) return null;
        int index = UnityEngine.Random.Range(0, coinsListTemp.Count - 1);
        return coinsListTemp[index];
    }

    public int CompareTo(SpawnArea other)
    {
        return weight.CompareTo(other.weight);
    }

    public class Chunk
    {

        public CoinPickup coinController;

        public Vector3 worldPos;

        public Vector2Int areaCoord;

        public SpawnArea refArea;

        public bool containsCoins
        {
            get
            {
                return coinController != null;
            }
        }

        public Chunk(Vector2Int coord, SpawnArea area)
        {
            areaCoord = coord;
            refArea = area;
            worldPos = refArea.ChunkCoordsToWorldPos(areaCoord);
        }

        public void SpawnCoin(GameObject objToSpawn)
        {
            GameObject newObj = Instantiate(objToSpawn, worldPos, Quaternion.identity, refArea.transform);
            coinController = newObj.GetComponent<CoinPickup>();
            coinController.chunk = this;
            refArea.CoinsInGame++;
            refArea.freeChunksList.Remove(this);
        }

        public void FreeChunk()
        {
            coinController = null;
            refArea.CoinsInGame--;
            refArea.freeChunksList.Add(this);
        }

    }
}


