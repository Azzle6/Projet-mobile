using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoidStats", menuName = "ScriptableObjects/BoidSettings_SO", order = 1)]
public class BoidStats : ScriptableObject
{
    [Header("View")]
    public float viewDistance;
    [Range(0,360)]
    public float viewAngle;
    public LayerMask viewLayer;

    [Header("Speed")]
    public float minSpeed;
    public float maxSpeed;
}
