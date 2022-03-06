using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extractor_LAC : Building
{
    public RessourceManager_LAC.RessourceType ressourceType;
    public ExtractorSO_LAC stats;
    public int people;
    float productCoolDown;

    [Header("Stock")]
    public bool fonctional = true;
    
    public float stock = 0;
    private void Start()
    {
        RessourceManager_LAC.instance.AddExtractor(this);
    }

    private void Update()
    {
        productCoolDown -= Time.deltaTime;
        if (productCoolDown < 0)
        {
            productCoolDown = 1;
            RessourceManager_LAC.instance.StockRessource(ProductCapacity(), ressourceType);
        }
        
    }

    public float ProductCapacity()
    {
       return  stats.production * (1 + (people-1) * stats.peopleGain);   
    }


    [ContextMenu("Regulation Loop")]
    public void RegulationLoop()
    {
        if (fonctional)
            RessourceManager_LAC.instance.RemoveExtractor(this);

        else if (!fonctional)
            RessourceManager_LAC.instance.AddExtractor(this);

        fonctional = !fonctional;

    }
}
