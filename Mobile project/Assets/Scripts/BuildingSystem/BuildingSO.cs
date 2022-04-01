using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(order = 1, fileName = "Building", menuName = "Scriptables/BuildingSO")]
public class BuildingSO : SerializedScriptableObject
{
    public string name;
    public string production;
    public string description;
    public string price;
    public Sprite image;
    public GameObject prefab;
    
    public bool[,] buildingArea;
}
