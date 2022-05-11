using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IslandManager : MonoBehaviour
{
    /*public static IslandManager instance;
    [HideInInspector] public Vector3Int mousePosition;
    public Tilemap tileM;
    
    
    //public Dictionary<int, Island> IslandsList = new Dictionary<int, Island>();
    public Island[] islandsList = new Island[2];
    public Island currentIsland;

    private void Awake()
    {
        if (instance != null) return;
        instance = this;
    }

    private void Start()
    {
        currentIsland = islandsList[0];
    }

    [ContextMenu("Switch Island")]
    public void SwitchIsland()
    {
        if (StateManager.CurrentState != StateManager.State.Free) return;
        
        currentIsland.TilemapObject.SetActive(false);
        currentIsland = islandsList[1];
        BuildingSystem.instance.MainTilemap = currentIsland.TilemapObject.GetComponent<Tilemap>();
        //BuildingSystem.instance.gridLayout = BuildingSystem.instance.MainTilemap.layoutGrid;
        BuildingSystem.instance.UpdateTMPos();
        islandsList[1].TilemapObject.SetActive(true);
    }
    
    

    /*[ExecuteInEditMode]
    public void TriggerSelecting(int islandInd)
    {
        StartCoroutine(Selecting(islandInd));
    }
    [ExecuteInEditMode]
    IEnumerator Selecting(int islandIndex)
    {
        Vector3Int firstAngle;
        Vector3Int secondAngle;
        
        yield return new WaitForSeconds(0.5f);
        
        Debug.Log("Can select");

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        firstAngle = mousePosition;
        Debug.Log("Select 1");
        
        
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
        secondAngle = mousePosition;
        Debug.Log("Select 2");

        Vector3Int selectionRange = secondAngle - firstAngle;
        int xMultiplicator = selectionRange.x < 0 ? -1 : 1;
        int yMultiplicator = selectionRange.y < 0 ? -1 : 1;

        List<TileInfos> infos = new List<TileInfos>();
        
        for (int x = 0; x < selectionRange.x * xMultiplicator; x++)
        {
            for (int y = 0; y < selectionRange.y * yMultiplicator; y++)
            {
                TileInfos inf = new TileInfos();
                inf.position = tileM.LocalToCell(firstAngle + new Vector3Int(x * xMultiplicator, y * yMultiplicator, 0));
                infos.Add(inf);
            }
        }

        foreach (var tileInf in infos)
        {
            if (islandsList[islandIndex].tilesList.Contains(tileInf))
            {
                islandsList[islandIndex].tilesList.Add(tileInf);
            }
        }
    }*/
}