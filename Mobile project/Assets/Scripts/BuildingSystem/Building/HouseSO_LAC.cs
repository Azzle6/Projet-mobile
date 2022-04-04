using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 2, fileName = "HouseStats", menuName = "Scriptables/BuildingStat/House")]
public class HouseSO_LAC : BuildingStatSO
{
        public GameObject visual;
        [Header("People")]
        public int peopleAdd;
}
