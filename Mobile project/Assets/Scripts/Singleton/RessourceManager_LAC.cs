using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceManager_LAC : MonoBehaviour
{
    public static RessourceManager_LAC instance { get; private set; }
   
    public int population;
    public enum RessourceType { MATTER, KNOWLEDGE }
    public List<Extractor_LAC> activeExtractor;// { get; private set; }
    public float matter;// { get; private set; }
    public float knowledge;// { get; private set; }



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

}
