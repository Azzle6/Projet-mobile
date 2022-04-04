using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BehavioursPreset", menuName = "Scriptables/BehavioursPreset_SO", order = 1)]
public class BehavioursPreset : ScriptableObject
{
    public List<SteeringBehaviour> behaviours;
}
