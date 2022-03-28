using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WaveDifficultyManager : MonoBehaviour
{
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
    //Recuperer totalite des tourelles du jeu, leur niveau actuel pour le joueur et le niveau maximum qui existe pour elle
    
    void GlobalDifficultyCalculation()
    {
        globalDifficulty = (unlockedTechnologiesValue + stockedRessourcesValue + globalDefensePotentialValue)/ 3;
    }    
    void LocalDifficultyCalculation(int isleNbrOfTiles)
    {
        localDifficulty = (isleNbrOfTiles + noisyBuildsProportion + localDefensePotential) / 3;
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

    [Header("Noisy Proportion Parameters")]
    [HideInInspector] public int isleNoisyBuilds;
    public float isleNoisyBuildsMaxProportion;

    void LocalDefensiveProportionCalculation()
    {
        localDefensiveProportion = Mathf.Clamp01(isleTurretPlacedNbr / isleBuildsPlacedNbr * isleDefensiveMaxRatio);
    }
    void LocalDefensePotentialCalculation()
    {
        localDefensePotential = localDefensiveProportion * defenseLevel;
    }
    public void IsleSizeValueCalculation(int isleNbrOfTiles)
    {
        isleSizeValue = Mathf.Clamp01(isleNbrOfTiles / isleTileMaxNbr);
    }
    void NoisyBuildsProportionCalculation()
    {
        noisyBuildsProportion = Mathf.Clamp01(isleNoisyBuilds / isleNoisyBuildsMaxProportion * isleNoisyBuildsMaxProportion);
    }
    #endregion

    void DefensiveLevelCalculation() //A COMPLETER
    {

    }    

    [ContextMenu("Calcuate Difficulty")]
    public float WaveDifficultyCalculation()
    {
        //Parametres de la Global Difficulty
        UnlockedTechCalculation();
        StockedRessourcesCalculation();
        DefensiveLevelCalculation();
        DefensiveProportionCalculation();
        GlobalDefensePotentialCalculation();

        GlobalDifficultyCalculation();

        //Parametres de la Local Difficulty
        LocalDefensePotentialCalculation();
        LocalDefensiveProportionCalculation();
        //IsleSizeValueCalculation(); AJOUTER PARAMETRE
        NoisyBuildsProportionCalculation();

        //LocalDifficultyCalculation(); AJOUTER PARAMETRE

        float waveDifficulty = globalDifficulty * globalDifficultyInfluence + localDifficulty * (100 - globalDifficultyInfluence);

        return waveDifficulty;
    }
}
