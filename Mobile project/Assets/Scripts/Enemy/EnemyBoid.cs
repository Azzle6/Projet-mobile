using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoid : Boid
{
    EnemyStats enemyStats;
    public enum EnemyState { MOVE, ATTACK, DIE };
    public EnemyState enemyState;

    // displacement
    [HideInInspector] public Transform target;
    bool inRange;
    float inRangeTime, inRangeDuration = 0.1f;

    //Attack
    float attackDelay = 0;

    [Header("Debug")]
    public MeshRenderer m_renderer;
    public Material moveMat, attackMat;

    public void Initialize(EnemyGroup enemyGroup)
    {
        enemyStats = enemyGroup.enemyStats;
        target = enemyGroup.target;
        Initialize(enemyGroup as BehaveGroup);

    }

    public void Attack()
    {
        if (target)
            Destroy(target.gameObject);
    }
    public void UpdateState()
    {
        if (target)
        {
            if (!inRange && Vector3.Distance(transform.position, target.transform.position) < enemyStats.range)
            {
                inRange = true;
                inRangeTime = Time.time;
            }
            if (Vector3.Distance(transform.position, target.transform.position) > enemyStats.range)
                inRange = false;
        }
        else
            inRange = false;

        switch (enemyState)
        {
            case EnemyState.MOVE:
                {
                    
                    if (inRange && (Time.time - inRangeTime) > inRangeDuration)
                    {
                        m_renderer.material = attackMat;
                        enemyState = EnemyState.ATTACK;
                    }
                        
                    break;
                }

            case EnemyState.ATTACK:
                {
                    
                    if (!inRange)
                    {
                        attackDelay = 0;
                        m_renderer.material = moveMat;
                        enemyState = EnemyState.MOVE;
                    }

                    if (target)
                    {
                        transform.forward = (target.position - transform.position).normalized;
                        attackDelay += Time.deltaTime;

                        if(attackDelay > enemyStats.attackSpeed)
                        {
                            attackDelay = 0;
                            Attack();
                        }
                        
                    }
                        
                    break;
                }
        }
    }
}
