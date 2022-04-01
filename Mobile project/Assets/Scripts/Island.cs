using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

[System.Serializable]
public class Island
{
    public GameObject TilemapObject;
    [HideInInspector]public List<Building> BuildingsList = new List<Building>();
    [HideInInspector]public float CurrentSounds;
    [HideInInspector]public bool IsUnderAttack;
    [HideInInspector]public bool IsConnected;
    public bool MainIsland;
}