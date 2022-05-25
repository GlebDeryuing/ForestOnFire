using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexType
{
    int typeId;
    string name;
    float tickFireChanse;
    float factorFire;
    bool canGoThrough;
    Transform prefab;
    List<Transform> currentPrefabs = new List<Transform>();

    public HexType(int typeId, string name, float tickFireChanse, float factorFire, bool canGoThrough)
    {
        this.typeId = typeId;
        this.name = name;
        this.tickFireChanse = tickFireChanse;
        this.factorFire = factorFire;
        this.canGoThrough = canGoThrough;
        this.prefab = GridGenerator.hexsPrefabs[typeId];
        foreach (Transform child in prefab)
        {
            Debug.Log(child);
            currentPrefabs.Add(child);
        }

    }

    public int TypeId { get => typeId; }
    public string Name { get => name; }
    public float TickFireChanse { get => tickFireChanse; }
    public float FactorFire { get => factorFire; }
    public bool CanGoThrough { get => canGoThrough; }
    public Transform Prefab { get => prefab; }
    public List<Transform> CurrentPrefabs { get => currentPrefabs; }
}