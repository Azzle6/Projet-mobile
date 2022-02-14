using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceManager_LAC : MonoBehaviour
{
    public static RessourceManager_LAC instance { get; private set; }
    public int gold;
    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void AddGold(int value)
    {
        gold += value;
    }
}
