using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceManager_LAC : MonoBehaviour
{
    public static RessourceManager_LAC instance { get; private set; }
    public int gold; 
    public float knowledge;

    public List<Extractor_LAC> extractor_G, extractor_K;

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
        float globalProduct = 0;

        for(int i = 0; i < extractor_G.Count; i++)
            globalProduct += extractor_G[i].productCapacity;

        for (int i = 0; i < extractor_G.Count; i++) 
            extractor_G[i].stock +=  (float)value * extractor_G[i].productCapacity/globalProduct;
        
    }

    public void AddKnowledge(float value)
    {
        knowledge += value;
        float globalProduct = 0;

        for (int i = 0; i < extractor_K.Count; i++)
            globalProduct += extractor_K[i].productCapacity;

        for (int i = 0; i < extractor_K.Count; i++)
            extractor_K[i].stock += value * extractor_K[i].productCapacity / globalProduct;
    }

}
