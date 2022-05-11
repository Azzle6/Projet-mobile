using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 2, fileName = "ExtractorStats", menuName = "Scriptables/BuildingStat/Extractor")]
public class ExtractorSO_LAC : BuildingStatSO
{
    public GameObject visual;
    [Header("People")]
    public int maxPeople;
    [Range(0, 1)]
    public float peopleGain;

    [Header("Product")]
    public ResourceQuantity production;
    public int maxStock = 100;
    public float noise;
}
