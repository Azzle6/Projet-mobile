using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparationBH : SteeringBehaviour
{
    public override Vector3 VectorCalc(Boid boid)
    {
        Vector3 sepVec = Vector3.zero;
        for(int i= 0; i < boid.boidMates.Count; i++)
        {
            sepVec += (boid.transform.position - boid.boidMates[i].transform.position);
        }
        // behavior characteristic vector calculation
        return sepVec;
    }
}
