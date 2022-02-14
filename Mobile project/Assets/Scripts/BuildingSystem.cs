using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem instance;
    
    public GameObject currentBuilding;
    public GridLayout gridLayout;
    public Tilemap MainTilemap;
    public Tilemap TempTilemap;
    public List<Vector3Int> tilesInf = new List<Vector3Int>(); // pour le débug

    public Dictionary<Vector3Int, TileInfos> globalCellsInfos = new Dictionary<Vector3Int, TileInfos>();


    private Vector3Int prevPos;
    private Vector3Int[] prevAreaPositions = Array.Empty<Vector3Int>();
    private Vector3Int[] currentAreaPositions = Array.Empty<Vector3Int>();
    private bool canPlaceBuilding = false;
    private bool isOutOfGrid;

    private void Awake()
    {
        InitializeAllTiles(MainTilemap);
        if (instance != null) return;

        
        instance = this;
    }

    private void Update()
    {
        //Déplacer un bâtiment snap sur grid
        if (currentBuilding != null)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit rayHit))
            {
                Vector3Int cellPos = gridLayout.LocalToCell(rayHit.point);

                if (prevPos != cellPos)
                {
                    currentBuilding.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos + new Vector3(0.5f,0.5f,0));
                    
                    canPlaceBuilding = EmplacementCheck(cellPos, currentBuilding.GetComponent<Building>().BuildingScriptable.buildingArea);
                    
                    prevPos = cellPos;
                }
            }

            if (Input.GetMouseButtonDown(0) && canPlaceBuilding)
            {
                PlaceBuilding();
                currentBuilding = null;
            }
        }
    }

    private void PlaceBuilding()
    {
        foreach (var vect in currentAreaPositions)
        {
            globalCellsInfos[vect].isPlaced = true;
        }
        ChangeColor(currentAreaPositions, Color.white);
    }

    private Vector3Int[] GetAreaEmplacements(Vector3Int pos, bool[,] buildingArea)
    {
        List<Vector3Int> vectList = new List<Vector3Int>();

        bool outOfGrid = false;
        for (int i = 0; i < buildingArea.GetLength(0); i++)
        {
            for (int j = 0; j < buildingArea.GetLength(1); j++)
            {
                if(!buildingArea[i,j]) continue;

                Vector3Int emplacement = new Vector3Int(pos.x + i, pos.y + j, pos.z);

                if (globalCellsInfos.ContainsKey(emplacement))
                {
                    vectList.Add(emplacement);
                }
                else
                {
                    outOfGrid = true;
                    Debug.Log("Déborde de la zone");
                }

            }
        }

        isOutOfGrid = outOfGrid;
        return vectList.ToArray();
    }

    private bool EmplacementCheck(Vector3Int pos, bool[,] buildingArea)
    {
        prevAreaPositions = currentAreaPositions;
        currentAreaPositions = GetAreaEmplacements(pos, buildingArea);

        bool canPlace = true;

        if (!isOutOfGrid)
        {
            foreach (var vect in currentAreaPositions)
            {
                if (globalCellsInfos[vect].isPlaced)
                {
                    canPlace = false;
                    break;
                }
            }
        }
        else canPlace = false;
        
        Color color = canPlace ? Color.green : Color.red;
        
        ChangeColor(prevAreaPositions, Color.white);
        
        ChangeColor(currentAreaPositions, color);

        return canPlace;
    }

    private void ChangeColor(Vector3Int[] area, Color color)
    {
        foreach (var vect in area)
        {
            MainTilemap.SetTileFlags(vect, TileFlags.None);
            MainTilemap.SetColor(vect, color);
            MainTilemap.SetTileFlags(vect, TileFlags.LockColor);
        }
    }
    

    private void InitializeAllTiles(Tilemap TileM)
    {
        
        foreach (var pos in TileM.cellBounds.allPositionsWithin)
        {
            if (!TileM.HasTile(pos))
            {
                continue;
            }
            
            globalCellsInfos.Add(pos, new TileInfos(TileM.GetTile(pos)));
            tilesInf.Add(pos); //pour le débug
        }
    }
    
    public void SpawnBuilding(GameObject build)
    {
        if (currentBuilding == null) currentBuilding = Instantiate(build, Vector3.zero, quaternion.identity);
        else Debug.Log("Building already selected");
    }
}

[System.Serializable]
public class TileInfos
{
    public TileBase TileB;
    public bool isPlaced = false;

    public TileInfos(TileBase tileB)
    {
        TileB = tileB;
    }
}
