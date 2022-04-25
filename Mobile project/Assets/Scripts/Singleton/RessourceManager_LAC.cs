using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceManager_LAC : MonoBehaviour
{
    public static DiffcultySettings diffPreset;
    public static RessourceManager_LAC instance { get; private set; }

    [Header("Map")]
    public int buildTile;
    public int defendTile;

    [Header("Ressource")]
    public int population;
    public enum RessourceType { MATTER, KNOWLEDGE }
    public List<Extractor_LAC> activeExtractor;// { get; private set; }
    public float matter;// { get; private set; }
    public float knowledge;// { get; private set; }
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
        // initialize difficulty preset
        DiffCalculator.setting = diffPreset;
    }
    public void StockNoise(float noise)
    {
        this.noise += noise;
        if (this.noise > DiffCalculator.NoiseThreshold())
        {
            this.noise = 0;
            DiffCalculator.DifficultyCalc();
            DiffCalculator.currentWave += 1;
        }
    }
    public void StockRessource(float value, RessourceType rType)
    {
        // ressource value
        if (rType == RessourceType.KNOWLEDGE)
            knowledge += value;
        if (rType == RessourceType.MATTER)
            matter += value;

        // update stock for all extractor
        for (int i = 0; i < activeExtractor.Count; i++)
        {
            if (activeExtractor[i].ressourceType == rType)
                activeExtractor[i].stock += value * activeExtractor[i].ProductCapacity()/G_ProductCapacity(rType);
        }
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
        }
    }

    public void RemoveExtractor(Extractor_LAC extractor)
    {
        StockRessource(-extractor.stock, extractor.ressourceType);
        activeExtractor.Remove(extractor);
        
    }

    public void AddPop()
    {
        Debug.Log("Add pop");
        if (population > 0)
        {
            UIManager_LAC.instance.CurrentSelectedBuilding.GetComponentInParent<Extractor_LAC>()?.AddPop();
            //UIManager_LAC.instance.CurrentSelectedBuilding.GetComponentInParent<Turret_LAC>()?.AddPop();
            
        }
    }
    
    public void RemovePop()
    {
        Debug.Log("Remove pop");
        if (population > 0)
        {
            UIManager_LAC.instance.CurrentSelectedBuilding.GetComponentInParent<Extractor_LAC>()?.RemovePop();
            //UIManager_LAC.instance.CurrentSelectedBuilding.GetComponentInParent<Turret_LAC>()?.RemovePop();
        }
    }


}
