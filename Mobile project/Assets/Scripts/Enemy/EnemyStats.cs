using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptables/EnemySettings_SO/EnemyStats", order = 1)]
public class EnemyStats : ScriptableObject
{
    public int healthPoint;
    public int damage;
    public int attackSpeed;
    public float range;
}
