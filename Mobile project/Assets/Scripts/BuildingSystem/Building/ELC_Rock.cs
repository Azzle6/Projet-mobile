using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELC_Rock : Building
{
    [ContextMenu("BreakRock")]
    public override void Remove()
    {
        RessourceManager_LAC.instance.SpendRessource(BuildingScriptable.price.quantity,
            BuildingScriptable.price.ressource);

        AudioManager.instance.PlaySound("BUILD_CleanRock");
        BuildingSystem.instance.RemoveBuilding(this.transform.GetChild(0).gameObject);

    }
}
