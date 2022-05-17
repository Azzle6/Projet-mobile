using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_Rock : Building
{
    [ContextMenu("BreakRock")]
    public void BreakRock()
    {
        RessourceManager_LAC.instance.SpendRessource(BuildingScriptable.price.quantity,
            BuildingScriptable.price.ressource);
        BuildingSystem.instance.RemoveBuilding(this.gameObject);
    }
}
