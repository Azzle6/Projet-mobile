using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TechProperties : MonoBehaviour
{
    public TechState curState = TechState.Locked;
    public TechProperties[] previousButtons;
    [HideInInspector] 
    public List<TechProperties> nextButtons;
    public int techPrice;
    public float timeToDiscover;
    [HideInInspector]
    public float timeSpentToDiscover;
    public TechnoType techType;
    public BuildingSO[] concernedBuilding;

    private void Start()
    {
        foreach (var but in previousButtons)
        {
            but.nextButtons.Add(this);
        }
        UpdateVisuals();
    }

    private void OnEnable()
    {
        UpdateVisuals();
    }

    public void BuyTech() //Vérifie si la technologie peut être achetée
    {
        if (techPrice <= RessourceManager_LAC.instance.knowledge && curState == TechState.Unlocked)
        {
            TechnoManager.instance.StartDiscoveringTech(this);
        }
    }
    
    public void ApplyTech() //Apllique la technologie sur tous les bâtiments concernés
    {
        switch (techType)
        {
            case TechnoType.Upgrade :
                foreach (BuildingSO build in concernedBuilding)
                {
                    build.unlockedLevel++;
                    Debug.Log("Upgrade for " + build.name + " unlocked !");
                }
                
                break;
            case TechnoType.Unlock :
                foreach (BuildingSO build in concernedBuilding)
                {
                    build.isUnlocked = true;
                    Debug.Log("Building " + build.name + " discovered !");
                }
                BuildingsManager.instance.UpdateUnlockedBuildings();
                
                break;
        }
    }
    
    public void CheckIfUnlocked()
    {
        foreach (var but in previousButtons)
        {
            if(but.curState != TechState.Discovered) return;
        }

        SwitchState(TechState.Unlocked);
    }

    private void UpdateVisuals()
    {
        Image img = GetComponent<Image>();
        TMP_Text nameTxt = transform.GetChild(0).GetComponent<TMP_Text>();
        TMP_Text stateTxt = transform.GetChild(1).GetComponent<TMP_Text>();
        TMP_Text priceTxt = transform.GetChild(2).GetComponent<TMP_Text>();
        Slider slider = transform.GetChild(3).GetComponent<Slider>();

        nameTxt.text = techType + " " + concernedBuilding[0].name;
        stateTxt.text = curState.ToString();
        priceTxt.text = techPrice.ToString();
        slider.gameObject.SetActive(false);
        
        switch (curState)
        {
            case TechState.Locked :
                img.color = Color.grey;
                break;
            case TechState.Unlocked :
                img.color = Color.red;
                break;
            case TechState.Discovering :
                img.color = Color.blue;
                slider.gameObject.SetActive(true);
                break;
            case TechState.Discovered :
                img.color = Color.green;
                break;
        }
    }

    public void SwitchState(TechState state)
    {
        curState = state;
        UpdateVisuals();
    }

    
}

public enum TechnoType
{
    Upgrade,
    Unlock
}

public enum TechState
{
    Locked,
    Unlocked,
    Discovering,
    Discovered
}
