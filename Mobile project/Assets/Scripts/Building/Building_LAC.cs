using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building_LAC : MonoBehaviour
{
    [SerializeField] GameObject visualPrefab;
    [SerializeField] protected int level;
    [SerializeField] int soundCost;
    bool fonctional;

    public void Upgrade()
    {
        level++;
    }
}
