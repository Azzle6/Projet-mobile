using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extractor_LAC : Building
{
    [Header("Extractor")]
    public RessourceManager_LAC.RessourceType ressourceType;
    public ExtractorSO_LAC[] stats;
    //[HideInInspector]
    public int people;
    float productCoolDown;

    
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

    public override void Upgrade()
    {
        if (stats.Length <= 0)
            return;
        level = Mathf.Clamp(level+1, 0, stats.Length);
    }

    public void AddPop()
    {
        people = Mathf.Clamp(people +1, 1, stats[level].maxPeople);
    }

    public void RemovePop()
    {
        people = Mathf.Clamp(people - 1, 1, stats[level].maxPeople);
    }
    public float ProductCapacity()
    {
       return  stats[level].production * (1 + (people-1) * stats[level].peopleGain);   
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
