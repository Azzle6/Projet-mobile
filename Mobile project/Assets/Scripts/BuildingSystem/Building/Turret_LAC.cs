using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_LAC : Building
{
    public TurretSO_LAC[] stats;
    public EnemyBoid enemyTarget;
    public LayerMask enemyMask;
    float attackDelay = 0;

    [Header("Debug")]
    public GameObject target;
    public MeshRenderer targetRenderer;
    public Material targetAttack, targetAim;
    public void Update()
    {
        UpdateTarget();
        if (enemyTarget)
        {
            attackDelay += Time.deltaTime;
            if(attackDelay > stats[level].attackSpeed)
            {
                attackDelay = 0;
                Attack(enemyTarget);
            }

            // debug target
            target.gameObject.SetActive(true);
            target.transform.position = enemyTarget.transform.position;
        }
        else
        {
            target.gameObject.SetActive(false);
        }
            
    }

    public void UpdateTarget()
    {
        if (!enemyTarget)
        {
            float minDist = stats[level].range;
            EnemyBoid targetT = null;

            Collider[] targets = Physics.OverlapSphere(transform.position, stats[level].range, enemyMask);
            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i])
                {
                    float dist = Vector3.Distance(transform.position, targets[i].transform.position);
                    if(dist < minDist)
                    {
                        EnemyBoid enemy = targets[i].GetComponent<EnemyBoid>();
                        if (enemy)
                        {
                            minDist = dist;
                            targetT = enemy;
                        }
                    }
                }
            }
            enemyTarget = targetT;
        }
        else if (Vector3.Distance(transform.position, enemyTarget.transform.position) > stats[level].range)
            enemyTarget = null;
    }
    public void Attack(EnemyBoid enemytarget)
    {
        enemyTarget.TakeDamage(stats[level].damage);
        // debug
        targetRenderer.material = targetAttack;
        StartCoroutine(ResetTargetMat(0.5f));
    }
    public override void Upgrade()
    {
        if (stats.Length <= 0)
            return;
        level = Mathf.Clamp(level + 1, 0, stats.Length);
    }

    #region Get Stats
    public int CurrentDamage()
    {
        return stats[level].damage;
    }
    public float CurrentRange()
    {
        return stats[level].range;
    }
    public float CurrentAttackSpeed()
    {
        return stats[level].attackSpeed;
    }
    #endregion
    // debug
    IEnumerator ResetTargetMat(float delay)
    {
        yield return new WaitForSeconds(delay);
        targetRenderer.material = targetAim;

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = (enemyTarget) ?  Color.magenta : Color.green;
        Gizmos.DrawWireSphere(transform.position,stats[level].range);
    }
}
