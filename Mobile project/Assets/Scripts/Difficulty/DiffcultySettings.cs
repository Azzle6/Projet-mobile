using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 1, fileName = "DifficultyPreset", menuName = "Scriptables/DifficultySetting")]
public class DiffcultySettings : ScriptableObject
{
    [Header("Level")]
    public int MapSize = 150;
    public float buildingW = 1, defenseW = 2;
    public float levelW = 1;

    [Header("Tech")]
    public float techMax = 6;
    public float techW = 1;

    [Header("People")]
    public float peopleMax = 100;
    public float peopleW = 1;

    [Header("Enemy")]

    public AnimationCurve spawnRatio; // between 0 & 1
    public AnimationCurve enemyRatio; // between 0 & 1

}
