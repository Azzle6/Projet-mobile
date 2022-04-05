using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(order = 1, fileName = "Building", menuName = "Scriptables/BuildingSO")]
public class BuildingSO : SerializedScriptableObject
{
    public new string name;
    public string description;
    public ResourceQuantity price;
    public Sprite image;
    public GameObject prefab;
    public BuildingStatSO buildingStats;
    
    public bool[,] buildingArea;
}

[System.Serializable]
public class ResourceQuantity
{
    public float quantity;
    public RessourceManager_LAC.RessourceType ressource;
}
