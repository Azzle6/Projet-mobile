using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StateManager
{
    public enum State
    {
        DisplaceBuilding,
        ChooseBuilding,
        SelectBuilding,
        Free,
        BuildingInfosPannel,
    }

    public static State CurrentState = State.Free;
}
