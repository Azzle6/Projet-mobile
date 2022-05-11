using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;
    public DiffcultySettings diffPreset;

    public Transform[] orientedSpawnParents = new Transform[5];
    [Range(0,1)]
    public float[] orientedProba = new float[5];

    [HideInInspector]public List<Transform> spawnPoints;
    List<Transform> activeSpawnPoints = new List<Transform>();
    List<Transform>[] orientedSpawn = new List<Transform>[5];

    public List<Extractor_LAC> targets;

    public GameObject enemyGroup;
    public List<EnemyGroup> groups;

    public int currentWave = 0;
    public bool underAttack;
    public int totalEnnemies;

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
        for(int i = 0; i <orientedSpawnParents.Length; i++)
        {
            if (orientedSpawnParents[i])
            {
                orientedSpawn[i] = new List<Transform>();
                for(int j = 0; j < orientedSpawnParents[i].childCount; j++)
                {
                    Transform spawn = orientedSpawnParents[i].GetChild(j);
                    orientedSpawn[i].Add(spawn);
                    Debug.Log(i + orientedSpawnParents[i].name + spawn.name);
                    spawnPoints.Add(spawn);
                }
            }
        }

        // initialize difficulty preset
        DiffCalculator.setting = diffPreset;
        UpdateActiveSpawn(DiffCalculator.SpawnRatio());
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
            StartWave();

            underAttack = true;
        }

        if (underAttack)
        {
            int currentEnnemies = 0;
            foreach(EnemyGroup g in groups)
            {
                if (g)
                    currentEnnemies += g.enemies.Count;
            }
            totalEnnemies = currentEnnemies;
            if (currentEnnemies == 0)
            {
                underAttack = false;
                UpdateActiveSpawn(DiffCalculator.SpawnRatio());
            }
                
        }
    }
    #region Wave Process
    public void ExtractorAsTarget(List<Extractor_LAC> ext)
    {
        targets = ext;
    }
    public void UpdateActiveSpawn(float spawnRatio)
    {
        // reset oriented spawn
        for (int j = 0; j < orientedProba.Length; j++)
        {
            orientedProba[j] = 0;
        }

        List<Transform> currentSpawns = spawnPoints;
        activeSpawnPoints.Clear();
        int number = (int)Mathf.Ceil(spawnPoints.Count * spawnRatio);

        for (int i = 0; i < number; i++)
        {
            // active spawn
            Transform t = currentSpawns[Random.Range(0, spawnPoints.Count)];
            Debug.Log("Spawn " + t.name);
            activeSpawnPoints.Add(t);
            currentSpawns.Remove(t);

            // oriented spawn
            for(int j = 0; j < orientedSpawn.Length; j++)
            {
                if (orientedSpawn[j].Contains(t))
                {
                    Debug.Log(j+ " find " + t.name);
                    orientedProba[j] += (float)1 / orientedSpawn[j].Count;
                }
                    
            }
        }
    }
    public void StartWave()
    {
        groups.Clear();
        int enemyToSpawn = DiffCalculator.EnemyNumber();
        
        for (int i = 0; i < activeSpawnPoints.Count; i++)
        {
            EnemyGroup enemyG = Instantiate(enemyGroup, activeSpawnPoints[i]).GetComponent<EnemyGroup>();
            enemyG.Initilaize(activeSpawnPoints[i], targets);
            groups.Add(enemyG);

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
    [ContextMenu("Oriented Spawn")]
    public void DebugOrientedSpawn()
    {
        UpdateActiveSpawn(0.5f);
    }
    #endregion
}
