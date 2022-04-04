using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 2, fileName = "TurretStats", menuName = "Scriptables/BuildingStat/Turret")]
public class TurretSO_LAC : BuildingStatSO
{
    public GameObject visual;
    [Header("People")]
    public int maxPeople;
    [Range(0, 1)]
    public float peopleGainDamage, peopleGainAttackSpeed;

    [Header("Turret")]
    public float range;
    public int damage;
    public float attackSpeed;
}
