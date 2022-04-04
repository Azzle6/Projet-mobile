using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class BuildingInfosPannel : MonoBehaviour
{
    [SerializeField] private Image buildImage;
    [SerializeField] private TMP_Text buildName, buildProd, buildPrice, buildDescription;
    public BuildingSO buildingInf;
    
    private void OnEnable()
    {
        UpdateUI();

    }

    private void UpdateUI()
    {
        buildImage.sprite = buildingInf.image;
        buildName.text = buildingInf.name;
        
        buildPrice.text = "Price : " + buildingInf.price.quantity + " " + buildingInf.price.ressource;
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


    }

    public void SpawnCurrentBuilding()
    {
        BuildingSystem.instance.SpawnBuilding(buildingInf.prefab);
    }

    public void ChangeBuilding(BuildingSO newBuild)
    {
        buildingInf = newBuild;
        UpdateUI();

    }
}
