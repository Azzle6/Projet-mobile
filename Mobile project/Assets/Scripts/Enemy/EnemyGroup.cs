using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : BehaveGroup
{
    public EnemyStats eStats;
    [HideInInspector] public List<Enemy> enemies;
    private void Awake()
    {
        child = this;
    }

    private new void Start()
    {
        base.Start();
        

    }




}
