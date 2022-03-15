using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(BehaveGroup))]
public abstract class SteeringBehaviour : MonoBehaviour
{
    [HideInInspector]
    public BehaveGroup group;
    public float weight = 1;
    public abstract Vector2 VectorCalc(Boid boid);
    public void OnEnable()
    {
        if (!group)
            group = GetComponent<BehaveGroup>();

        group?.behaviours.Add(this);
    }
    public void OnDisable()
    {
        group?.behaviours.Remove(this);
    }
}
