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
    public float productCoolDown;

    [Header("Attack")]
    public bool fonctionnal;
    public float stock = 0;
    public float attackStock;
    bool triggerWave;
    public ParticleSystem smokeFX;
    
    private void Start()
    {
        RessourceManager_LAC.instance.AddExtractor(this);
        stats = Array.ConvertAll(statsSO, input => input as ExtractorSO_LAC);
        smokeFX.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!WaveManager.instance.underAttack)
        {
            if (!fonctionnal)
                Repair();

            productCoolDown -= Time.deltaTime;
            stock = attackStock = stats[level].maxStock * ((ressourceType == RessourceManager_LAC.RessourceType.MATTER) ? RessourceManager_LAC.instance.matterRatio : RessourceManager_LAC.instance.knowledgeRatio);
        }
            

        if (productCoolDown < 0)
        {
            
            productCoolDown = 1;
            RessourceManager_LAC.instance.StockRessource(ProductCapacity(), ressourceType);
            RessourceManager_LAC.instance.StockNoise(stats[level].noise);
            //stock = attackStock = stats[level].maxStock * ((ressourceType == RessourceManager_LAC.RessourceType.MATTER) ? RessourceManager_LAC.instance.matterRatio : RessourceManager_LAC.instance.knowledgeRatio);
        }
  
    }

    public override void Upgrade()
    {
        int currentStock = stats[level].maxStock;
        base.Upgrade();

        // update max Stock
        if (ressourceType == RessourceManager_LAC.RessourceType.MATTER)
            RessourceManager_LAC.instance.maxMatter += stats[level].maxStock - currentStock;

        if (ressourceType == RessourceManager_LAC.RessourceType.KNOWLEDGE)
            RessourceManager_LAC.instance.maxKnowledge += stats[level].maxStock - currentStock;
    }

    public override void Remove()
    {
        base.Remove();

        RessourceManager_LAC.instance.StockRessource(-stock, ressourceType);
            
    }

    #region Manage product
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

    
    #endregion

    #region Attack
    public void TakeDamage(int damage)
    {
        if(damage <= stock)
            stock -= damage;
        else
        {
            RessourceManager_LAC.instance.StockRessource(-attackStock, ressourceType);
            stock = attackStock = 0;
            TakeDown();
        }
        
    }
    public void TakeDown()
    {
        AudioManager.instance.PlaySound("BUILD_Destroyed");
        WaveManager.instance.BuildingCountDown();
        fonctionnal = false;
        if(smokeFX)
            smokeFX?.Play();
    }

    public void Repair()
    {
        fonctionnal = true;
        if(smokeFX)
            smokeFX?.Stop();
    }
    #endregion

}
