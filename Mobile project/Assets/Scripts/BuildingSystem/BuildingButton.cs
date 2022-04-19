using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingButton : MonoBehaviour
{
    public BuildingSO buildingSo;

    public void SwitchBuilding()
    {
        BuildingInfosPannel.instance.ChangeBuilding(buildingSo);
        UIManager_LAC.instance.SwitchState(StateManager.State.BuildingInfosPannel);
        BuildingInfosPannel.instance.gameObject.SetActive(true);
    }
}
