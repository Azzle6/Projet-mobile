using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DiffCalculator 
{
    public static DiffcultySettings setting;
    // level
    public static int builTile;
    public static int defendTile;

    // Tech
    public static int currentTech;

    // people
    public static int currentMaxPeople = 0;

    // threat
    public static int currentWave = 0;
    public static float Difficulty;

    #region Calculation
     static float LevelCalc()
    {
        if (setting.MapSize <= 0 || builTile <= 0 || (setting.buildingW + setting.defenseW) == 0)
        {
            Debug.Log("Level diff 0 exception");
            return 0;
        }
            

        return ((builTile / setting.MapSize)*setting.buildingW + (defendTile/builTile)*setting.defenseW)/(setting.buildingW+ setting.defenseW);
    }
    static float TechCalc()
    {
        if(setting.techMax <= 0)
        {
            Debug.Log("Tech diff 0 exception");
            return 0;
        }

        return currentTech / setting.techMax;
    }
    static float PeopleCalc()
    {
        if (setting.peopleMax <= 0)
        {
            Debug.Log("People diff 0 exception");
            return 0;
        }

        return Mathf.Clamp(currentMaxPeople / setting.peopleMax,0,1);
    }

    public static void DifficultyCalc()
    {
        if((setting.levelW + setting.techW + setting.peopleW) <= 0)
        {
            Debug.Log(" Difficulty 0 exception");
            return ;
        }
        Difficulty = (LevelCalc() * setting.levelW + TechCalc() * setting.techW + PeopleCalc() * setting.peopleW) / (setting.levelW+setting.techW+setting.peopleW);
    }

    public static float NoiseThreshold()
    {
        return setting.noiseThreshold * (setting.noiseGainPerWave + 1);
    }

    public static float SpawnRatio()
    {
        return setting.spawnRatio.Evaluate(Difficulty);
    }

    public static int EnemyNumber()
    {
        return Mathf.RoundToInt(setting.enemyRatio.Evaluate(Difficulty)*setting.maxEnemy);
    }

    #endregion;
}
