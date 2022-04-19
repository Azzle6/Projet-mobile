using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;
    //public WaveData data;

    [Range(0, 1)]
    public float spawnRatio;
    public List<Transform> spawnPoints;
    List<Transform> activeSpawnPoints = new List<Transform>();

    public List<Transform> targets;

    public GameObject enemyGroup;
    int currentWave;
    void Awake()
    {
        if (instance != this && instance)
            Destroy(this);
        else
            instance = this;

        currentWave = DiffCalculator.currentWave;
    }

    public void Update()
    {
        if(currentWave != DiffCalculator.currentWave)
        {
            currentWave = DiffCalculator.currentWave;  
            ExtractorAsTarget(RessourceManager_LAC.instance.activeExtractor);
            UpdateActiveSpawn(DiffCalculator.SpawnRatio());
            StartWave();
            
        }
    }
    #region Wave Process
    public void ExtractorAsTarget(List<Extractor_LAC> ext)
    {
        targets.Clear();
        foreach (Extractor_LAC ex in ext)
            targets.Add(ex.transform);
    }
    public void UpdateActiveSpawn(float spawnRatio)
    {
        List<Transform> currentSpawns = spawnPoints;
        activeSpawnPoints.Clear();
        int number = (int)Mathf.Ceil(spawnPoints.Count * spawnRatio);

        for (int i = 0; i < number; i++)
        {
            Transform t = currentSpawns[Random.Range(0, spawnPoints.Count)];
            activeSpawnPoints.Add(t);
            currentSpawns.Remove(t);
        }
    }
    public void StartWave()
    {
        int enemyToSpawn = DiffCalculator.EnemyNumber();
        
        for (int i = 0; i < activeSpawnPoints.Count; i++)
        {
            EnemyGroup enemyG = Instantiate(enemyGroup, activeSpawnPoints[i]).GetComponent<EnemyGroup>();
            enemyG.Initilaize(activeSpawnPoints[i], targets);

            int midEnemy = (enemyToSpawn / (activeSpawnPoints.Count - i));
            float enemyDisp = enemyToSpawn*DiffCalculator.setting.enemyDisp * 0.5f;

            int currentSpawnEnemy = Mathf.RoundToInt(Random.Range(midEnemy - enemyDisp, midEnemy + enemyDisp));
            enemyToSpawn -= currentSpawnEnemy;
            enemyG.SpawnEnemy(currentSpawnEnemy);
        }
    }
    #endregion
    #region Debug
    public void DebugWave()
    {
        ExtractorAsTarget(RessourceManager_LAC.instance.activeExtractor);
        UpdateActiveSpawn(spawnRatio);
        if(targets.Count > 0) StartWave();
    }

    public void DebugResetTarget()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (!targets[i].gameObject.activeSelf)
                targets[i].gameObject.SetActive(true);
        }
    }
    #endregion
}
