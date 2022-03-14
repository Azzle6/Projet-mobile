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
    
    public LayerMask BuildingsLayer;
    public GameObject CurrentSelectedBuilding;
    
    [Header("Références")]
    [SerializeField] private TextMeshProUGUI matter;
    [SerializeField] private TextMeshProUGUI knowledge;
    [SerializeField] private GameObject BuildMenu;
    [SerializeField] private GameObject BuildingConfirmMenu;
    [SerializeField] private GameObject BuildingChoiceMenu;
    [SerializeField] private GameObject BuildingInfos;
    [SerializeField] private TextMeshProUGUI SelectedBuildingCurrentPop;
    [SerializeField] private TextMeshProUGUI SelectedBuildingProduction;
    [SerializeField] private TextMeshProUGUI SelectedBuildingStockage;

    private void Awake()
    {
        if (instance != null) return;
        instance = this;
    }

    private void Start()
    {
        DisplayBasicUI();
    }

    private void Update()
    {
        Debug.Log(StateManager.CurrentState);
        matter.text = (Mathf.Ceil(ressourceM.matter)).ToString();
        knowledge.text = (Mathf.Ceil(ressourceM.knowledge)).ToString();

        if ((StateManager.CurrentState != StateManager.State.DisplaceBuilding) && InputsManager.Click())
        {
            Ray ray = Camera.main.ScreenPointToRay(InputsManager.GetPosition());
            if (Physics.Raycast(ray, out RaycastHit rayHit,100, BuildingsLayer))
            {
                CurrentSelectedBuilding = rayHit.collider.gameObject;
                Debug.Log("oui");
                DisplayBuildingInfos();
            }
            else
            {
                CurrentSelectedBuilding = null;
                DisplayBasicUI();
            }
        }
    }

    public void DisplayBasicUI()
    {
        StateManager.CurrentState = StateManager.State.Free;
        BuildMenu.SetActive(true);
        BuildingInfos.SetActive(false);
    }

    public void DisplayBuildingConfirmMenu()
    {
        StateManager.CurrentState = StateManager.State.DisplaceBuilding;
        BuildingConfirmMenu.SetActive(true);
        BuildingChoiceMenu.SetActive(false);
    }
    
    public void DisplayBuildingChoiceMenu()
    {
        StateManager.CurrentState = StateManager.State.ChooseBuilding;
        BuildingConfirmMenu.SetActive(false);
        BuildingChoiceMenu.SetActive(true);
    }

    public void DisplayBuildingInfos()
    {
        StateManager.CurrentState = StateManager.State.SelectBuilding;
        Extractor_LAC extractor = CurrentSelectedBuilding.GetComponentInParent<Extractor_LAC>();
        if (extractor)
        {
            SelectedBuildingCurrentPop.text = extractor.people.ToString();
            SelectedBuildingProduction.text = extractor.ProductCapacity().ToString();
            SelectedBuildingStockage.text = extractor.stock.ToString();
        }
        
        
        
        BuildMenu.SetActive(false);
        BuildingInfos.SetActive(true);
    }
}
