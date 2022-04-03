using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 2, fileName = "ExtractorStats", menuName = "Scriptables/BuildingSO/ExtractorSO")]
public class ExtractorSO_LAC : ScriptableObject
{
    public GameObject visual;
    [Header("People")]
    public int maxPeople;
    [Range(0, 1)]
    public float peopleGain;

    [Header("Product")]
    public float production;
}
