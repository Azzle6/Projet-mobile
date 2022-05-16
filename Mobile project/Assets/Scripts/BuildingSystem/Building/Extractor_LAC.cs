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
    [HideInInspector] public float maxStock, attackStock;
    bool triggerWave;
    public ParticleSystem smokeFX;
    
    private void Start()
    {
        RessourceManager_LAC.instance.AddExtractor(this);
        stats = Array.ConvertAll(statsSO, input => input as ExtractorSO_LAC);
    }

    private void Update()
    {
        if (!WaveManager.instance.underAttack)
        {
            if (!fonctionnal)
                Repair();

            productCoolDown -= Time.deltaTime;
            stock = attackStock =  maxStock * ((ressourceType == RessourceManager_LAC.RessourceType.MATTER) ? RessourceManager_LAC.instance.matterRatio : RessourceManager_LAC.instance.knowledgeRatio);
        }
            

        if (productCoolDown < 0)
        {
            
            productCoolDown = 1;
            RessourceManager_LAC.instance.StockRessource(ProductCapacity(), ressourceType);
            RessourceManager_LAC.instance.StockNoise(stats[level].noise);
        }
  
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
            if (ressourceType == RessourceManager_LAC.RessourceType.MATTER)
                RessourceManager_LAC.instance.matter -= attackStock;

            if (ressourceType == RessourceManager_LAC.RessourceType.KNOWLEDGE)
                RessourceManager_LAC.instance.knowledge -= attackStock;

            stock = attackStock = 0;
            TakeDown();
        }
        
    }
    public void TakeDown()
    {
        AudioManager.instance.PlaySound("BUILD_Destroyed");
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
