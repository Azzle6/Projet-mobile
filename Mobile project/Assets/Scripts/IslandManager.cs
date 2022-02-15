using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandManager : MonoBehaviour
{
    public static IslandManager instance;
    public Dictionary<int, Island> IslandsList = new Dictionary<int, Island>();

    private void Awake()
    {
        if (instance != null) return;

        instance = this;
    }
}