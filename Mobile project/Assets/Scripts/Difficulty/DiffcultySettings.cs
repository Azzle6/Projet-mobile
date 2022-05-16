using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 1, fileName = "DifficultyPreset", menuName = "Scriptables/DifficultySetting")]
public class DiffcultySettings : ScriptableObject
{
    [Header("Level")]
    public int MapSize = 150;
    public float buildingW = 1, defenseW = 2;
    
    [Header("Tech")]
    public float techMax = 6;

    [Header("Ressource")]
    public float maxMatter;
    public float matterW;

    public float maxKnowledge;
    public float knowledgeW;

    [Header("Parameter")]
    public float techW = 1;
    public float levelW = 1;
    public float ressourceW = 1;

    [Header("Noise")]
    public float noiseThreshold = 210;
    [Range(0,1)]
    public float noiseGainPerWave = 0.1f;
    [Header("Enemy")]
    public int maxEnemy = 30;
    public AnimationCurve enemyRatio; // between 0 & 1
    [Range(0, 1)]
    public float enemyDisp = 0;
    public AnimationCurve spawnRatio; // between 0 & 1
    
    


}
