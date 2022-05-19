using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Labo_LAC : Building
{
    public LaboSO_LAC[] laboStats;
    public CristalSO_LAC[] cristalStats;
    public Transform cristalSocket;
    GameObject cristalVisual;

    public int cristalLv = 0;
    int maxMatter, maxKnowledge;
    private void Start()
    {
        cristalVisual = Instantiate(cristalStats[0].visual, cristalSocket);
        maxMatter = laboStats[0].maxStockMatter;
        maxKnowledge = laboStats[0].maxStockKnowledge;

        TechnoManager.instance.timeBoost = laboStats[0].researchBoost;
        RessourceManager_LAC.instance.maxKnowledge += maxKnowledge;
        RessourceManager_LAC.instance.maxMatter += maxMatter;
    }

    public override void Upgrade()
    {
        base.Upgrade();
        TechnoManager.instance.timeBoost = laboStats[level].researchBoost;

        // upgrade max stock
        RessourceManager_LAC.instance.maxKnowledge += (laboStats[level].maxStockKnowledge - maxKnowledge);
        RessourceManager_LAC.instance.maxMatter += (laboStats[level].maxStockMatter - maxMatter);

        maxMatter = laboStats[level].maxStockMatter;
        maxKnowledge = laboStats[level].maxStockKnowledge;

    }
    public void UpgradeCristal()
    {
        if (cristalStats.Length <= 0)
            return;

        cristalLv = Mathf.Clamp(cristalLv + 1, 0, cristalStats.Length);
        Debug.Log("Upgrade !");

        AudioManager.instance.PlaySound("BUILD_Upgrade");
        Destroy(cristalVisual);
        cristalVisual = Instantiate(cristalStats[level].visual, cristalSocket);
        
        //Debug.Log("Upgrade pas");
    }
}

