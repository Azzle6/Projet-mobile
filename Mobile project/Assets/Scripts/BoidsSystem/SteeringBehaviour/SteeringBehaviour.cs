using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SteeringBehaviour", menuName = "Scriptables/SteeringBehaviour_SO", order = 1)]
public abstract class SteeringBehaviour : ScriptableObject
{
    [HideInInspector]
    public BehaveGroup group;
    public float weight = 1;
    public void Initilaize(BehaveGroup group)
    {
        if(!group.presetBH.behaviours.Contains(this))
            group.presetBH.behaviours.Add(this);
        this.group = group;
    }


    /*public void OnEnable()
    {
        if (!group)
            group = GetComponent<BehaveGroup>();

        group?.behaviours.Add(this);
    }
    public void OnDisable()
    {
        group?.behaviours.Remove(this);
    }*/
}
