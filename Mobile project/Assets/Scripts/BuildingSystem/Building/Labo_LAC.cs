using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Labo_LAC : Building
{
    public LaboSO_LAC[] laboStats;
    public Cristal_LAC[] cristalStats;
    public int cristalLv = 0;
    private void Awake()
    {
        TechnoManager.instance.timeBoost = laboStats[0].researchBoost;
    }

    public override void Upgrade()
    {
        base.Upgrade();
        TechnoManager.instance.timeBoost = laboStats[level].researchBoost;
    }
    public void UpgradeCristal()
    {
        if (cristalStats.Length <= 0)
            return;

        cristalLv = Mathf.Clamp(cristalLv + 1, 0, cristalStats.Length);
        Debug.Log("Upgrade !");

        AudioManager.instance.PlaySound("BUILD_Upgrade");
        return;
        
        //Debug.Log("Upgrade pas");
    }
}

