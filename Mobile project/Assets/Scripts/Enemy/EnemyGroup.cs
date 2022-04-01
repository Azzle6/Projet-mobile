using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : BehaveGroup
{
    public EnemyStats enemyStats;
    [HideInInspector] public List<EnemyBoid> enemies;

    private new void Start()
    {
        // initialize boids
        for (int i = 0; i < transform.childCount; i++)
        {
            EnemyBoid eboid = transform.GetChild(i).GetComponent<EnemyBoid>();
            if (eboid)
            {
                AddBoid(eboid);
                eboid.Initialize(this, boidStats,enemyStats);
            }
        }

        for (int i = 0; i < spawnBoids; i++)
        {
            Vector3 position = transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, Random.Range(-spawnRadius, spawnRadius));
            InstantiateBoid(position);
        }
    }

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

        enemy.Initialize(this, boidStats,enemyStats);
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
