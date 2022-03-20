using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class UIManager_LAC : MonoBehaviour
{
    public static UIManager_LAC instance;
    
    public RessourceManager_LAC ressourceM;
    [Header("Ressource")]
    
    public LayerMask BuildingsLayer;
    public LayerMask UILayerMask;
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
        SwitchState(StateManager.State.Free);
    }

    private void Update()
    {
        UpdateUI();
        Debug.Log(StateManager.CurrentState);
        matter.text = (Mathf.Ceil(ressourceM.matter)).ToString();
        knowledge.text = (Mathf.Ceil(ressourceM.knowledge)).ToString();
        

        if ((StateManager.CurrentState != StateManager.State.DisplaceBuilding) && InputsManager.Click())
        {
            bool canSwitchSelected = true;
            
            //Pour déterminer si le joueur clique sur l'UI
            PointerEventData pointer = new PointerEventData(EventSystem.current);
            List<RaycastResult> raycastResult = new List<RaycastResult>();
            pointer.position = InputsManager.GetPosition();
            EventSystem.current.RaycastAll(pointer, raycastResult);
            
            foreach (RaycastResult result in raycastResult)
            {
                if (result.gameObject.layer == 5)
                {
                    UpdateUI();
                    canSwitchSelected = false;
                }
            }
            if(canSwitchSelected) SelectBuilding();
            
             
            
        }
    }

    public void SwitchState(StateManager.State newState)
    {
        StateManager.CurrentState = newState;
        UpdateUI();
    }

    private void UpdateUI()
    {
        switch (StateManager.CurrentState)
        {
            case StateManager.State.Free :
                DisplayBasicUI();
                break;
            case StateManager.State.ChooseBuilding :
                DisplayBuildingChoiceMenu();
                break;
            case StateManager.State.DisplaceBuilding :
                DisplayBuildingConfirmMenu();
                break;
            case StateManager.State.SelectBuilding :
                DisplayBuildingInfos();
                break;
        }
    }

    public void SelectBuilding()
    {
        Ray ray = Camera.main.ScreenPointToRay(InputsManager.GetPosition());
        if (Physics.Raycast(ray, out RaycastHit rayHit, 100, BuildingsLayer))
        {
            CurrentSelectedBuilding = rayHit.collider.gameObject;
            
            SwitchState(StateManager.State.SelectBuilding);
        }
        else
        {
            CurrentSelectedBuilding = null;
            SwitchState(StateManager.State.Free);
        }
    }

    private void DisplayBasicUI()
    {
        BuildMenu.SetActive(true);
        BuildingInfos.SetActive(false);
    }

    private void DisplayBuildingConfirmMenu()
    {
        BuildingConfirmMenu.SetActive(true);
        BuildingChoiceMenu.SetActive(false);
    }
    
    private void DisplayBuildingChoiceMenu()
    {
        BuildingConfirmMenu.SetActive(false);
        BuildingChoiceMenu.SetActive(true);
    }

    private void DisplayBuildingInfos()
    {
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
