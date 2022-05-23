using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(order = 2, fileName = "LaboStats", menuName = "Scriptables/BuildingStat/Laboratory")]
public class LaboSO_LAC : BuildingStatSO
{
    public float researchBoost = 1;
    public int maxStockMatter, maxStockKnowledge;
}
