using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    //[HideInInspector]public List<Island> IslandsList = new List<Island>();
    //[HideInInspector]public List<Tilemap> IslandsTMList = new List<Tilemap>();
    public LayerMask GroundMask;
    public List<Vector3Int> tilesInf = new List<Vector3Int>(); // pour le débug
    public Transform SpawnBuildingPos;
    private bool displaceBuildingPhase;
    


    public Dictionary<Vector3Int, TileInfos> globalCellsInfos = new Dictionary<Vector3Int, TileInfos>();
    private Vector3Int tilemapTilePos;

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
    private GameObject placementVFX;
    [SerializeField] private Material canPlace, cantPlace;

    private void Awake()
    { 
        
        
        if (instance != null) return;

        instance = this;
    }

    private void Start()
    {
        InputsManager.PhoneInputs = DisableMouseControl;
        //IslandsList = IslandManager.instance.islandsList.ToList();
        
        
        
        InitializeAllTiles();
        RegisterPreplacedObstacles(BuildingsManager.instance.preplacedBuildings);
    }


    private IEnumerator DisplaceBuilding() //Lorsque le joueur est en phase de placement d'un bâtiment
    {
        while (isMovingBuilding)
        {
            UIManager_LAC.instance.SwitchState(StateManager.State.DisplaceBuilding);
            Ray ray = Camera.main.ScreenPointToRay(InputsManager.GetPosition());
            if (Physics.Raycast(ray, out RaycastHit rayHit, 100, GroundMask))
            {
                Vector3Int cellPos = gridLayout.LocalToCell(rayHit.point);
                //Debug.Log(cellPos.x + " " + cellPos.y);
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
            UIManager_LAC.instance.SwitchState(StateManager.State.HoldBuilding);
            if (InputsManager.Click())
            {
                Ray ray2 = Camera.main.ScreenPointToRay(InputsManager.GetPosition());
                if (Physics.Raycast(ray2, out RaycastHit rayHit2, 100, GroundMask))
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
        if (prevPos != cellPos && !isOutOfGrid)
        {
            currentBuilding.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos + new Vector3(0.5f,0.5f,0));

            prevPos = cellPos;
            
        }
        
        canPlaceBuilding = EmplacementCheck(GetAreaEmplacements(cellPos, currentBuilding.GetComponent<Building>().BuildingScriptable.buildingArea));
        
        Color color = canPlaceBuilding ? Color.green : Color.red;

        if(placementVFX) placementVFX.GetComponent<MeshRenderer>().material = canPlaceBuilding ? canPlace : cantPlace;
        
        
        ChangeColor(prevAreaPositions, Color.white);
        
        ChangeColor(currentAreaPositions, color);
    }

    public void ConfirmBuild(bool result)
    {
        StopCoroutine(DisplaceCoroutine);
        if (result && canPlaceBuilding)
        {
            PlaceBuilding();
            Destroy(placementVFX);
            if (!displaceBuildingPhase)
            {
                Building build = currentBuilding.GetComponent<Building>();
                RessourceManager_LAC.instance.CanPlaceBuilding(build.BuildingScriptable.price.quantity,
                    build.BuildingScriptable.price.ressource);
            }
            currentBuilding = null;
        }
        else
        {
            Destroy(currentBuilding);
            ChangeColor(currentAreaPositions, Color.white);
            currentBuilding = null;
        }

        displaceBuildingPhase = false;
        isMovingBuilding = false;
        UIManager_LAC.instance.SwitchState(StateManager.State.Free);
        
    }

    private void PlaceBuilding()
    {
        foreach (var vect in currentAreaPositions)
        {
            globalCellsInfos[vect].isPlaced = true;
        }

        AudioManager.instance.PlaySound("BUILD_Place");

        Building building = currentBuilding.GetComponent<Building>();
        building.enabled = true;
        building.RegisterTile();
        ChangeColor(currentAreaPositions, Color.black);
    }

    //récupère toutes les coordonnées des tiles concernées et si des tiles sont en dehors de la grid
    public Vector3Int[] GetAreaEmplacements(Vector3Int pos, bool[,] buildingArea)
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

                if (!IsOutOfGrid(emplacement))
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

    //détecte si un emplacement est hors de la grid
    private bool IsOutOfGrid(Vector3Int coordinate)
    {
        if (globalCellsInfos.ContainsKey(coordinate))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    
    private bool IsOutOfGrid(Vector3Int[] coordinate)
    {
        bool result = false;
        foreach (Vector3Int coord in coordinate)
        {
            if (!globalCellsInfos.ContainsKey(coord))
            {
                result = true;
                
            }
        }
        return result;
    }
    

    //Check si l'emplacement est libre
    private bool EmplacementCheck(Vector3Int[] pos)
    {
        prevAreaPositions = currentAreaPositions ;
        

        bool canPlace = true;

        if (!isOutOfGrid)
        {
            foreach (var vect in pos)
            {
                if (globalCellsInfos[vect].isPlaced)
                {
                    canPlace = false;
                    break;
                }
            }
        }
        else canPlace = false;

        if (!isOutOfGrid)
        {
            currentAreaPositions = pos ;
        }
        
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
    

    private void InitializeAllTiles()
    {
        
        foreach (var pos in MainTilemap.cellBounds.allPositionsWithin)
        {
            if (!MainTilemap.HasTile(pos))
            {
                continue;
            }

            TileInfos tileInf = new TileInfos();
            tileInf.position = pos;

            tilemapTilePos = new Vector3Int((int)MainTilemap.transform.localPosition.x, 0,
                (int)MainTilemap.transform.localPosition.z);
        
            globalCellsInfos.Add(pos + tilemapTilePos, new TileInfos(/*TileM.GetTile(pos)*/));
            tilesInf.Add(pos + tilemapTilePos); //pour le débug
        }
        
    }

    public int CalculateTilesOfIsland(Tilemap TM)
    {
        int number = 0;
        foreach (var pos in TM.cellBounds.allPositionsWithin)
        {
            if (TM.HasTile(pos)) number++;
        }

        return number;
    }
    
    public void SpawnBuilding(GameObject build)
    {
        if (currentBuilding == null) currentBuilding = Instantiate(build, SpawnBuildingPos.position, quaternion.identity); 
        else
        {
            Debug.Log("Building already selected");
            return;
        }
        currentBuilding.GetComponent<Building>().enabled = false;
        isMovingBuilding = false;
        GameObject vfx = currentBuilding.GetComponent<Building>().BuildingScriptable.PlacementVFX;
        placementVFX = Instantiate(vfx, currentBuilding.transform.GetChild(0).transform);
        
        UIManager_LAC.instance.SwitchState(StateManager.State.DisplaceBuilding);
        DisplaceCoroutine = StartCoroutine(DisplaceBuilding());
        UpdateBuildingPosition(gridLayout.WorldToCell(SpawnBuildingPos.position));
        
        
    }

    /*public void UpdateTMPos() 
    {
        tilemapTilePos = new Vector3Int((int)IslandManager.instance.transform.localPosition.x, 0,
            (int)IslandManager.instance.transform.localPosition.z);
    }*/

    public void Movebuilding()
    {
        displaceBuildingPhase = true;
        GameObject go = UIManager_LAC.instance.CurrentSelectedBuilding;
        currentBuilding = go.transform.parent.gameObject;
        Vector3Int[] area = GetAreaEmplacements(gridLayout.LocalToCell(go.transform.parent.position),
            go.GetComponentInParent<Building>().BuildingScriptable.buildingArea);
        
        foreach (var vect in area)
        {
            globalCellsInfos[vect].isPlaced = false;
        }
        ChangeColor(area, Color.white, true);
        
        
        currentBuilding.GetComponent<Building>().enabled = false;
        isMovingBuilding = false;
        
        GameObject vfx = currentBuilding.GetComponent<Building>().BuildingScriptable.PlacementVFX;
        placementVFX = Instantiate(vfx, currentBuilding.transform.GetChild(0).transform);
        
        UIManager_LAC.instance.SwitchState(StateManager.State.DisplaceBuilding);
        DisplaceCoroutine = StartCoroutine(DisplaceBuilding());
        UpdateBuildingPosition(gridLayout.WorldToCell(SpawnBuildingPos.position));

        AudioManager.instance.PlaySound("BUILD_Rotate");
    }

    public void RemoveBuilding(GameObject ObjectToRemove = null)
    {
        if (ObjectToRemove == null) ObjectToRemove = UIManager_LAC.instance.CurrentSelectedBuilding;
        currentBuilding = ObjectToRemove.transform.parent.gameObject;
        
        Debug.Log("Removed");
        Vector3Int[] area = GetAreaEmplacements(gridLayout.LocalToCell(ObjectToRemove.transform.parent.position),
            ObjectToRemove.GetComponentInParent<Building>().BuildingScriptable.buildingArea);
        
        foreach (var vect in area)
        {
            globalCellsInfos[vect].isPlaced = false;
        }
        ChangeColor(area, Color.white, true);
        
        Destroy(currentBuilding);
        
        UIManager_LAC.instance.SwitchState(StateManager.State.Free);
    }
    
    
    public void RegisterPreplacedObstacles(GameObject[] buildingsList)
    {
        Debug.Log("Register preplaced buildings");
        foreach (var obj in buildingsList)
        {
            Building buildScript = obj.GetComponent<Building>();
            currentBuilding = obj;
            Vector3Int[] area = GetAreaEmplacements(
                gridLayout.WorldToCell(obj.transform.position),
                buildScript.BuildingScriptable.buildingArea);

            foreach (var vect in area)
            {
                globalCellsInfos[vect].isPlaced = true;
            }

            ChangeColor(area, Color.black);
        }

        currentBuilding = null;
    }

    public void Rotate()
    {
        Building currentBuild = currentBuilding.GetComponent<Building>();
        currentBuilding.transform.Rotate(0,-90,0, Space.Self);
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

        AudioManager.instance.PlaySound("BUILD_Rotate");

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
                newDisplacement = new Vector3Int(-j, - i,0);
                break;
            case Rotation.Right :
                newDisplacement = new Vector3Int(j, i,0);
                break;
            
        }

        return newDisplacement;
    }
    
    

    
}

[System.Serializable]
public class TileInfos
{
    //public TileBase TileB;
    public bool isPlaced = false;
    public Vector3Int position;
    //public Tilemap IslandTM;

    /*public TileInfos(TileBase tileB)
    {
        TileB = tileB;
    }*/
}
