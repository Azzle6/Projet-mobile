using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIManager_LAC : MonoBehaviour
{
    public static UIManager_LAC instance;
    
    public RessourceManager_LAC ressourceM;
    [Header("Ressource")]
    public TextMeshProUGUI matter;
    public TextMeshProUGUI knowledge;
    [SerializeField]
    private GameObject BuildMenu;
    [SerializeField]
    private GameObject BuildingConfirmMenu;
    [SerializeField]
    private GameObject BuildingChoiceMenu;
    [SerializeField]
    private GameObject BuildingInfos;

    private void Awake()
    {
        if (instance != null) return;
        instance = this;
    }

    private void Start()
    {
        DisplayBuildMenu();
    }

    private void Update()
    {
        matter.text = (Mathf.Ceil(ressourceM.matter)).ToString();
        knowledge.text = (Mathf.Ceil(ressourceM.knowledge)).ToString();
    }

    public void DisplayBuildMenu()
    {
        BuildMenu.SetActive(true);
        BuildingInfos.SetActive(false);
    }

    public void DisplayBuildingConfirMenu()
    {
        BuildingConfirmMenu.SetActive(true);
        BuildingChoiceMenu.SetActive(false);
    }
    
    public void DisplayBuildingChoiceMenu()
    {
        BuildingConfirmMenu.SetActive(false);
        BuildingChoiceMenu.SetActive(true);
    }

    public void DisplayBuildingInfos()
    {
        BuildMenu.SetActive(false);
        BuildingInfos.SetActive(true);
    }
}
