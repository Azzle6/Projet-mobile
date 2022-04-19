using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DiffCalculator 
{
    // level
    public static int builTile;
    public static int defendTile;

    // Tech
    public static int currentTech;

    // people
    public static int currentMaxPeople;

    public static float Difficulty;
    #region Calculation
    public static float LevelCalc(DiffcultySettings setting)
    {
        if (setting.MapSize <= 0 || builTile <= 0 || (setting.buildingW + setting.defenseW) == 0)
        {
            Debug.Log("Level diff 0 exception");
            return 0;
        }
            

        return ((builTile / setting.MapSize)*setting.buildingW + (defendTile/builTile)*setting.defenseW)/(setting.buildingW+ setting.defenseW);
    }
    public static float TechCalc(DiffcultySettings setting)
    {
        if(setting.techMax <= 0)
        {
            Debug.Log("Tech diff 0 exception");
            return 0;
        }

        return currentTech / setting.techMax;
    }
    public static float PeopleCalc(DiffcultySettings setting)
    {
        if (setting.peopleMax <= 0)
        {
            Debug.Log("People diff 0 exception");
            return 0;
        }

        return Mathf.Clamp(currentMaxPeople / setting.peopleMax,0,1);
    }

    public static float DifficultyCalc(DiffcultySettings setting)
    {
        if((setting.levelW + setting.techW + setting.peopleW) <= 0)
        {
            Debug.Log(" Difficulty 0 exception");
            return 0;
        }
        Difficulty = (LevelCalc(setting) * setting.levelW + TechCalc(setting) * setting.techW + PeopleCalc(setting) * setting.peopleW) / (setting.levelW+setting.techW+setting.peopleW);
        return Difficulty;
    }
    #endregion;
}
