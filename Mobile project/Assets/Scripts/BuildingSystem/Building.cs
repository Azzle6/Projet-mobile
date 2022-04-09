using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Building : MonoBehaviour
{
    public BuildingSO BuildingScriptable;
    public GameObject Visual;
    //[HideInInspector]
    public int level;
    public float PlacementSound;
    public bool Activated;
    public BuildingSystem.Rotation curRotation = BuildingSystem.Rotation.Face;
    public BuildingStatSO[] statsSO;

    private void Awake()
    {
        if (Visual)
            Instantiate(Visual, transform);
    }

    public virtual void Upgrade()
    {
        Debug.Log("Commence l'upgrade");
        if (statsSO.Length <= 0) 
            return;
        
        if (BuildingScriptable.unlockedLevel > level)
        {
            level = Mathf.Clamp(level+1, 0, statsSO.Length);
            Debug.Log("Upgrade !");
            return;
        }
        Debug.Log("Upgrade pas");
    }
    
 

    /*private void Start()
    {
        for (int i = 0; i < area.GetLength(0); i++)
        {
            for (int j = 0; j < area.GetLength(1); j++)
            {
                
                
                if(Random.Range(0,2) == 0) area[i, j] = true;
                else area[i, j] = false;
            }
        }//ça c'est juste pour exemple
    }*/
}
