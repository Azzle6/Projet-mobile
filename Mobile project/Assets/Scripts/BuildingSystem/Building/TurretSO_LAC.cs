using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 2, fileName = "TurretStats", menuName = "Scriptables/BuildingSO/TurretSO")]
public class TurretSO_LAC : ScriptableObject
{
    public GameObject visual;
    [Header("People")]
    public int maxPeople;
    [Range(0, 1)]
    public float peopleGainDamage;

    [Header("Turret")]
    public float range;
    public int damage;
    public float attackSpeed;
}
