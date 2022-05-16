using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingsManager : MonoBehaviour
{
    public static BuildingsManager instance;
    public BuildingSO[] allBuildings;
    public GameObject[] preplacedBuildings;
    public GameObject buttonsParent;
    

    private void Awake()
    {
        if (instance) return;
        instance = this;
    }




    [ContextMenu("Update Buildings")]
    public void UpdateUnlockedBuildings()
    {
        foreach (Transform buttonGO in buttonsParent.transform)
        {
            buttonGO.gameObject.SetActive(false);
        }
        
        foreach (var build in allBuildings)
        {
            if (build.isUnlocked)
            {
                FindAndEnableBuilding(build);
            }
        }
    }

    public void FindAndEnableBuilding(BuildingSO building)
    {
        foreach (Transform buttonGO in buttonsParent.transform)
        {
            BuildingButton build = buttonGO.GetComponent<BuildingButton>();
            if (build.buildingSo == building)
            {
                buttonGO.gameObject.SetActive(true);
            }

        }
    }
    
}
