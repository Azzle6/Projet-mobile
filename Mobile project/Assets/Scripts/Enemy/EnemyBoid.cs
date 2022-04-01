using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoid : Boid
{
    EnemyStats enemyStats;
    [HideInInspector] public Transform target;
    bool inRange;
    float inRangeTime, inRangeDuration = 1;
    public enum EnemyState { MOVE, ATTACK, DIE };
    public EnemyState enemyState;

    public void Initialize(EnemyGroup enemyGroup)
    {
        enemyStats = enemyGroup.enemyStats;
        Initialize(enemyGroup as BehaveGroup);
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
                        enemyState = EnemyState.ATTACK;
                    break;
                }

            case EnemyState.ATTACK:
                {
                    if (!inRange)
                        enemyState = EnemyState.MOVE;
                    break;
                }
        }
    }
}
