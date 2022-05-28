using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIEndStats_LAC : MonoBehaviour
{
    public DiffcultySettings diffRef;
    public GameObject victory, defeat;
    //public Animation anim;
    [HideInInspector] public bool win;
    [Header("Stats")]
    public TextMeshProUGUI playTime;
    public TextMeshProUGUI enemyKilled, building, tech;


    public void DisplayStats( bool win)
    {
        gameObject.SetActive(true);

        victory.SetActive(win);
        defeat.SetActive(!win);

        playTime.text = "Play Time" + (int)(Time.fixedUnscaledTime/60) +" min";
        enemyKilled.text = "Enemies Killed " + EndStats_LAC.enemiesKilled;
        building.text = "Buildings " + EndStats_LAC.buildings;
        tech.text = "Tech discovered " + EndStats_LAC.techDiscovered + "/" + diffRef.techMax;

        //anim.Play();
    }

}
