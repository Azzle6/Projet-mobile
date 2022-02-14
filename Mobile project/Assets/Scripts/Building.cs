using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Building : MonoBehaviour
{
    public bool[,] area = new bool[2,2];
    public GameObject Visual;
    public int UpgradeTier;
    public float PlacementSound;
    public bool Activated;

    private void Start()
    {
        for (int i = 0; i < area.GetLength(0); i++)
        {
            for (int j = 0; j < area.GetLength(1); j++)
            {
                
                
                if(Random.Range(0,2) == 0) area[i, j] = true;
                else area[i, j] = false;
            }
        }//ça c'est juste pour exemple
    }
}
