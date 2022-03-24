using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparationBH : SteeringBehaviour
{
    public override Vector3 VectorCalc(Boid boid)
    {
        if (boid.boidMates.Count <= 0)
            return Vector3.zero;

        Vector3 sepVec = Vector3.zero;
        for(int i= 0; i < boid.boidMates.Count; i++)
        {
            float dist = Vector3.Distance(boid.transform.position, boid.boidMates[i].transform.position);
            if(dist != 0)
                sepVec += (boid.transform.position - boid.boidMates[i].transform.position)/dist;
        }
        // behavior characteristic vector calculation
        return (sepVec/ boid.boidMates.Count);
    }
}
