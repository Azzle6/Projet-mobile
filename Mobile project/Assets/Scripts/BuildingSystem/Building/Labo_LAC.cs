using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Labo_LAC : Building
{
    public LaboSO_LAC[] laboStats;

    [Header("Cristal")]
    public BuildingSO cristalB_SO;
    public int cristalLv = 0;
    public CristalSO_LAC[] cristalStats;
    public Transform cristalSocket;
    public GameObject cristalVisual;

    public bool maxCristal = false;
    int maxMatter, maxKnowledge;
    
    [Header("Upgrade")]
    public GameObject[] upgradableVisuals;
    public Material[] upgradesMat;
    
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
        
        foreach (GameObject GO in upgradableVisuals)
        {
            MeshRenderer meshRend = GO.GetComponent<MeshRenderer>();
            if (meshRend)
            {
                meshRend.material = upgradesMat[level];
            }
        }

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

        if (CanUpgradeCristal())
        {
            RessourceManager_LAC.instance.SpendRessource(cristalStats[cristalLv].UpgradePrice.quantity,
                cristalStats[cristalLv].UpgradePrice.ressource);
            cristalLv = Mathf.Clamp((cristalLv + 1), 0, cristalStats.Length - 1);
            Debug.Log("Upgrade !");

            AudioManager.instance.PlaySound("BUILD_Upgrade");
            Destroy(cristalVisual.gameObject);
            cristalVisual = Instantiate(cristalStats[cristalLv].visual, cristalSocket);

            maxCristal = (cristalLv >= cristalStats.Length - 1);
            
        }
        //Debug.Log("Upgrade pas");
    }

    public bool CanUpgradeCristal()
    {
        return (cristalLv + 1 < cristalStats.Length) && (cristalLv + 1 <= cristalB_SO.unlockedLevel);
    }
}

