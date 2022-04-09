using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extractor_LAC : Building
{
    [Header("Extractor")]
    public RessourceManager_LAC.RessourceType ressourceType;
    [HideInInspector] public ExtractorSO_LAC[] stats;
    //[HideInInspector]
    public int people;
    float productCoolDown;

    
    public bool fonctional = true;
    
    public float stock = 0;
    private void Start()
    {
        RessourceManager_LAC.instance.AddExtractor(this);
        stats = Array.ConvertAll(statsSO, input => input as ExtractorSO_LAC);
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

    public void AddPop()
    {
        if (RessourceManager_LAC.instance.population <= 0 || people == stats[level].maxPeople)
            return;

        people ++;
        RessourceManager_LAC.instance.population--;
    }

    public void RemovePop()
    {
        if( people <= 1)
            return;

        people--;
        RessourceManager_LAC.instance.population++;
    }
    public float ProductCapacity()
    {
       return  stats[level].production.quantity * (1 + (people-1) * stats[level].peopleGain);   
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
