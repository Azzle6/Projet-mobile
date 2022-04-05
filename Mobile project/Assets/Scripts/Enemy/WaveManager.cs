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
    void Awake()
    {
        if (instance != this && instance)
            Destroy(this);
        else
            instance = this;
    }
    public void ExtractorAsTarget(List<Extractor_LAC> ext)
    {
        targets.Clear();
        foreach (Extractor_LAC ex in ext)
            targets.Add(ex.transform);
    }
    public void UpdateActiveSpawn()
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
        for (int i = 0; i < activeSpawnPoints.Count; i++)
        {
            EnemyGroup enemyG = Instantiate(enemyGroup, activeSpawnPoints[i]).GetComponent<EnemyGroup>();
            enemyG.Initilaize(activeSpawnPoints[i], targets);
            enemyG.DebugSpawn();
        }
    }

    public void DebugWave()
    {
        ExtractorAsTarget(RessourceManager_LAC.instance.activeExtractor);
        UpdateActiveSpawn();
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
}
