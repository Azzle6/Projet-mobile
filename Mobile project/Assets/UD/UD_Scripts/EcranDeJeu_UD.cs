using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcranDeJeu_UD : MonoBehaviour
{
    WaveManager waveManager;

    bool inFight;
    Animator anim;

    void Start()
    {
        waveManager = WaveManager.instance;
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        inFight = waveManager.underAttack;
        anim.SetBool("fight",inFight);
    }
}
