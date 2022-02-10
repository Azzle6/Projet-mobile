using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public GameObject currentBuilding;
    public GridLayout gridLayout;
    public Tilemap MainTilemap;
    public Tilemap TempTilemap;
    

    private Vector3Int prevPos;

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
                    prevPos = cellPos;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                //Placer bâtiment
                currentBuilding = null;
            }
        }
    }

    public void GetCellInfos()
    {
        
    }

    public void SpawnBuilding(GameObject build)
    {
        if (currentBuilding == null) currentBuilding = Instantiate(build, Vector3.zero, quaternion.identity);
        else Debug.Log("Building already selected");
    }
}
