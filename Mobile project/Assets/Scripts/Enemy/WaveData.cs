using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "WaveData", menuName = "Scriptables/EnemySettings_SO/WaveData", order = 1)]
public class WaveData : ScriptableObject
{
    public List<WaveCompo>waveCompos;
    [Serializable]
    public struct WaveCompo
    {
        public int difficultyThreshold;
        public List<EnemyQuant> enemies;
    }
    [Serializable]
    public struct EnemyQuant
    {
        public GameObject enemyToSpawn;
        public int number;
    }
}
