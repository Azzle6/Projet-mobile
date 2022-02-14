using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(order = 1, fileName = "Building", menuName = "Scriptables/BuildingSO")]
public class BuildingSO : ScriptableObject
{
    public string name;
    public bool[,] buildingArea = new bool[2,2];
}
