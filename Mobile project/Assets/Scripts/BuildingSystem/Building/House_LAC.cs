using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House_LAC : Building
{
    [HideInInspector] public HouseSO_LAC[] stats;
    public int currentPeople = 0;

    private void Start()
    {
        stats = Array.ConvertAll(statsSO, input => input as HouseSO_LAC);
        currentPeople += stats[level].peopleAdd;
        RessourceManager_LAC.instance.population += currentPeople;
        
    }

    public override void Upgrade()
    {
        if (stats.Length <= 0)
            return;

        if (BuildingScriptable.unlockedLevel > level)
        {
            base.Upgrade();
            int newPeople = stats[level].peopleAdd - currentPeople;

            RessourceManager_LAC.instance.population += newPeople;
            currentPeople = stats[level].peopleAdd;
            Debug.Log("Upgraded !");
        }
    }
}


