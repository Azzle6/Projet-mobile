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
    
    private RessourceManager_LAC ressourceM;
    [Header("Ressource")]
    public LayerMask BuildingsLayer;
    public LayerMask UILayerMask;
    public GameObject CurrentSelectedBuilding;

    [Header("Références")] 
    [SerializeField] private BuildingInfosPannel InfosPannel;
    [SerializeField] private TextMeshProUGUI matter;
    [SerializeField] private TextMeshProUGUI knowledge;
    [SerializeField] private TextMeshProUGUI pop;
    [SerializeField] private TextMeshProUGUI knowledgeTechTree;
    //[SerializeField] private GameObject BuildMenu;
    //[SerializeField] private GameObject BuildingConfirmMenu;
    //[SerializeField] private GameObject BuildingChoiceMenu;
    [SerializeField] private GameObject BuildingInfos;
    //[SerializeField] private GameObject BuildingPannelInfos;
    //[SerializeField] private GameObject MainUI;
    [SerializeField] private TextMeshProUGUI[] Texts;
    [SerializeField] private GameObject BuildingInfosUpgradeButton;
    [SerializeField] private GameObject BuildingInfosPop;


    [Header("Wave Preview")]
    [SerializeField] private Transform camT;
    [SerializeField] private GameObject wavePreview;
    [SerializeField] private Image[] waveZone = new Image[0];

    [Header("UI")] 
    [SerializeField] private Slider noiseSlider;

    private void Awake()
    {
        InfosPannel.RegisterInstance();
        if (instance != null) return;
        instance = this;
    }

    private void Start()
    {
        ressourceM = RessourceManager_LAC.instance;
        SwitchState(StateManager.State.Free);
    }

    private void Update()
    {
        UpdateWavePreview();
        //UpdateUI();
        //Debug.Log(StateManager.CurrentState);
        matter.text = Mathf.Ceil(ressourceM.matter).ToString();
        knowledge.text = Mathf.Ceil(ressourceM.knowledge).ToString();
        knowledgeTechTree.text = Mathf.Ceil(ressourceM.knowledge).ToString();
        pop.text = Mathf.Ceil(ressourceM.population).ToString();

        if ((StateManager.CurrentState != StateManager.State.DisplaceBuilding && StateManager.CurrentState != StateManager.State.HoldBuilding) && InputsManager.Click())
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
        
        ActualizeNoiseSlider();
    }

    public void SwitchState(StateManager.State newState)
    {
        StateManager.CurrentState = newState;
        UpdateUI();
    }
    
    public void SwitchState(int newState)
    {
        StateManager.CurrentState = (StateManager.State)newState;
        Debug.Log(StateManager.CurrentState);
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
            case StateManager.State.HoldBuilding :
                DisplayBuildingConfirmMenu();
                break;
            case StateManager.State.SelectBuilding :
                DisplayBuildingInfos();
                break;
            case StateManager.State.BuildingInfosPannel :
                DisplayBuildingPannel();
                break;
        }
    }
    private void UpdateWavePreview()
    {
        Vector3 worldCamdir = camT.forward;
        Vector2 uiCamDir = new Vector2(worldCamdir.x, worldCamdir.z);

        wavePreview.transform.up = uiCamDir;

        for(int i = 0; i < waveZone.Length; i++)
        {
            if (waveZone[i])
                waveZone[i].color = Color.Lerp(Color.white, Color.red, WaveManager.instance.orientedProba[i]);
            else
                Debug.LogWarning(" wave zone " + i + " is missing");
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

    public void IncreaseBuildingPop(bool increaseOrDecrease)
    {
        if(increaseOrDecrease) RessourceManager_LAC.instance.AddPop();
        else RessourceManager_LAC.instance.RemovePop();
    }

    public void UpgradeBuilding()
    {
        CurrentSelectedBuilding.GetComponentInParent<Building>().Upgrade();
    }

    public void DisplaceBuilding()
    {
        BuildingSystem.instance.Movebuilding();
    }

    private void DisplayBuildingPannel()
    {
        /*BuildingInfos.SetActive(false);
        BuildingChoiceMenu.SetActive(false);
        BuildingPannelInfos.SetActive(true);*/
        
    }

    private void DisplayBasicUI()
    {
        //MainUI.SetActive(true);
        BuildingInfos.SetActive(false);
        //BuildingConfirmMenu.SetActive(false);
    }

    private void DisplayBuildingConfirmMenu()
    {
        /*BuildingConfirmMenu.SetActive(true);
        BuildingPannelInfos.SetActive(false);
        BuildingChoiceMenu.SetActive(false);*/
    }
    
    private void DisplayBuildingChoiceMenu()
    {
        /*BuildMenu.SetActive(true);
        BuildingPannelInfos.SetActive(false);
        BuildingConfirmMenu.SetActive(false);
        MainUI.SetActive(false);
        BuildingChoiceMenu.SetActive(true);*/
    }

    private void DisplayBuildingInfos()
    {
        foreach (var txt in Texts)
        {
            txt.gameObject.SetActive(true);
        }
        BuildingInfosPop.SetActive(true);

        
        Building build = CurrentSelectedBuilding.GetComponentInParent<Building>();
        ColorBlock colors = BuildingInfosUpgradeButton.GetComponent<Button>().colors;
        if (build.level < build.BuildingScriptable.unlockedLevel)
        {
            BuildingInfosUpgradeButton.GetComponent<Button>().interactable = true;
            colors.normalColor = Color.green;
        }
        else
        {
            BuildingInfosUpgradeButton.GetComponent<Button>().interactable = false;
            colors.normalColor = Color.red;
        }
        BuildingInfosUpgradeButton.GetComponent<Button>().colors = colors;

        Extractor_LAC extractor = CurrentSelectedBuilding.GetComponentInParent<Extractor_LAC>();
        if (extractor)
        {
            Texts[0].text = "Pop : " + extractor.people;
            Texts[1].text = "Production : " + extractor.ProductCapacity() + " / s";
            Texts[2].text = "Stock : " + extractor.stock;
            Texts[3].gameObject.SetActive(false);
            Texts[4].text = extractor.BuildingScriptable.name;
        }
        else
        {
            Turret_LAC turret = CurrentSelectedBuilding.GetComponentInParent<Turret_LAC>();
            if (turret)
            {
                Texts[0].text = "Pop : ";
                Texts[1].text = "Range : " + turret.CurrentRange();
                Texts[2].text = "Damage : " + turret.CurrentDamage();
                Texts[3].text = "Attack speed : " + turret.CurrentAttackSpeed();
                Texts[4].text = turret.BuildingScriptable.name;
            }
            else
            {
                House_LAC house = CurrentSelectedBuilding.GetComponentInParent<House_LAC>();
                if (house)
                {
                    Texts[0].text = "Added pop : " + house.currentPeople;
                    for (int i = 1; i < Texts.Length; i++)
                    {
                        Texts[i].gameObject.SetActive(false);
                    }
                    BuildingInfosPop.SetActive(false);
                    
                    Texts[4].text = house.BuildingScriptable.name;
                    Texts[4].gameObject.SetActive(true);
                }
            }
            
        }

        BuildingInfos.SetActive(true);
    }
    
    #region Noise
    public void ActualizeNoiseSlider()
    {
        noiseSlider.maxValue = DiffCalculator.setting.noiseThreshold *
            (1 + DiffCalculator.setting.noiseGainPerWave * WaveManager.instance.currentWave);
        noiseSlider.value = RessourceManager_LAC.instance.noise;
    }
    #endregion
    
    public void PlayValidationSFX()
    {
        AudioManager.instance.PlaySound("UI_Validation");
    }
}
