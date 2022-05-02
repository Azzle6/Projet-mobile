using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;
    public DiffcultySettings diffPreset;

    public Transform spawnParent;
    public List<Transform> spawnPoints;
    List<Transform> activeSpawnPoints = new List<Transform>();

    public List<Transform> targets;

    public GameObject enemyGroup;
    public int currentWave = 0;
    [Header("Debug")]
    public float difficulty;
    public float levelDiff, techDiff, ressourceDiff;
    void Awake()
    {
        if (instance != this && instance)
            Destroy(this);
        else
            instance = this;
        // setup spawnPoint
        if (spawnParent)
        {
            for (int i = 0; i < spawnParent.childCount; i++)
            {
                spawnPoints.Add(spawnParent.GetChild(i));

            }

        }

        // initialize difficulty preset
        DiffCalculator.setting = diffPreset;
    }

    public void Update()
    {
        // debug diff
        DiffCalculator.DifficultyCalc();
        levelDiff = DiffCalculator.levelDiff;
        techDiff = DiffCalculator.techDiff;
        ressourceDiff = DiffCalculator.ressourceDiff;

        difficulty = DiffCalculator.Difficulty;
        float noiseThresohld = DiffCalculator.setting.noiseThreshold * (1 + DiffCalculator.setting.noiseGainPerWave * currentWave);
        if(RessourceManager_LAC.instance.noise > noiseThresohld)
        {
            RessourceManager_LAC.instance.noise = 0;
            
            currentWave++;

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
        UpdateActiveSpawn(0.5f);
        if(targets.Count > 0) StartWave();
    }

    public void DebugDifficulty()
    {
        difficulty = DiffCalculator.DifficultyCalc();
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
