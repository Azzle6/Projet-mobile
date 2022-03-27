using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine.Tilemaps;

public class IslandTool : EditorWindow
{
    private IslandManager islandMana;
    private GridLayout gridLayout;
    private LayerMask gridMask;
    private Tilemap TileM;
    private int islandIndex = 0;

    private Vector3 mousePosition;
    
    [MenuItem("Assets/IslandTool window")]
    private static void IslandToolWindow()
    {
        EditorWindow.GetWindow(typeof(IslandTool));
        EditorWindow.GetWindow<IslandTool>("OnDestroy");
    }

    private void Init()
    {
        islandMana = FindObjectOfType<IslandManager>();
        TileM = islandMana.tileM;
        gridLayout = TileM.layoutGrid;
        gridMask = FindObjectOfType<BuildingSystem>().GroundMask;
    }

    private void OnGUI()
    {
        if(islandMana == null) Init();
        islandMana = (IslandManager)EditorGUILayout.ObjectField(islandMana, typeof(IslandManager), true);
        
        islandIndex = EditorGUILayout.IntField("Island index", islandIndex);
        
        if (GUILayout.Button("Select Tiles"))
        {
            islandMana.TriggerSelecting(islandIndex);
        }

        islandMana.mousePosition = MousePositionToCell();
    }

    private Vector3Int MousePositionToCell()
    {
        mousePosition = Event.current.mousePosition;
        mousePosition.y = SceneView.lastActiveSceneView.camera.pixelHeight - mousePosition.y;
        Ray ray = SceneView.lastActiveSceneView.camera.ScreenPointToRay(mousePosition);
        //Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit rayHit, 100, gridMask))
        {
            return gridLayout.LocalToCell(rayHit.point);
        }
        else return Vector3Int.zero;
    }
    
    /*
    IEnumerator Selecting()
    {
        Vector3Int firstAngle;
        Vector3Int secondAngle;
        
        yield return new WaitForSeconds(0.5f);

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        firstAngle = MousePositionToCell();
        
        
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
        secondAngle = MousePositionToCell();

        Vector3Int selectionRange = secondAngle - firstAngle;
        int xMultiplicator = selectionRange.x < 0 ? -1 : 1;
        int yMultiplicator = selectionRange.y < 0 ? -1 : 1;

        List<TileInfos> infos = new List<TileInfos>();
        
        for (int x = 0; x < selectionRange.x * xMultiplicator; x++)
        {
            for (int y = 0; y < selectionRange.y * yMultiplicator; y++)
            {
                TileInfos inf = new TileInfos();
                inf.position = TileM.LocalToCell(firstAngle + new Vector3Int(x * xMultiplicator, y * yMultiplicator, 0));
                infos.Add(inf);
            }
        }

        foreach (var tileInf in infos)
        {
            if (!islandMana.islandsList[islandIndex].tilesList.Contains(tileInf))
            {
                islandMana.islandsList[islandIndex].tilesList.Add(tileInf);
            }
        }
    }*/

    
}
