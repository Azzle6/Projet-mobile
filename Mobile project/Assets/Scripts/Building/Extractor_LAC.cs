using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extractor_LAC : Building
{
    [Header("People")]
    [SerializeField] int[] maxPeople;
    [SerializeField] [Range(0, 1)] float peopleMultiplicator;
    public int people;

    public enum RessourceType { GOLD, KNOWLEDGE }
    [Header("Product")]
    public RessourceType ressourceType;
    [SerializeField] float[] productLevel;
    public float productCapacity;
    float productCoolDown;

    [Header("Stock")]
    public bool fonctional = true;
    public float stock;

    private void Start()
    {
        if (ressourceType == RessourceType.GOLD)
            RessourceManager_LAC.instance.extractor_G.Add(this);

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

 

    public void AddPeople( int people)
    {
        int maxPeople = this.maxPeople[Mathf.Clamp(UpgradeTier, 0, this.maxPeople.Length)];
        this.people = Mathf.Clamp(this.people + people, 1, maxPeople);
    }

    [ContextMenu("Update product")]
    public void UpdateProductCapacity()
    {
        productCapacity =  productLevel[UpgradeTier] * (1 + (people-1) * peopleMultiplicator);   
    }

    public void ProductRessource()
    {
        if(ressourceType == RessourceType.GOLD)
            RessourceManager_LAC.instance.AddGold((int)productCapacity);

        if (ressourceType == RessourceType.KNOWLEDGE)
            RessourceManager_LAC.instance.AddKnowledge(productCapacity);
    }

    [ContextMenu("Regulation Loop")]
    public void RegulationLoop()
    {
        if (fonctional)
        {
            RessourceManager_LAC.instance.extractor_G.Remove(this);
        }

        else if (!fonctional)
        {
            RessourceManager_LAC.instance.extractor_G.Add(this);

        }
        fonctional = !fonctional;

    }
}
