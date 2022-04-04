using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_LAC : Building
{
    TurretSO_LAC[] stats;
    [HideInInspector] public EnemyBoid enemyTarget;
    public LayerMask enemyMask;


    public void UpdateTarget()
    {

        if (!enemyTarget)
        {
            float minDist = stats[level].range;
            Transform targetT = null;

            Collider[] targets = Physics.OverlapSphere(transform.position, stats[level].range, enemyMask);
            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i])
                {
                    float dist = Vector3.Distance(transform.position, targets[i].transform.position);
                    if(dist < minDist)
                    {
                        minDist = dist;
                        targetT = targets[i].transform;
                    }
                }
            }
            enemyTarget = targetT.GetComponent<EnemyBoid>();
        }
        else if (Vector3.Distance(transform.position, enemyTarget.transform.position) > stats[level].range)
            enemyTarget = null;
    }
    public override void Upgrade()
    {
        if (stats.Length <= 0)
            return;
        level = Mathf.Clamp(level + 1, 0, stats.Length);
    }
}
