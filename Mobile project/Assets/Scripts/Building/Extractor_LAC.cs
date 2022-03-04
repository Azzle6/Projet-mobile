using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extractor_LAC : Building
{
    [Header("People")]
    [SerializeField] int[] maxPeople;
    [SerializeField] [Range(0, 1)] float peopleMultiplicator;
    public int people;

    public enum ProductType { MATERIAL,KNOWLEDGE}
    delegate float RessourceDelegate(RessourceManager_LAC ressource);

    [Header("Product")]
    public ProductType productType;
    [SerializeField] float[] productLevel;
    public float productCapacity;
    float productCoolDown;

    [Header("Stock")]
    public bool fonctional = true;
    public float stock;

    private void Start()
    {
        people = 1;
        RessourceManager_LAC.instance.extractorData.AddExtractor(this);
        UpdateProductCapacity();
    }

    private void Update()
    {
        productCoolDown -= Time.deltaTime;
        if (productCoolDown < 0)
        {
            productCoolDown = 1;
            ProductRessource();
        }
        
    }

 
    public int MaxPeople()
    {
        return maxPeople[Mathf.Clamp(UpgradeTier, 0, this.maxPeople.Length)];
    }

    [ContextMenu("Update product")]
    public void UpdateProductCapacity()
    {
        productCapacity =  productLevel[UpgradeTier] * (1 + (people-1) * peopleMultiplicator);   
    }

    float RessourceType()
    {
        if (productType == ProductType.MATERIAL)
            return RessourceManager_LAC.instance.ressource;

        if (productType == ProductType.KNOWLEDGE)
            return RessourceManager_LAC.instance.knowledge;

        return 0;
    }
    public void ProductRessource()
    {
            RessourceManager_LAC.instance.AddRessource(productCapacity,productType);
    }

    [ContextMenu("Regulation Loop")]
    public void RegulationLoop()
    {
        if (fonctional)
            RessourceManager_LAC.instance.extractorData.RemoveExtractor(this);

        else if (!fonctional)
            RessourceManager_LAC.instance.extractorData.AddExtractor(this);

        fonctional = !fonctional;

    }
}
