using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House_LAC : Building
{
    public int population;
    private void Start()
    {
        RessourceManager_LAC.instance.population += population;
    }
}
