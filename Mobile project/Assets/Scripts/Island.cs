using System;
using System.Collections.Generic;
using UnityEngine;

public class Island
{
    public Vector2Int[] Positions = new Vector2Int[2];
    public List<Building> BuildingsList = new List<Building>();
    public float CurrentSounds;
    public bool IsUnderAttack;
    public bool IsConnected;
    public bool MainIsland;
}