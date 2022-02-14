using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extractor_LAC : Building_LAC
{
    [Header("People")]
    [SerializeField] int[] maxPeople;
    [SerializeField] [Range(0, 1)] float peopleMultiplicator;
    public int people;

    [Header("Product")]
    [SerializeField] float[] productLevel;
    public float productCapacity;
    float productCoolDown;

    private void Start()
    {
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

    new void Upgrade()
    {
        base.Upgrade();
    }

    public void AddPeople( int people)
    {
        int maxPeople = this.maxPeople[Mathf.Clamp(level, 0, this.maxPeople.Length)];
        this.people = Mathf.Clamp(this.people + people, 1, maxPeople);
    }

    [ContextMenu("Update product")]
    public void UpdateProductCapacity()
    {
        productCapacity =  productLevel[level] * (1 + (people-1) * peopleMultiplicator);   
    }

    public void ProductRessource()
    {
        RessourceManager_LAC.instance.AddGold((int)productCapacity);
    }
}
