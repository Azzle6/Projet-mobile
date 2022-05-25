using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EndStats_LAC 
{
    public static int enemiesKilled;
    public static int buildings;
    public static int techDiscovered;

    public static void ResetStat()
    {
        enemiesKilled = 0;
        buildings = 0;
        techDiscovered = 0;
        Debug.Log("Reset Stats");
    }
}
