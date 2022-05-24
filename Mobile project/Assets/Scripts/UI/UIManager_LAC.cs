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
    [SerializeField] private Slider matterSlider, fightMatterSlider;
    [SerializeField] private TextMeshProUGUI knowledge;
    [SerializeField] private Slider knowledgeSlider, fightKnowledgeSlider;
    [SerializeField] private TextMeshProUGUI pop;
    [SerializeField] private TextMeshProUGUI knowledgeTechTree;
    [SerializeField] private GameObject matterGainLossAnim, knowledgeGainLossAnim;
    //[SerializeField] private GameObject BuildMenu;
    //[SerializeField] private GameObject BuildingConfirmMenu;
    //[SerializeField] private GameObject BuildingChoiceMenu;
    //[SerializeField] private GameObject BuildingPannelInfos;
    //[SerializeField] private GameObject MainUI;
    
    [Header("Stats buildings InGame")]
    [SerializeField] private GameObject BuildingInfos;
    [SerializeField] private TextMeshProUGUI[] Texts;
    [SerializeField] private GameObject BuildingInfosUpgradeButton;
    [SerializeField] private TMP_Text UpgradePrice;
    [SerializeField] private Image UpgradeIcon;
    [SerializeField] private GameObject BuildingInfosUpgradeCristal;
    [SerializeField] private GameObject BuildingInfosRemoveButton;
    [SerializeField] private GameObject BuildingInfosMoveButton;
    [SerializeField] private GameObject BuildingInfosPop;


    [Header("Wave Preview")]
    [SerializeField] private Transform camT;
    [SerializeField] private GameObject wavePreview;
    [SerializeField] private Image[] waveZone = new Image[0];
    [SerializeField] private Animator wavePAnimator;
    bool showTrig;

    [Header("UI")] 
    [SerializeField] private Slider noiseSlider;
    [SerializeField] private GameObject noiseHandle;
    private float previousNoise = 0;
    public Animation anim_techCompleted;

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
        DisplayWavePreview();
        //UpdateWavePreview();
        //UpdateUI();
        //Debug.Log(StateManager.CurrentState);
        matter.text = Mathf.Ceil(ressourceM.matter).ToString();
        knowledge.text = Mathf.Ceil(ressourceM.knowledge).ToString();
        knowledgeTechTree.text = Mathf.Ceil(ressourceM.knowledge).ToString();
        pop.text = Mathf.Ceil(ressourceM.population).ToString();

        UpdateRessourcesSlider();

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
        Vector2 uiCamDir = new Vector2(-worldCamdir.x, worldCamdir.z);

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
        if(increaseOrDecrease) RessourceManager_LAC.instance.AddPopBuild();
        else RessourceManager_LAC.instance.RemovePopBuild();
        UpdateUI();
    }

    public void UpgradeBuilding()
    {
        CurrentSelectedBuilding.GetComponentInParent<Building>().Upgrade();
    }
    public void UpgradeCristal()
    {
        CurrentSelectedBuilding.GetComponent<Labo_LAC>()?.UpgradeCristal();
    }

    public void RemoveBuilding()
    {
        CurrentSelectedBuilding.GetComponentInParent<Building>().Remove();
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
        BuildingInfosRemoveButton.SetActive(true);
        BuildingInfosMoveButton.SetActive(true);
        BuildingInfosUpgradeButton.SetActive(true);

        
        Building build = CurrentSelectedBuilding.GetComponentInParent<Building>();
        ColorBlock colors = BuildingInfosUpgradeButton.GetComponent<Button>().colors;
        Debug.Log("Display building " + build.name);
        if (build.level < build.BuildingScriptable.unlockedLevel && ressourceM.CanSpendResources(build.statsSO[build.level].UpgradePrice.quantity, build.statsSO[build.level].UpgradePrice.ressource))
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
        UpgradePrice.text = "Price : " + build.statsSO[build.level].UpgradePrice.quantity;
        UpgradeIcon.sprite = ressourceM.GetResourceLogo(build.statsSO[build.level].UpgradePrice.ressource);
        
        // upgrade cristal
        if(build is Labo_LAC && BuildingInfosUpgradeCristal != null)
        {
            Labo_LAC lab = build as Labo_LAC;
            BuildingInfosUpgradeCristal.SetActive(true);
            if (ressourceM.CanSpendResources(lab.cristalStats[lab.cristalLv].UpgradePrice.quantity, lab.cristalStats[lab.cristalLv].UpgradePrice.ressource))
            {
                BuildingInfosUpgradeCristal.GetComponent<Button>().interactable = true;
                colors.normalColor = Color.green;
            }
            else
            {
                BuildingInfosUpgradeCristal.GetComponent<Button>().interactable = false;
                colors.normalColor = Color.red;
            }
        }
        else
        {
            if (BuildingInfosUpgradeCristal == null)
                Debug.LogWarning("No Upgrade cristal button");
            else
                BuildingInfosUpgradeCristal.SetActive(false);
        }
        // end upgrade cristal
        
        Extractor_LAC extractor = CurrentSelectedBuilding.GetComponentInParent<Extractor_LAC>();
        if (extractor)
        {
            Texts[0].text = "Stock : " + (int)extractor.stock +"/" +extractor.stats[extractor.level].maxStock; // stockage
            Texts[1].text = "Production : " + extractor.ProductCapacity() + " / s"; // production
            Texts[2].text =  ( extractor.people) + "/" + extractor.stats[extractor.level].maxPeople; // people
            Texts[3].text = "Bruit : " + extractor.stats[extractor.level].noise; // noise
            Texts[4].text = extractor.BuildingScriptable.name;
        }
        else
        {
            Turret_LAC turret = CurrentSelectedBuilding.GetComponentInParent<Turret_LAC>();
            if (turret)
            {
                Texts[0].text = "Damage : " + turret.CurrentDamage();
                Texts[1].text = "Range : " + turret.CurrentRange();
                Texts[2].text = turret.people + "/" + turret.stats[turret.level].maxPeople; // people
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
                // cristal modif
                else
                {
                    Labo_LAC labo = CurrentSelectedBuilding.GetComponentInParent<Labo_LAC>();
                    if (labo)
                    {
                        Texts[4].text = labo.BuildingScriptable.name;
                        BuildingInfosRemoveButton.SetActive(false);
                        BuildingInfosPop.SetActive(false);
                        BuildingInfosMoveButton.SetActive(false);
                    }
                    else
                    {
                        ELC_Rock rock = CurrentSelectedBuilding.GetComponentInParent<ELC_Rock>();
                        if (rock)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                Texts[i].gameObject.SetActive(false);
                            }
                            Texts[4].text = rock.BuildingScriptable.name;
                            BuildingInfosUpgradeButton.SetActive(false);
                            BuildingInfosPop.SetActive(false);
                            BuildingInfosMoveButton.SetActive(false);
                            
                        }
                    }
                }
                // end cristal
            }
            
        }

        BuildingInfos.SetActive(true);
    }
    
    void UpdateRessourcesSlider()
    {
        matterSlider.maxValue = ressourceM.maxMatter;
        fightMatterSlider.maxValue = matterSlider.maxValue;
        matterSlider.value = ressourceM.matter;
        fightMatterSlider.value = matterSlider.value;

        knowledgeSlider.maxValue = ressourceM.maxKnowledge;
        fightKnowledgeSlider.maxValue = knowledgeSlider.maxValue;
        knowledgeSlider.value = ressourceM.knowledge;
        fightKnowledgeSlider.value = knowledgeSlider.value;
    }

    public void RessourceGainLossFeedback(float value, RessourceManager_LAC.RessourceType ressourceType)
    {
        TextMeshProUGUI text = null;
        Animation anim = null;

        if (ressourceType == RessourceManager_LAC.RessourceType.KNOWLEDGE)
        {            
            text = knowledgeGainLossAnim.GetComponentInChildren<TextMeshProUGUI>();
            anim = knowledgeGainLossAnim.GetComponentInChildren<Animation>();

            text.text = Mathf.Ceil(value).ToString();
        }
        else if (ressourceType == RessourceManager_LAC.RessourceType.MATTER)
        {
            text = matterGainLossAnim.GetComponentInChildren<TextMeshProUGUI>();
            anim = matterGainLossAnim.GetComponentInChildren<Animation>();
            
            text.text = Mathf.Ceil(value).ToString();
        }

        if (value > 0)
        {
            text.color = Color.white;
            text.text = "+" + text.text;
        }
        else
        {
            text.color = Color.red;
            Debug.Log("Négatif");
        }

        anim.Stop();
        anim.Play();
    }

    #region Noise
    public void ActualizeNoiseSlider()
    {
        noiseSlider.maxValue = DiffCalculator.setting.noiseThreshold *
            (1 + DiffCalculator.setting.noiseGainPerWave * WaveManager.instance.currentWave);
        noiseSlider.value = RessourceManager_LAC.instance.noise;

        if (previousNoise != noiseSlider.value)
        {
            float animSpeed = (noiseSlider.value - previousNoise)/noiseSlider.maxValue * 50;
         
            noiseHandle.GetComponentInChildren<Animator>().speed = animSpeed;
            Debug.Log(animSpeed);
            
            previousNoise = RessourceManager_LAC.instance.noise;
        }
    }

    public void DisplayWavePreview(float noiseT = 0.7f)
    {
        float noiseR = noiseSlider.value / noiseSlider.maxValue;
        if (noiseR > noiseT && !showTrig)
        {
            showTrig = true;
            wavePAnimator.SetTrigger("Show");
               
        }
        if(noiseR < noiseT && showTrig)
        {
            showTrig = false;
            wavePAnimator.SetTrigger("Show");
        }

    }

    
    #endregion
    
    public void PlayValidationSFX()
    {
        AudioManager.instance.PlaySound("UI_Validation");
    }
}
