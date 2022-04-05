using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildPosCursor : MonoBehaviour
{
    [SerializeField] private LayerMask mask;
    [SerializeField] private Tilemap TileM;
    private void Update()
    {
        UpdateCursorPos();
    }

    public void UpdateCursorPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit rayHit, 100, mask))
        {
            Vector3Int cellPos = TileM.WorldToCell(rayHit.point) + Vector3Int.forward; //On ajoute Vector3Int.forward prcq sinon on a une valeur arrondie à -1 en Z
            if(BuildingSystem.instance.globalCellsInfos.ContainsKey(cellPos)) 
            {
                if (!BuildingSystem.instance.globalCellsInfos[cellPos].isPlaced)
                {
                    transform.position = TileM.CellToWorld(cellPos ) + new Vector3(0.5f,0, 0.5f);
                }
                
            }
        }
    }
}
