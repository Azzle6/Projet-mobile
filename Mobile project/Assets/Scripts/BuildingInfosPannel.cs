using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class BuildingInfosPannel : MonoBehaviour
{
    public static BuildingInfosPannel instance;
    [SerializeField] private Image buildImage;
    [SerializeField] private TMP_Text buildName, buildProd, buildPrice, buildDescription;
    [SerializeField] private Image priceIcon;
    [SerializeField] private Button confirmButton;
    public BuildingSO buildingInf;


    public void RegisterInstance()
    {
        if (instance) return;
        instance = this;
    }
    
    private void OnEnable()
    {
        UpdateUI();

    }

    private void UpdateUI()
    {
        buildImage.sprite = buildingInf.image;
        buildName.text = buildingInf.name;
        
        buildPrice.text = "Price : " + buildingInf.price.quantity;
        priceIcon.sprite = RessourceManager_LAC.instance.GetResourceLogo(buildingInf.price.ressource);
        buildDescription.text = buildingInf.description;

        if (buildingInf.buildingStats.GetType() == typeof(ExtractorSO_LAC))
        {
            ExtractorSO_LAC stats = (ExtractorSO_LAC) buildingInf.buildingStats;
            buildProd.text = "Production : " + stats.production.quantity + "/s " + stats.production.ressource;
        }
        else if (buildingInf.buildingStats.GetType() == typeof(HouseSO_LAC))
        {
            HouseSO_LAC stats = (HouseSO_LAC) buildingInf.buildingStats;
            buildProd.text = "Pop added : " + stats.peopleAdd;
        }
        else if(buildingInf.buildingStats.GetType() == typeof(TurretSO_LAC))
        {
            TurretSO_LAC stats = (TurretSO_LAC) buildingInf.buildingStats;
            buildProd.text = "Range : " + stats.range + "\nDamage : " + stats.damage + "\nAttack speed : " + stats.attackSpeed;
        }

        float matterComparison = 0;
        
        if (buildingInf.price.ressource == RessourceManager_LAC.RessourceType.MATTER) matterComparison = RessourceManager_LAC.instance.matter;
        else matterComparison = RessourceManager_LAC.instance.knowledge;
        
        Debug.Log(matterComparison);
        confirmButton.interactable = buildingInf.price.quantity < matterComparison;

    }

    public void SpawnCurrentBuilding()
    {
        BuildingSystem.instance.SpawnBuilding(buildingInf.prefab);
    }

    public void ChangeBuilding(BuildingSO newBuild)
    {
        buildingInf = newBuild;
        //Debug.Log(newBuild.name);
        UpdateUI();

    }
}
