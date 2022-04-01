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
        buildProd.text = buildingInf.production;
        buildPrice.text = buildingInf.price;
        buildDescription.text = buildingInf.description;
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
