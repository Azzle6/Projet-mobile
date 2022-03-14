using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem instance;

    public bool DisableMouseControl;
    public GameObject currentBuilding;
    public GridLayout gridLayout;
    public Tilemap MainTilemap;
    public Tilemap TempTilemap;
    public List<Vector3Int> tilesInf = new List<Vector3Int>(); // pour le débug
    public GameObject ConfirmBuildButtons;
    public GameObject BuildingsButton;


    public Dictionary<Vector3Int, TileInfos> globalCellsInfos = new Dictionary<Vector3Int, TileInfos>();

    public enum Rotation
    {
        Face,
        Back,
        Right,
        Left
    }

    private Vector3Int prevPos;
    private Vector3Int[] prevAreaPositions = Array.Empty<Vector3Int>();
    private Vector3Int[] currentAreaPositions = Array.Empty<Vector3Int>();
    private bool canPlaceBuilding = false;
    private bool isOutOfGrid;
    [SerializeField]private bool isMovingBuilding;
    private Coroutine DisplaceCoroutine;

    private void Awake()
    {
        InitializeAllTiles(MainTilemap);
        InputsManager.PhoneInputs = DisableMouseControl;
        if (instance != null) return;

        
        instance = this;
    }

    private IEnumerator DisplaceBuilding() //Lorsque le joueur est en phase de placement d'un bâtiment
    {
        while (isMovingBuilding)
        {
            Debug.Log("isMoving");
            Ray ray = Camera.main.ScreenPointToRay(/*Input.mousePosition*/InputsManager.GetPosition());
            if (Physics.Raycast(ray, out RaycastHit rayHit))
            {
                Vector3Int cellPos = gridLayout.LocalToCell(rayHit.point);

                UpdateBuildingPosition(cellPos);
            }

            if (InputsManager.Release())
            {
                isMovingBuilding = false;
                yield return null;
            }
            yield return null;
        }

        while (!isMovingBuilding)
        {
            Debug.Log("isNotMoving");
            
            if (InputsManager.Click())
            {
                Ray ray2 = Camera.main.ScreenPointToRay(InputsManager.GetPosition());
                if (Physics.Raycast(ray2, out RaycastHit rayHit2))
                {
                    Vector3Int cellPos = gridLayout.LocalToCell(rayHit2.point);
                    foreach (var pos in currentAreaPositions)
                    {
                        if (cellPos == pos) isMovingBuilding = true;
                    }
                }
            }
            
            yield return null;
        }

        DisplaceCoroutine = StartCoroutine(DisplaceBuilding());
        
    }

    private void UpdateBuildingPosition(Vector3Int cellPos) //Déplace le bâtiment sur la case indiquée
    {
        if (prevPos != cellPos)
        {
            currentBuilding.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos + new Vector3(0.5f,0.5f,0));

                    
            prevPos = cellPos;
        }
        canPlaceBuilding = EmplacementCheck(cellPos, currentBuilding.GetComponent<Building>().BuildingScriptable.buildingArea);
    }

    public void ConfirmBuild(bool result)
    {
        StopCoroutine(DisplaceCoroutine);
        if (result && canPlaceBuilding)
        {
            PlaceBuilding();
            currentBuilding = null;
        }
        else
        {
            Destroy(currentBuilding);
            ChangeColor(currentAreaPositions, Color.white);
            currentBuilding = null;
        }
        
        isMovingBuilding = false;
        UIManager_LAC.instance.DisplayBuildingChoiceMenu();
        
    }

    private void PlaceBuilding()
    {
        foreach (var vect in currentAreaPositions)
        {
            globalCellsInfos[vect].isPlaced = true;
        }
        
        ChangeColor(currentAreaPositions, Color.black);
    }

    //récupère toutes les coordonnées des tiles concernées
    private Vector3Int[] GetAreaEmplacements(Vector3Int pos, bool[,] buildingArea)
    {
        List<Vector3Int> vectList = new List<Vector3Int>();

        bool outOfGrid = false;
        for (int i = 0; i < buildingArea.GetLength(0); i++)
        {
            for (int j = 0; j < buildingArea.GetLength(1); j++)
            {
                if(!buildingArea[i,j]) continue;

                Vector3Int XYDisplacement = CalculateRotation(i, j);
                Vector3Int emplacement = new Vector3Int(pos.x + XYDisplacement.x, pos.y + XYDisplacement.y, pos.z);

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

    //Check si l'emplacement est libre
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

    private void ChangeColor(Vector3Int[] area, Color color, bool OverrideBlack = false)
    {
        foreach (var vect in area)
        {
            if (MainTilemap.GetColor(vect) == Color.black && !OverrideBlack) continue; //seulement pour éviter que ça override les bâtiments déjà placés
            
            
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
        else
        {
            Debug.Log("Building already selected");
            return;
        }
        
        isMovingBuilding = true;
        UIManager_LAC.instance.DisplayBuildingConfirMenu();
        DisplaceCoroutine = StartCoroutine(DisplaceBuilding());
        
    }

    public void Rotate()
    {
        Building currentBuild = currentBuilding.GetComponent<Building>();
        switch (currentBuild.curRotation)
        {
            case Rotation.Back :
                currentBuild.curRotation = Rotation.Left;
                break;
            case Rotation.Face :
                currentBuild.curRotation = Rotation.Right;
                break;
            case Rotation.Left :
                currentBuild.curRotation = Rotation.Face;
                break;
            case Rotation.Right :
                currentBuild.curRotation = Rotation.Back;
                break;
        }
        UpdateBuildingPosition(gridLayout.LocalToCell(currentBuild.gameObject.transform.localPosition));
    }

    private Vector3Int CalculateRotation(int i, int j)
    {
        Building currentBuild = currentBuilding.GetComponent<Building>();
        Vector3Int newDisplacement = new Vector3Int();
        
        switch (currentBuild.curRotation)
        {
            case Rotation.Back :
                newDisplacement = new Vector3Int(- i,  j,0);
                break;
            case Rotation.Face :
                newDisplacement = new Vector3Int(i, - j,0);
                break;
            case Rotation.Left :
                newDisplacement = new Vector3Int(j, - i,0);
                break;
            case Rotation.Right :
                newDisplacement = new Vector3Int(- j, i,0);
                break;
            
        }

        return newDisplacement;
    }
}

[System.Serializable]
public class TileInfos
{
    public TileBase TileB;
    public bool isPlaced = false;
    public int IslandIndex = 0;

    public TileInfos(TileBase tileB)
    {
        TileB = tileB;
    }
}
