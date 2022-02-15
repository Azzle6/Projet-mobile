using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceManager_LAC : MonoBehaviour
{
    public static RessourceManager_LAC instance { get; private set; }
    public enum RessourceType { GOLD, KNOWLEDGE }

    public float gold; 

    public ExtractorData extractorData = new ExtractorData();

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

    public void AddGold(int value)
    {
        gold += value;
        extractorData.UpdateStock();
        
    }
    [System.Serializable]
    public struct ExtractorData
    {
        public List<Extractor_LAC> extractors;
        public float globalProduct;

        public ExtractorData(float globalProduct = 0)
        {
            extractors = new List<Extractor_LAC>();
            this.globalProduct = globalProduct;
        }

        public void AddExtractor(Extractor_LAC extractor)
        {
            if (extractors == null)
                extractors = new List<Extractor_LAC>();

            if (!extractors.Contains(extractor))
            {
                extractors.Add(extractor);
                globalProduct += extractor.productCapacity;
                instance.gold += extractor.stock;
            }
        }

        public void RemoveExtractor(Extractor_LAC extractor)
        {
            if (extractors.Contains(extractor))
            {
                extractors.Remove(extractor);
                globalProduct -= extractor.productCapacity;
                instance.gold -= extractor.stock;
            }
        }

        public void UpdateGlobalProduct()
        {
            globalProduct = 0;
            for (int i = 0; i < extractors.Count; i++)
                globalProduct += extractors[i].productCapacity;
        }

        public void UpdateStock()
        {
            for (int i = 0; i < extractors.Count; i++)
                extractors[i].stock = instance.gold*extractors[i].productCapacity / globalProduct;
        }

    }

}
