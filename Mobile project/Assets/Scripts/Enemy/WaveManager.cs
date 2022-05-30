using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;



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
    public int totalBuilding;
    [Header("Debug")] 
    [SerializeField] private bool debug = false;
    public float difficulty;
    [SerializeField] private GameObject difficultyDebugText;
    public float levelDiff, techDiff, ressourceDiff;

    [Header("Feedbacks")]
    [SerializeField] private GameObject waveAlert;
    [SerializeField] private Volume normal, dark;

    void Awake()
    {
        EndStats_LAC.ResetStat();
        gameOver = false;

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
        float noiseThresohld = DiffCalculator.setting.noiseThreshold *
                               (1 + DiffCalculator.setting.noiseGainPerWave * currentWave);
        if(RessourceManager_LAC.instance.noise > noiseThresohld)
        {
            RessourceManager_LAC.instance.noise = 0;
            currentWave++;

            ExtractorAsTarget(RessourceManager_LAC.instance.activeExtractor);
            StartWave(DiffCalculator.EnemyNumber());
            totalBuilding = RessourceManager_LAC.instance.activeExtractor.Count;
            underAttack = true;
        }

        if (underAttack)
        {
            dark.weight = Mathf.Clamp01(dark.weight + Time.deltaTime );
            normal.weight = Mathf.Clamp01(normal.weight - Time.deltaTime );


            int currentEnnemies = 0;
            for(int i = 0; i < groups.Count; i++)
            {
                if (groups[i])
                {
                    //Debug.Log("Group " + groups[i].gameObject.name + " enemy " + groups[i].enemies.Count);
                    currentEnnemies += groups[i].enemies.Count;
                }
            }
            totalEnnemies = currentEnnemies;
            UIManager_LAC.instance.SetEnemiesCount(totalEnnemies);
            if (totalEnnemies == 0)
            {
                underAttack = false;
                AudioManager.instance.PlaySound("THREAT_WaveEnd",0.5f);
                UpdateActiveSpawn(DiffCalculator.SpawnRatio());
            }
        }
        
        if(!underAttack && normal.weight != 1)
        {
            dark.weight = Mathf.Clamp01(dark.weight - Time.deltaTime );
            normal.weight = Mathf.Clamp01(normal.weight + Time.deltaTime);
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
        waveAlert.SetActive(true);
        Invoke("WaveAlertDisable",5f);

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
        StartCoroutine(StartAttack());
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

    public void BuildingCountDown()
    {
        if (underAttack)
        {
            totalBuilding--;
            if (totalBuilding <= 0)
                gameOver = true;
        }
    }
    IEnumerator StartAttack()
    {
        yield return new WaitForSeconds(1);
        underAttack = true;
    }
    #endregion

    void WaveAlertDisable()
    {
        waveAlert.SetActive(false);
    }

    #region Debug
    public void DebugWave()
    {
        Debug.Log("Debug xwave");
        ExtractorAsTarget(RessourceManager_LAC.instance.activeExtractor);
        UpdateActiveSpawn(0.5f);
        StartWave(10);
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
        else if (!debug && difficultyDebugText.activeSelf == true)
        {
            difficultyDebugText.SetActive(false);
        }
    }
    #endregion
}
