using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceManager_LAC : MonoBehaviour
{
    
    public static RessourceManager_LAC instance { get; private set; }

    [Header("Map")]
    public int buildTile;
    public int defendTile;

    [Header("Ressource")]
    public int population;
    float productTimer = 0;
    public enum RessourceType { MATTER, KNOWLEDGE }
    public ResourcesIcons[] resourcesIcon;
    public List<Extractor_LAC> activeExtractor;// { get; private set; }
    public float matter;// { get; private set; }
    float matterToAdd;// 
    public float maxMatter; 
        
    [HideInInspector]public float matterRatio;

    public float knowledge;// { get; private set; }
    float knowledgeToAdd;
    public float maxKnowledge; // { get; private set; }
    [HideInInspector] public float knowledgeRatio;
    public float noise;

    [Header("Tech")]
    public int currentTech;

    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        productTimer += Time.deltaTime;
        if(productTimer > 1)
        {
            productTimer = 0;
            ProductRessource();
        }
    }
    public void StockNoise(float noise)
    {
        this.noise += noise;
    }

    public bool SpendRessource(float value, RessourceType rType)
    {
        bool canSpend = true;
        // ressource value
        if (rType == RessourceType.KNOWLEDGE)
        {
            if (knowledge - value >= 0)
                knowledge -= value;
            else
                canSpend = false;
            knowledgeRatio = knowledge / maxKnowledge;
        }
            
        if (rType == RessourceType.MATTER)
        {
            if (matter - value > 0)
                matter -= value;
            else
                canSpend = false;
            matterRatio = matter / maxMatter;
        }
        if(canSpend)
            UIManager_LAC.instance.RessourceGainLossFeedback(value, rType);
        return canSpend;
    }

    public bool CanSpendResources(float value, RessourceType rType)
    {
        float matterComparison = 0;
        
        if (rType == RessourceManager_LAC.RessourceType.MATTER) matterComparison = RessourceManager_LAC.instance.matter;
        else matterComparison = RessourceManager_LAC.instance.knowledge;
        
        Debug.Log(matterComparison);
        return value < matterComparison;
    }

    public bool CanPlaceBuilding(float cost, RessourceType rType)
    {
        if (population > 0)
        {
            population--;
            return SpendRessource(cost, rType);
        }
            
        else
            return false;
        
    }
    public void ProductRessource()
    {
        // Matter ressource
        matter = Mathf.Clamp(matter + matterToAdd, 0, maxMatter);
        if(matterToAdd != 0)
            UIManager_LAC.instance.RessourceGainLossFeedback(matterToAdd, RessourceType.MATTER);
        matterToAdd = 0;

        // Knowledge ressource
        knowledge = Mathf.Clamp(knowledge + knowledgeToAdd, 0, maxKnowledge);
        if (knowledgeToAdd != 0)
            UIManager_LAC.instance.RessourceGainLossFeedback(knowledgeToAdd, RessourceType.KNOWLEDGE);
        knowledgeToAdd = 0;

        // update ratio
        knowledgeRatio = knowledge / maxKnowledge;
        matterRatio = matter / maxMatter;
    }
    public void StockRessource(float value, RessourceType rType)
    {
        // ressource value
        if (rType == RessourceType.KNOWLEDGE)
        {
            knowledgeToAdd += value;
        }
            
        if (rType == RessourceType.MATTER)
        {
            matterToAdd += value;

        }

        //UIManager_LAC.instance.RessourceGainLossFeedback(value, rType);

    }
   
    // Extractor
    float G_ProductCapacity( RessourceType rType)
    {
        float globalProduct = 0;
        for(int i = 0; i < activeExtractor.Count; i++)
        {
            if(activeExtractor[i].ressourceType == rType)
                globalProduct += activeExtractor[i].ProductCapacity();
        }
        return globalProduct;
    }
    public void AddExtractor(Extractor_LAC extractor)
    {
        if (activeExtractor == null)
            activeExtractor = new List<Extractor_LAC>();

        if (!activeExtractor.Contains(extractor))
        {
            StockRessource(extractor.stock, extractor.ressourceType);
            activeExtractor.Add(extractor);

            // update max Stock
            if (extractor.ressourceType == RessourceType.MATTER)
                maxMatter += extractor.stats[extractor.level].maxStock;

            if (extractor.ressourceType == RessourceType.KNOWLEDGE)
                maxKnowledge += extractor.stats[extractor.level].maxStock;
        }
    }

    public void RemoveExtractor(Extractor_LAC extractor)
    {
        StockRessource(-extractor.stock, extractor.ressourceType);
        activeExtractor.Remove(extractor);
        
    }

    public void AddPopBuild()
    {
        
        if (population > 0)
        {
            UIManager_LAC.instance.CurrentSelectedBuilding.GetComponentInParent<Extractor_LAC>()?.AddPop();
            UIManager_LAC.instance.CurrentSelectedBuilding.GetComponentInParent<Turret_LAC>()?.AddPop();

            Debug.Log("Add / pop : " + population);
        }
    }
    
    public void RemovePopBuild()
    {
        //Debug.Log("Remove pop");

            UIManager_LAC.instance.CurrentSelectedBuilding.GetComponentInParent<Extractor_LAC>()?.RemovePop();
            UIManager_LAC.instance.CurrentSelectedBuilding.GetComponentInParent<Turret_LAC>()?.RemovePop();
            Debug.Log("Remove / pop : " + population);
        
    }

    public Sprite GetResourceLogo(RessourceType type)
    {
        foreach (var icons in resourcesIcon)
        {
            if (icons.type == type) return icons.icon;
        }
        return null;

    }


}

[System.Serializable]
public class ResourcesIcons
{
    public RessourceManager_LAC.RessourceType type;
    public Sprite icon;
}
