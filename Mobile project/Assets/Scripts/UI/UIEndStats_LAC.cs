using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIEndStats_LAC : MonoBehaviour
{
    public DiffcultySettings diffRef;
    public TextMeshProUGUI playTime, enemyKilled, building, tech;
    void Start()
    {
        EndStats_LAC.ResetStat();
    }

    public void DisplayStats()
    {
        playTime.text = "Play Time" + Time.fixedUnscaledTime;
        enemyKilled.text = "Enemies Killed " + EndStats_LAC.enemiesKilled;
        building.text = "Buildings " + EndStats_LAC.buildings;
        tech.text = "Tech discovered " + EndStats_LAC.techDiscovered + "/" + diffRef.techMax;
    }

}
