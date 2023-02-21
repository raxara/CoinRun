using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CoinSpawner))]
public class CoinSpawnerEditor : Editor
{
    CoinSpawner coinSpawner;

    private void OnEnable()
    {
        coinSpawner = (CoinSpawner)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("new spawn area "))
        {
            coinSpawner.CreateSpawnArea();
        }

    }
}
