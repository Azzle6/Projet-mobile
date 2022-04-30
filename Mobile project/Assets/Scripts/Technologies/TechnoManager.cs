using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechnoManager : MonoBehaviour
{
    public static TechnoManager instance;
    public bool isDiscoveringTechnology;
    public bool resetDiscoveriesOnLaunch = true;

    private void Awake()
    {
        if (instance) return;
        instance = this;
    }

    private void Start()
    {
        if(resetDiscoveriesOnLaunch) ResetAllScriptables();
        BuildingsManager.instance.UpdateUnlockedBuildings();
    }

    public void StartDiscoveringTech(TechProperties tech)
    {
        StartCoroutine(DiscoverTechnology(tech));
    }

    public IEnumerator DiscoverTechnology(TechProperties techno) //Découvre la techno après un certain temps et l'applique ensuite
    {
        if (isDiscoveringTechnology)
        {
            Debug.Log("Une technologie est déjà en cours");
            yield break;
        }

        isDiscoveringTechnology = true;
        Debug.Log("Discovering new technology !");
        
        
        techno.SwitchState(TechState.Discovering);
        Slider slider = techno.transform.GetChild(3).GetComponent<Slider>();
        slider.maxValue = techno.timeToDiscover;
        
        while (techno.timeToDiscover > techno.timeSpentToDiscover)
        {
            techno.timeSpentToDiscover += 0.1f;
            if(techno.gameObject.activeInHierarchy) slider.value = techno.timeSpentToDiscover; //Actualise le slider que si le joueur est dans le menu de techno
            yield return new WaitForSeconds(0.1f);
            yield return null;
        }
        
        Debug.Log(techno.timeSpentToDiscover);
        techno.SwitchState(TechState.Discovered);
        techno.ApplyTech();
        
        foreach (var but in techno.nextButtons) //Pour chaque technologie dépendante de celle-ci on leur fait check si toutes ses dépendances ont été débloquées pour savoir si elle s'unlock ou pas
        {
            but.CheckIfUnlocked();
        }

        isDiscoveringTechnology = false;
    }

    [ContextMenu("Reset all scriptables")]
    public void ResetAllScriptables()
    {
        foreach (var build in BuildingsManager.instance.allBuildings)
        {
            build.isUnlocked = false;
            build.unlockedLevel = 0;
        }
        Debug.LogWarning("Every buildings aren't discovered anymore, to unlock them, go to their scriptables");
    }
}