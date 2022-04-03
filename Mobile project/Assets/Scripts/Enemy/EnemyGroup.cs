using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : BehaveGroup
{
    public EnemyStats enemyStats;
    [HideInInspector] public List<EnemyBoid> enemies;
    public Transform target;
    [Header("Debug")]
    public HeadingBH headings;
    private new void Start()
    {
        InitilaizePreset();
        // initialize boids
        for (int i = 0; i < transform.childCount; i++)
        {
            EnemyBoid eboid = transform.GetChild(i).GetComponent<EnemyBoid>();
            if (eboid)
            {
                AddBoid(eboid);
                eboid.Initialize(this);
            }
        }

        for (int i = 0; i < spawnBoids; i++)
        {
            Vector3 position = transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, Random.Range(-spawnRadius, spawnRadius));
            InstantiateBoid(position);
        }

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
                enemies[i].ViewDetection();
                enemies[i].UpdateTargetVelocity(BehaviourHelper.BehavioursVec(enemies[i], presetBH.behaviours));
                //Debug.DrawRay(boids[i].transform.position, boids[i].targetVelocity);

                enemies[i].UpdateState();
                if(enemies[i].enemyState == EnemyBoid.EnemyState.MOVE)
                    enemies[i].Move();
            }
        }
    }

    #region Group Management
    public void SetTarget(Transform target)
    {
        BehaviourHelper.GetHeadingBH(ref presetBH.behaviours).head = target;
        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].target = target;
        }
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
