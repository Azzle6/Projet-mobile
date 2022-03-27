using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

[System.Serializable]
public class Island
{
    public List<TileInfos> tilesList = new List<TileInfos>();
    [HideInInspector]public List<Building> BuildingsList = new List<Building>();
    [HideInInspector] public float CurrentSounds;
    [HideInInspector]public bool IsUnderAttack;
    [HideInInspector]public bool IsConnected;
    [HideInInspector]public bool MainIsland;
}