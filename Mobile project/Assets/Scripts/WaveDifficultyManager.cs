using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WaveDifficultyManager : MonoBehaviour
{
    public static WaveDifficultyManager instance;

    [Header("Difficulty Values")]
    [SerializeField] float globalDifficulty;
    [SerializeField] float localDifficulty;

    [Header("Mob Spawn Repartition")]
    public AnimationCurve baseMobRepartition;
    public AnimationCurve tankMobRepartition;
    public AnimationCurve runnerMobRepartition;

    [Header("Difficulty Influences")]
    [SerializeField] [Range(1,99)] int globalDifficultyInfluence = 40;

    //globalDifficulty Parameters
    float unlockedTechnologiesValue;
    float stockedRessourcesValue;
    float globalDefensePotentialValue;

    //localDifficulty Parameters
    float localDefensePotential;
    float isleSizeValue;
    float noisyBuildsProportion;      

    //[Header("Defense Level Parameters")]
    //Recupérer totalité des tourelles du jeu, leur niveau actuel pour le joueur et le niveau maximum qui existe pour elle

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        
        instance = this;
    }

    public void ActualizeParameters(int islandNbr)
    {
        RessourceManager_LAC ressourceMana = RessourceManager_LAC.instance;

        //Unlocked Tech Parameters
        
        mineralsStocked = Mathf.RoundToInt(ressourceMana.matter);
        cristalsStocked = Mathf.RoundToInt(ressourceMana.knowledge);

        mineralExtractorsTotal = 0;
        cristalGeneratorsTotal = 0;
        for (int i = 0; i < ressourceMana.activeExtractor.Count; i++)
        {
            if (ressourceMana.activeExtractor[i].ressourceType == RessourceManager_LAC.RessourceType.MATTER)
            {
                mineralExtractorsTotal++;
            }
            else
            {
                cristalGeneratorsTotal++;
            }
        }
        
        //Global Defense Potential Parameters
        
        //Local Defense Potential Parameters
       
        //isleTileNbr =
        //isleBuildsCount = IslandManager.instance.IslandsList[islandNbr].BuildingsList.Count;
    }
    
    void GlobalDifficultyCalculation()
    {
        globalDifficulty = (unlockedTechnologiesValue + stockedRessourcesValue + globalDefensePotentialValue)/ 3;
    }    
    void LocalDifficultyCalculation()
    {
        localDifficulty = (isleTileNbr + noisyBuildsProportion + localDefensePotential) / 3;
    }

    #region Global Difficulty Parameters Calculation
    //Unlocked Technologies Parameters
    [HideInInspector] public int techsUnlocked;
    [HideInInspector] public int totalTechs;

    [Header("Stocked Ressources Value Parameters")]
    [HideInInspector] public int mineralsStocked;
    [HideInInspector] public int mineralExtractorsTotal;
    public float mineralImportanceMultiplicator = 0.5f;
    [HideInInspector] public int cristalsStocked;
    [HideInInspector] public int cristalGeneratorsTotal;
    public float cristalsImportanceMultiplicator = 1.5f;
    public float stockedRessourcesValueDivider = 0.002f;

    //Global Defense Potential Value Parameters
    float defensiveProportion;
    float defenseLevel;

    //Defensive proportion Parameters
    [HideInInspector] public int turretPlacedTotal;
    [HideInInspector] public int buildsPlacedTotal;
    public float defensiveMaxRatio = 0.55f;

    void UnlockedTechCalculation()
    {
        unlockedTechnologiesValue = techsUnlocked / totalTechs;
    }
    void StockedRessourcesCalculation()
    {
        stockedRessourcesValue = Mathf.Clamp01(((mineralsStocked/mineralExtractorsTotal * mineralImportanceMultiplicator) + 
            (cristalsStocked/cristalGeneratorsTotal * cristalsImportanceMultiplicator))/stockedRessourcesValueDivider);
    }
    void GlobalDefensePotentialCalculation()
    {
        globalDefensePotentialValue = (defenseLevel + defensiveProportion) / 2;
    }
    void DefensiveProportionCalculation()
    {
        defensiveProportion = Mathf.Clamp01(turretPlacedTotal/buildsPlacedTotal * defensiveMaxRatio);
    }
    #endregion
    #region Local Difficulty Parameters Calculation
    
    //Local Defense Potential Value Parameters
    [HideInInspector] public float localDefensiveProportion;

    [Header("Local Defensive Proportion Parameters")]
    [HideInInspector] public int isleTurretPlacedNbr;
    [HideInInspector] public int isleBuildsPlacedNbr;
    public float isleDefensiveMaxRatio;

    [Header("Isle Size Value Parameters")]
    [Min(1)] public int isleTileMaxNbr = 150;
    public int isleTileNbr;

    [Header("Noisy Proportion Parameters")]
    [HideInInspector] public int isleNoisyBuilds;
    [Range(0f,1f)] public float isleNoisyBuildsMaxProportion = 0.25f;
    [HideInInspector] public int isleBuildsCount;

    void LocalDefensiveProportionCalculation()
    {
        localDefensiveProportion = Mathf.Clamp01(isleTurretPlacedNbr / isleBuildsPlacedNbr * isleDefensiveMaxRatio);
    }
    void LocalDefensePotentialCalculation()
    {
        localDefensePotential = localDefensiveProportion * defenseLevel;
    }
    public void IsleSizeValueCalculation()
    {
        isleSizeValue = Mathf.Clamp01(isleTileNbr / isleTileMaxNbr);
    }
    void NoisyBuildsProportionCalculation()
    {
        noisyBuildsProportion = Mathf.Clamp01(isleNoisyBuilds / isleBuildsCount * isleNoisyBuildsMaxProportion);
    }
    #endregion

    void DefensiveLevelCalculation() //A COMPLETER
    {

    }    

    [ContextMenu("Calculate Difficulty")]
    public float WaveDifficultyCalculation()
    {
        //Parametres de la Global Difficulty
        //UnlockedTechCalculation();
        StockedRessourcesCalculation();
        DefensiveLevelCalculation();
        DefensiveProportionCalculation();
        GlobalDefensePotentialCalculation();

        GlobalDifficultyCalculation();

        //Parametres de la Local Difficulty
        LocalDefensePotentialCalculation();
        LocalDefensiveProportionCalculation();
        IsleSizeValueCalculation();
        NoisyBuildsProportionCalculation();

        LocalDifficultyCalculation();

        float waveDifficulty = globalDifficulty * globalDifficultyInfluence + localDifficulty * (100 - globalDifficultyInfluence);

        return waveDifficulty;
    }



    #region Wave Threshold
    public int WaveThresholdCalculation(int isleNbrOfTile, int isleNbrOfOccupiedTiles)
    {
        int maxThreshold = 0;

        if (isleNbrOfTile < 15)
        {
            maxThreshold = 2000;
        }
        else if (isleNbrOfTile < 20)
        {
            maxThreshold = 3500;
        }
        else if (isleNbrOfTile > 20)
        {
            maxThreshold = 7500;
        }

        int waveThreshold = isleNbrOfTile / isleNbrOfOccupiedTiles * maxThreshold;

        return waveThreshold;
    }
    #endregion
}
