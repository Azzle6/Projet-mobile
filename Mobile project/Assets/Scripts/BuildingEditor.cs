using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(Building))]
public class BuildingEditor : Editor
{
    private Building script;
    private SerializedObject targetObject;
    private int width;
    private int height;
    private bool[,] area;
    

    private void OnEnable()
    {
        script = (Building) target;
        targetObject = new SerializedObject(script);
        if (script.area != null)
        {
            width = script.area.GetLength(0);
            height = script.area.GetLength(1);
            area = script.area;
        }
        else
        {
            Debug.Log("ici");
            script.area = new bool[2, 3];
        }
        
    }

    public override void OnInspectorGUI()
    {
        targetObject.Update();
        area = script.area;
        width = EditorGUILayout.IntField(width);
        height = EditorGUILayout.IntField(height);

        if (width != area.GetLength(0) || height != area.GetLength(1))
        {
            Debug.Log("reset");
            area = new bool[width, height];
            script.area = area;
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
        script.area = area;
        if(GUILayout.Button("oui"))
        {
            DebugScriptValue();
        }
        
        targetObject.ApplyModifiedProperties();
    }

    
    private void DebugScriptValue()
    {
        Debug.Log(script.area.GetLength(0));
        Debug.Log(script.area.GetLength(1));
    }
}
