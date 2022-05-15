using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;
    public static bool gameOver = false;
    public DiffcultySettings diffPreset;

    public Transform[] orientedSpawnParents = new Transform[5];
    [Range(0,1)]
    public float[] orientedProba = new float[5];

    public List<Transform> spawnPoints;
    [SerializeField]
    List<Transform> activeSpawnPoints = new List<Transform>();
    List<Transform>[] orientedSpawn = new List<Transform>[5];

    public List<Extractor_LAC> targets;

    public GameObject enemyGroup;
    public List<EnemyGroup> groups;

    public int currentWave = 0;
    public bool underAttack;
    public int totalEnnemies;

    [Header("Debug")] 
    [SerializeField] private bool debug = false;
    public float difficulty;
    [SerializeField] private GameObject difficultyDebugText;
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
                    //Debug.Log(i + orientedSpawnParents[i].name + spawn.name);
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
            StartWave(DiffCalculator.EnemyNumber());

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

            if (RessourceManager_LAC.instance.matter + RessourceManager_LAC.instance.knowledge <= 0)
                gameOver = true;
        }
        
        DebugDifficultyText();
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

        List<Transform> currentSpawns = new List<Transform>();
        foreach(Transform t in spawnPoints)
        {
            currentSpawns.Add(t);
        }

        activeSpawnPoints.Clear();
        int number = (int)Mathf.Ceil(spawnPoints.Count * spawnRatio);
        Debug.Log("NB Spawn " + number + "/ " + spawnPoints.Count);

        for (int i = 0; i < number; i++)
        {
            // active spawn
            Transform t = currentSpawns[Random.Range(0, currentSpawns.Count)];
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
    public void StartWave(int enemyToSpawn)
    {
        AudioManager.instance.PlaySound("THREAT_ThresholdReached");

        // assign enemy to spawn
        Dictionary<int, int> enemySpawn = new Dictionary<int, int>();
        for(int i = 0; i < enemyToSpawn; i++)
        {
            int spawnIndex = Random.Range(0, activeSpawnPoints.Count);
            if (enemySpawn.ContainsKey(spawnIndex))
                enemySpawn[spawnIndex]++;
            else
                enemySpawn.Add(spawnIndex, 1);

            
        }

        groups.Clear();
        
        foreach(KeyValuePair<int,int> eS in enemySpawn)
        {
            EnemyGroup enemyG = Instantiate(enemyGroup, activeSpawnPoints[eS.Key]).GetComponent<EnemyGroup>();
            enemyG.Initilaize(activeSpawnPoints[eS.Key], targets);
            groups.Add(enemyG);

            enemyG.SpawnEnemy(eS.Value);
        }
        /*
        for (int i = 0; i < activeSpawnPoints.Count; i++)
        {
            EnemyGroup enemyG = Instantiate(enemyGroup, activeSpawnPoints[i]).GetComponent<EnemyGroup>();
            enemyG.Initilaize(activeSpawnPoints[i], targets);
            groups.Add(enemyG);

            //int midEnemy = (enemyToSpawn / (activeSpawnPoints.Count - i));
            //float enemyDisp = enemyToSpawn*DiffCalculator.setting.enemyDisp * 0.5f;

            //int currentSpawnEnemy = Mathf.RoundToInt(Random.Range(midEnemy - enemyDisp, midEnemy + enemyDisp));
            if(enemyToSpawn > 0)
                enemyG.SpawnEnemy(enemyToSpawn);
        }*/
    }
    #endregion
    #region Debug
    public void DebugWave()
    {
        ExtractorAsTarget(RessourceManager_LAC.instance.activeExtractor);
        UpdateActiveSpawn(0.5f);
        StartWave(3);
    }
    [ContextMenu("DebugDifficulty")]
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
    
    void DebugDifficultyText()
    {
        if (debug)
        {
            if(difficultyDebugText.activeSelf == true) 
            difficultyDebugText.GetComponentInChildren<TextMeshProUGUI>().text = difficulty.ToString("F2");
            else
            {
                difficultyDebugText.SetActive(true);
            }
        }
        else if (difficultyDebugText.activeSelf == true)
        {
            difficultyDebugText.SetActive(false);
        }
    }
    #endregion
}
