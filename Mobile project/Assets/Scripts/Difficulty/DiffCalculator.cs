using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DiffCalculator
{
    public static DiffcultySettings setting;
    public static float Difficulty;
    // debug
    public static float levelDiff, techDiff, ressourceDiff;

    #region Calculation
    static DifficultyData LevelCalc()
    {
        int buildTile = RessourceManager_LAC.instance.buildTile;
        int defendTile = RessourceManager_LAC.instance.defendTile;

        DifficultyData buildDif = new DifficultyData(((float)buildTile / setting.MapSize), setting.buildingW);
        DifficultyData defendDif = new DifficultyData((buildTile!= 0)?((float)defendTile / buildTile) : 0, setting.defenseW);
        List<DifficultyData> diffs = new List<DifficultyData>{buildDif, defendDif};

        levelDiff = (SummDiffData(diffs).value / SummDiffData(diffs).weight);
        return new DifficultyData(SummDiffData(diffs).value / SummDiffData(diffs).weight, setting.levelW);
    }
    static DifficultyData TechCalc()
    {
        DifficultyData diff = new DifficultyData(0, setting.techW);
        if (setting.techMax <= 0)
        {
            Debug.Log("Tech diff 0 exception");
            return diff;
        }
        
        diff.value = RessourceManager_LAC.instance.currentTech / setting.techMax;
        techDiff = diff.value;
        return diff;
    }
    static DifficultyData RessourceCalc()
    {
        DifficultyData diffData = new DifficultyData(0, setting.ressourceW);
        DifficultyData matterDiff = new DifficultyData(Mathf.Clamp(RessourceManager_LAC.instance.matter / setting.maxMatter, 0, 1), setting.matterW);
        DifficultyData knowlegeDiff = new DifficultyData(Mathf.Clamp(RessourceManager_LAC.instance.knowledge / setting.maxKnowledge, 0, 1), setting.knowledgeW);

        List<DifficultyData> diffs = new List<DifficultyData> { matterDiff, knowlegeDiff };
        DifficultyData diffSumm = SummDiffData(diffs);
        diffData.value = diffSumm.value / diffSumm.weight;

        ressourceDiff = diffData.value;
        return diffData;
    }


    public static float DifficultyCalc()
    {
        List<DifficultyData> diffs = new List<DifficultyData> { LevelCalc(), TechCalc(), RessourceCalc() };
        DifficultyData diff = SummDiffData(diffs);
        Difficulty = (diff.value / diff.weight);
        return Difficulty;
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
        return Mathf.CeilToInt(setting.enemyRatio.Evaluate(Difficulty) * setting.maxEnemy+0.01f);
    }

    #endregion;
    struct DifficultyData
    {
        public float value, weight;
        public DifficultyData(float value, float weight)
        {
            this.value = value;
            this.weight = weight;
        }
    }
    static DifficultyData SummDiffData(List<DifficultyData> data)
    {
        float summValue = 0;
        float summWeight = 0;
        DifficultyData diffData = new DifficultyData(0, 0);
        
        foreach(DifficultyData d in data)
        {
            summValue += (d.value * d.weight);
            summWeight += d.weight;
        }

        diffData.value = summValue;
        diffData.weight = summWeight;
        return diffData;
    }
}


   
