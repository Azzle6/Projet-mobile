using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House_LAC : Building
{
    public HouseSO_LAC[] stats;
    public int currentPeople = 0;

    private void Start()
    {
        currentPeople += stats[level].peopleAdd;
        RessourceManager_LAC.instance.population += currentPeople;
    }

    public override void Upgrade()
    {
        if (stats.Length <= 0)
            return;

        level = Mathf.Clamp(level + 1, 0, stats.Length);
        int newPeople = stats[level].peopleAdd - currentPeople;

        RessourceManager_LAC.instance.population += newPeople;
        currentPeople = stats[level].peopleAdd;
    }
}


