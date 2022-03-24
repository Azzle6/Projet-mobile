using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 2, fileName = "HouseStats", menuName = "Scriptables/HouseSO")]
public class HouseSO_LAC : ScriptableObject
{
        public GameObject visual;
        [Header("People")]
        public int peopleAdd;
}
