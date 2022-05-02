using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildPosCursor : MonoBehaviour
{
    [SerializeField] private LayerMask mask;
    [SerializeField] private Tilemap TileM;
    [SerializeField] private int rightSideSecurity;
    [SerializeField] private int downSideSecurity;
    private void FixedUpdate()
    {
        UpdateCursorPos();
    }

    public void UpdateCursorPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit rayHit, 100, mask))
        {
            Vector3Int cellPos = TileM.WorldToCell(rayHit.point) + Vector3Int.forward; //On ajoute Vector3Int.forward prcq sinon on a une valeur arrondie à -1 en Z
            bool canPlace = true;

            for (int i = 0; i < rightSideSecurity; i++)
            {
                for (int j = 0; j < downSideSecurity; j++)
                {
                    Vector3Int additionVect = new Vector3Int(i, -j, 0);
                    if (BuildingSystem.instance.globalCellsInfos.ContainsKey(cellPos + additionVect))
                    {
                        if (BuildingSystem.instance.globalCellsInfos[cellPos + additionVect].isPlaced)
                        {
                            canPlace = false;
                        }
                    }
                    else canPlace = false;
                }
            }
            
            if(canPlace) transform.position = TileM.CellToWorld(cellPos ) + new Vector3(0.5f,0, 0.5f);
        }
    }
}
