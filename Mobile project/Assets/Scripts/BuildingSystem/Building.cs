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
    public virtual void RegisterTile()
    {
        RessourceManager_LAC.instance.buildTile += GetTile();
    }
    public int GetTile()
    {
        int tile = 0;
        foreach (bool b in BuildingScriptable.buildingArea)
        {
            if (b) { tile++; }
        }
        return tile;
    }
    public virtual void Upgrade()
    {
        if (statsSO.Length <= 0) 
            return;
        
        if (BuildingScriptable.unlockedLevel > level)
        {
            level = Mathf.Clamp(level+1, 0, statsSO.Length -1);
            Debug.Log("Upgrade !");
            int index = level + 1;
            VFXManager.instance.PlayVFX("UpgradeBuildingLvl" + index, transform.GetChild(0).transform);
            AudioManager.instance.PlaySound("BUILD_Upgrade");
            return;
        }
        Debug.Log("Upgrade pas");
    }

    public virtual void Remove()
    {
        BuildingSystem.instance.RemoveBuilding();
        
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
