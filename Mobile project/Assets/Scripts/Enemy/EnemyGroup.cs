using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : BehaveGroup
{
    public EnemyStats enemyStats;
    [HideInInspector] public List<EnemyBoid> enemies;

    [Header("Displacement")]
    public Transform target;
    public Transform spawnPoint;

    [Header("Debug")]
    public List<Transform> targets;

    private new void Start()
    {
        InitilaizePreset();

        // initialize boids
       // StartCoroutine(SpawnEnemies(spawnPoint.position, spawnBoids, 0.5f));
        for (int i = 0; i < transform.childCount; i++)
        {
            EnemyBoid eboid = transform.GetChild(i).GetComponent<EnemyBoid>();
            if (eboid)
            {
                AddBoid(eboid);
                eboid.Initialize(this);
            }
        }

        SetTarget(target);




        /*foreach (SteeringBehaviour sb in presetBH.behaviours)
        {
            if(sb is HeadingBH)
            {
                Debug.Log("Head " + sb.name);
                headings = Instantiate(sb as HeadingBH);
            }
            
        }*/
    }

    new void Update()
    {
        // run all steering behaviour throug out boids
        if (enemies != null)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i])
                {
                    enemies[i].ViewDetection();
                    enemies[i].UpdateTargetVelocity(BehaviourHelper.BehavioursVec(enemies[i], presetBH.behaviours));
                    //Debug.DrawRay(boids[i].transform.position, boids[i].targetVelocity);

                    enemies[i].UpdateState();
                    if (enemies[i].enemyState == EnemyBoid.EnemyState.MOVE)
                        enemies[i].Move();
                }
            }
        }

        Transform nTarget = NearestTarget(spawnPoint, targets);
        if ((!target || !target.gameObject.activeSelf))
        {
            target = nTarget;
            SetTarget(target);
        }
            
    }

    #region Group Management
    public void SetTarget(Transform target)
    {
        if (target)
        {
            if (!target.gameObject.activeSelf)
                target = null;
        }

            
        if (target == null)
            Debug.Log("Target null for " + this.name);

        BehaviourHelper.GetHeadingBH(ref presetBH.behaviours).head = target;
        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].target = target;
        }
    }
    public Transform NearestTarget(Transform origin,List<Transform> targets)
    {
        Transform nearestTarget = null;
        float minDist = -1;

        
        for(int i= 0; i < targets.Count; i++)
        {
            if (targets[i])
            {
                if (targets[i].gameObject.activeSelf)
                {
                    float dist = Vector3.Distance(transform.position, targets[i].position);
                    if (minDist < 0 || dist < minDist)
                    {
                        minDist = dist;
                        nearestTarget = targets[i];
                    }
                }
            }
        }
        return nearestTarget;
    }
    #region debug
    public void DebugSpawn()
    {
        StartCoroutine(SpawnEnemies(spawnPoint.transform.position, 10, 0.2f));
    }
    public void DebugResetTarget()
    {
        for(int i = 0; i< targets.Count; i++)
        {
            if (!targets[i].gameObject.activeSelf)
                targets[i].gameObject.SetActive(true);
        }
    }
    #endregion
    public IEnumerator SpawnEnemies(Vector3 origin, int number, float delay)
    {
        yield return new WaitForSeconds(delay);
        Vector3 position = origin + new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, Random.Range(-spawnRadius, spawnRadius));
        InstantiateBoid(position);
        number--;

        if (number > 0)
            StartCoroutine(SpawnEnemies(origin, number, delay));
    }

    #endregion
    #region BoidManagement
    public new void InstantiateBoid(Vector3 position)
    {
        GameObject boid0bj = Instantiate(boidPrefab, position, transform.rotation, transform);
        EnemyBoid enemy = boid0bj.GetComponent<EnemyBoid>();

        if (!enemy)
        {
            Debug.LogWarning("Wrong prefab " + boidPrefab + "in " + this.gameObject);
            return;
        }

        enemy.Initialize(this);
        AddBoid(enemy);
    }
    public new void AddBoid(Boid boid)
    {
        EnemyBoid enemy = boid.gameObject.GetComponent<EnemyBoid>();
        if (!enemy)
            return;

        enemies.Add(enemy);
        base.AddBoid(boid);
    }
    public void AddBoid(EnemyBoid enemy)
    {
        enemies.Add(enemy);
        base.AddBoid(enemy);
    }
    #endregion
}
