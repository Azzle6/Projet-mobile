using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagistraProgression_UD : MonoBehaviour
{
    Image image;
    public Labo_LAC labScript;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        image.fillAmount = (float)labScript.cristalLv / labScript.cristalStats.Length;
        //print((float)labScript.cristalLv / labScript.cristalStats.Length);
    }
}
