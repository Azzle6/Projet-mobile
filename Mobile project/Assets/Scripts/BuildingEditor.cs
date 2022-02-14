using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(BuildingSO))]
public class BuildingEditor : Editor
{
    private BuildingSO script;
    private SerializedObject targetObject;
    private int width;
    private int height;
    private bool[,] area;
    

    private void OnEnable()
    {
        
        script = (BuildingSO) target;
        targetObject = new SerializedObject(script);
        DebugScriptValue();
        if (script.buildingArea != null)
        {
            width = script.buildingArea.GetLength(0);
            height = script.buildingArea.GetLength(1);
            area = script.buildingArea;
        }
        else
        {
            script.buildingArea = new bool[2, 3];
        }
        
    }

    public override void OnInspectorGUI()
    {
        targetObject.Update();
        area = script.buildingArea;
        width = EditorGUILayout.IntField(width);
        height = EditorGUILayout.IntField(height);

        if (width != area.GetLength(0) || height != area.GetLength(1))
        {
            Debug.Log("reset");
            area = new bool[width, height];
            script.buildingArea = area;
        }

        for (int i = 0; i < width; i++)
        {
            Rect r = EditorGUILayout.BeginHorizontal();

            for (int j = 0; j < height; j++)
            {
                area[i,j] = EditorGUILayout.Toggle("", area[i,j]);
                
            }
        
            EditorGUILayout.EndHorizontal();
        }
        script.buildingArea = area;
        if(GUILayout.Button("oui"))
        {
            DebugScriptValue();
        }
        
        targetObject.ApplyModifiedProperties();
    }

    
    private void DebugScriptValue()
    {
        Debug.Log(script.buildingArea.GetLength(0));
        Debug.Log(script.buildingArea.GetLength(1));
    }
}
