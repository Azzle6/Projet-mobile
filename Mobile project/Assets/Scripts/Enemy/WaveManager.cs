using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;
    public WaveData data;
    void Awake()
    {
        if (instance != this && instance)
            Destroy(this);
        else
            instance = this;
    }
}
