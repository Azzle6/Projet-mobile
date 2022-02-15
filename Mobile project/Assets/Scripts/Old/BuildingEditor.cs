using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


/*[CustomEditor(typeof(BuildingSO))]
public class BuildingEditor : Editor
{
    private BuildingSO script;
    private SerializedObject targetObject;
    private int width;
    private int height;
    private bool[,] area = new bool[2,3];
    

    private void OnEnable()
    {
        Debug.Log("oui");
        script = (BuildingSO) target;
        targetObject = new SerializedObject(script);
        
        width = script.buildingArea.GetLength(0);
        height = script.buildingArea.GetLength(1);
        
        
        /*if (script.buildingArea != null)
        {
            width = script.buildingArea.GetLength(0);
            height = script.buildingArea.GetLength(1);
            //area = script.buildingArea;
        }
        else
        {
            Debug.Log("ici");
            script.buildingArea = new bool[2,2];
        }
        
    }

    public override void OnInspectorGUI()
    {
        
        
        
        //targetObject.Update();

        DrawDefaultInspector();
        
        area = script.buildingArea;
        width = script.width;
        height = script.height;

        if (width != area.GetLength(0) || height != area.GetLength(1))
        {
            DebugScriptValue();
            Debug.Log("reset because width = " + area.GetLength(0) +" != "+ width +" & height = " + area.GetLength(1) + " != " + height);
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
        
        for (int i = 0; i < script.buildingArea.GetLength(0); i++)
        {
            for (int j = 0; j < script.buildingArea.GetLength(1); j++)
            {
                script.buildingArea[i, j] = area[i, j];
            }
        }
        
        
        if(GUILayout.Button("oui"))
        {
            DebugScriptValue();
        }
        
        //targetObject.ApplyModifiedProperties();
    }

    
    private void DebugScriptValue()
    {
        Debug.Log(script.buildingArea.GetLength(0));
        Debug.Log(script.buildingArea.GetLength(1));
    }
}*/
