using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

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
    [SerializeField] private TextMeshProUGUI matter, fightMatter;
    [SerializeField] private Slider matterSlider, fightMatterSlider;
    [SerializeField] private TextMeshProUGUI knowledge, fightKnowledge;
    [SerializeField] private Slider knowledgeSlider, fightKnowledgeSlider;
    [SerializeField] private TextMeshProUGUI pop, fightPop;
    [SerializeField] private TextMeshProUGUI knowledgeTechTree;
    [SerializeField] private GameObject matterGainLossAnim, knowledgeGainLossAnim;

    public Button confirmPlacementButton;
    //[SerializeField] private GameObject BuildMenu;
    //[SerializeField] private GameObject BuildingConfirmMenu;
    //[SerializeField] private GameObject BuildingChoiceMenu;
    //[SerializeField] private GameObject BuildingPannelInfos;
    //[SerializeField] private GameObject MainUI;

    [Header("Stats buildings InGame")]
    [SerializeField] private GameObject menuBatiment;
    [SerializeField] private GameObject statAchatBatiment;
    [SerializeField] private GameObject BuildingInfos;
    [SerializeField] private GameObject boutonMenuBatiments;
    [SerializeField] private TextMeshProUGUI[] Texts;
    [SerializeField] private GameObject BuildingInfosUpgradeButton;
    [SerializeField] private TMP_Text UpgradeCristalPrice;
    [SerializeField] private Image UpgradeCristalIcon;
    [SerializeField] private TMP_Text UpgradePrice;
    [SerializeField] private Image UpgradeIcon;
    [SerializeField] private TMP_Text RemovePrice;
    [SerializeField] private Image RemoveIcon;
    [SerializeField] private GameObject BuildingInfosUpgradeCristal;
    [SerializeField] private GameObject BuildingInfosRemoveButton;
    [SerializeField] private GameObject BuildingInfosMoveButton;
    [SerializeField] private GameObject BuildingInfosPop;
    [SerializeField] private TMP_Text levelDisplayText;
    [SerializeField] private GameObject BuildingInfosLevelDisplay;


    [Header("Wave Preview")]
    [SerializeField] private Transform camT;
    [SerializeField] private GameObject wavePreview;
    [SerializeField] private Image[] waveZone = new Image[0];
    [SerializeField] private Animator wavePAnimator;
    bool showTrig;
    [SerializeField] private TMP_Text enemiesCount;

    [Header("UI")] 
    [SerializeField] private Slider noiseSlider;
    [SerializeField] private GameObject noiseHandle;
    [SerializeField] private Animator noiseTextAnimator;
    private float previousNoise = 0;
    public Animation anim_techCompleted;

    [Header("End")]
    public UIEndStats_LAC endScreen;
    public GameObject startDialogue, endDialogue;
    bool startTrig = true, endTrig;

    [Header("Sound")]
    [SerializeField] private GameObject soundButton;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Sprite soundOnSprite, soundOffSprite;
    bool onOff = true;
    [SerializeField]private float touchDuration;
    private bool isTouching;

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
        // end condition
        if (WaveManager.gameOver && !endTrig)
        {
            endTrig = true;
            endScreen.DisplayStats(false);
        }
            

        // start dialogue
        if(startTrig)
        {
            startTrig = false;
            startDialogue.SetActive(true);
        }

        if (InputsManager.Click()) touchDuration = 0;
        else if (InputsManager.IsDown()) touchDuration += Time.deltaTime;
        
        //DisplayWavePreview();
        //UpdateWavePreview();
        //UpdateUI();
        //Debug.Log(StateManager.CurrentState);
        matter.text = Mathf.Ceil(ressourceM.matter).ToString();
        fightMatter.text = Mathf.Ceil(ressourceM.matter).ToString();
        knowledge.text = Mathf.Ceil(ressourceM.knowledge).ToString();
        fightKnowledge.text = Mathf.Ceil(ressourceM.knowledge).ToString();
        knowledgeTechTree.text = Mathf.Ceil(ressourceM.knowledge).ToString();
        pop.text = Mathf.Ceil(ressourceM.population).ToString();
        fightPop.text = Mathf.Ceil(ressourceM.population).ToString();

        UpdateRessourcesSlider();

        if ((StateManager.CurrentState != StateManager.State.DisplaceBuilding && StateManager.CurrentState != StateManager.State.HoldBuilding && StateManager.CurrentState != StateManager.State.ChooseBuilding && StateManager.CurrentState != StateManager.State.BuildingInfosPannel) && InputsManager.Release() && (Input.touchCount == 1 || Input.GetMouseButtonUp(0) ) && touchDuration < 0.25f)
        {
            //Debug.Log("detection");
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
        Debug.Log(StateManager.CurrentState);
        UpdateUI();
    }

    public void PauseGame(bool pause)
    {
        if (pause) Time.timeScale = 0;
        else Time.timeScale = 1;
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
            GameObject lastSelectedBuilding = CurrentSelectedBuilding;
            CurrentSelectedBuilding = rayHit.collider.gameObject;
            ;
            if (lastSelectedBuilding != CurrentSelectedBuilding)
            {
                if (lastSelectedBuilding)
                {
                    Building lastBuild = lastSelectedBuilding.GetComponentInParent<Building>();
                    if (lastBuild)
                        Destroy(lastBuild.selectVFX);

                    Turret_LAC lastTurret = lastBuild as Turret_LAC;
                    if (lastTurret)
                        lastTurret.rangeOrigin.gameObject.SetActive(false);
                }
                
                Building build = CurrentSelectedBuilding.GetComponentInParent<Building>();
                GameObject vfx = build.BuildingScriptable.PlacementVFX;
                if (vfx)
                    build.selectVFX = Instantiate(vfx, build.transform.GetChild(0).transform);

                Turret_LAC turret = build as Turret_LAC;
                if (turret)
                    turret.rangeOrigin.gameObject.SetActive(true);
            }
            
            
            SwitchState(StateManager.State.SelectBuilding);

            // show vfx
            
        }
        else
        {
            // hide sfx
            if (CurrentSelectedBuilding)
            {
                Building build = CurrentSelectedBuilding.GetComponentInParent<Building>();
                if (build)
                    Destroy(build.selectVFX);

                Turret_LAC turret = build as Turret_LAC;
                if (turret)
                    turret.rangeOrigin.gameObject.SetActive(false);
            }

            CurrentSelectedBuilding = null;
            SwitchState(StateManager.State.Free);

            // end game condition
            if (endTrig)
            {
                endTrig = false;
                endDialogue.SetActive(true);
            }
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
        UpdateUI();
    }
    public void UpgradeCristal()
    {
        Debug.Log("Upgrade Cristal");
        Labo_LAC lab = CurrentSelectedBuilding.GetComponentInParent<Labo_LAC>();
        lab.UpgradeCristal();
        endTrig = lab.maxCristal;

        UpdateUI();
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
        BuildingInfos.SetActive(false);
        /*BuildingChoiceMenu.SetActive(false);
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
        BuildingInfosRemoveButton.GetComponent<Button>().interactable = true;
        BuildingInfosUpgradeCristal.SetActive(false);
        BuildingInfosMoveButton.SetActive(true);
        BuildingInfosUpgradeButton.SetActive(true);
        BuildingInfosLevelDisplay.gameObject.SetActive(true);

        if (RemovePrice)
        {
            RemovePrice.gameObject.SetActive(false);
            RemoveIcon.gameObject.SetActive(false);
        }
        

        
        Building build = CurrentSelectedBuilding.GetComponentInParent<Building>();
        ColorBlock colors = BuildingInfosUpgradeButton.GetComponent<Button>().colors;
        //Debug.Log("Display building " + build.name);
        if (build.level < build.BuildingScriptable.unlockedLevel && ressourceM.CanSpendResources(build.statsSO[build.level].UpgradePrice.quantity, build.statsSO[build.level].UpgradePrice.ressource))
        {
            BuildingInfosUpgradeButton.GetComponent<Button>().interactable = true;
            
            colors.normalColor = Color.green;
        }
        else
        {
            if(build.level >= build.BuildingScriptable.unlockedLevel) BuildingInfosUpgradeButton.SetActive(false);
            BuildingInfosUpgradeButton.GetComponent<Button>().interactable = false;
            colors.normalColor = Color.red;
        }
        BuildingInfosUpgradeButton.GetComponent<Button>().colors = colors;
        UpgradePrice.text = "Price : " + build.statsSO[build.level].UpgradePrice.quantity;
        UpgradeIcon.sprite = ressourceM.GetResourceLogo(build.statsSO[build.level].UpgradePrice.ressource);
        
        //Display lvl
        int lvl = build.level + 1;
        levelDisplayText.text = lvl.ToString();
        
        // upgrade cristal
        if(build is Labo_LAC && BuildingInfosUpgradeCristal != null)
        {
            Labo_LAC lab = build as Labo_LAC;
            BuildingInfosUpgradeCristal.SetActive(lab.CanUpgradeCristal());
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
            Texts[3].text = "Noise : " + extractor.stats[extractor.level].noise * (1 + (extractor.people - 1) * extractor.stats[extractor.level].peopleNoise); // noise
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
                        Texts[1].text = "Research Boost x" + labo.laboStats[labo.level].researchBoost; 
                        Texts[0].text = "Matter Stock +" + labo.laboStats[labo.level].maxStockMatter; 
                        Texts[3].text = "Crystals Stock +" + labo.laboStats[labo.level].maxStockKnowledge; 
                        Texts[4].text = labo.BuildingScriptable.name;
                        
                        UpgradeCristalPrice.text = ""+(int)labo.cristalStats[labo.cristalLv].UpgradePrice.quantity;
                        UpgradeIcon.sprite = ressourceM.GetResourceLogo(labo.cristalStats[labo.cristalLv].UpgradePrice.ressource);
                        
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
                            BuildingInfosRemoveButton.GetComponent<Button>().interactable =
                                ressourceM.CanSpendResources(build.BuildingScriptable.price.quantity,
                                    build.BuildingScriptable.price.ressource);
                            RemovePrice.gameObject.SetActive(true);
                            RemoveIcon.gameObject.SetActive(true);
                            BuildingInfosLevelDisplay.gameObject.SetActive(false);
                            if (RemovePrice)
                            {
                                RemovePrice.text = build.BuildingScriptable.price.quantity.ToString();
                                RemoveIcon.sprite = ressourceM.GetResourceLogo(build.BuildingScriptable.price.ressource);
                            }
                            


                        }
                    }
                }
                // end cristal
            }
            
        }
        if(WaveManager.instance.underAttack) BuildingInfosMoveButton.SetActive(false);
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

    public void CloseBuyMenu()
    {
        statAchatBatiment.SetActive(false);
    }

    public void CloseBuildingInfos()
    {
        BuildingInfos.SetActive(false);
    }

    public void CloseBuildingMenu()
    {
        menuBatiment.SetActive(false);
        boutonMenuBatiments.SetActive(true);
    }


    #region Noise
    public void ActualizeNoiseSlider()
    {
        noiseSlider.maxValue = DiffCalculator.setting.noiseThreshold *
            (1 + DiffCalculator.setting.noiseGainPerWave * WaveManager.instance.currentWave);
        noiseSlider.value = RessourceManager_LAC.instance.noise;

        if (previousNoise != noiseSlider.value)
        {
            if (noiseTextAnimator.GetBool("Noise") == false) noiseTextAnimator.SetBool("Noise", true);
            float animSpeed = (noiseSlider.value - previousNoise)/noiseSlider.maxValue * 50;
         
            noiseHandle.GetComponentInChildren<Animator>().speed = animSpeed;
            noiseTextAnimator.speed = animSpeed;
            
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
            UpdateWavePreview();
               
        }
        if(noiseR < noiseT && showTrig)
        {
            showTrig = false;
            wavePAnimator.SetTrigger("Show");
        }

    }

    public void SetEnemiesCount(int enemies)
    {
        enemiesCount.text = enemies + " enemies";
    }
    
    #endregion
    
    public void ChangeAudioStatus()
    {
        if(!onOff)
        {
            soundButton.GetComponent<Image>().sprite = soundOnSprite;
            audioMixer.SetFloat("MasterVolume",0);
            onOff = true;
        }
        else
        {
            soundButton.GetComponent<Image>().sprite = soundOffSprite;
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(-80));
            onOff = false;
        }
    }

    public void PlayValidationSFX()
    {
        AudioManager.instance.PlaySound("UI_Validation");
    }
}
