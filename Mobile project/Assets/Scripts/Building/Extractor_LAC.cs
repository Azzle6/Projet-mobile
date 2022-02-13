using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extractor_LAC : Building_LAC
{
    [SerializeField] [Range(1, 3)] int people;
    new void Upgrade()
    {
        base.Upgrade();
    }
}
