using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIManager_LAC : MonoBehaviour
{
    public RessourceManager_LAC ressourceM;
    [Header("Ressource")]
    public TextMeshProUGUI matter;
    public TextMeshProUGUI knowledge;

    private void Update()
    {
        matter.text = (Mathf.Ceil(ressourceM.matter)).ToString();
        knowledge.text = (Mathf.Ceil(ressourceM.knowledge)).ToString();
    }
}
