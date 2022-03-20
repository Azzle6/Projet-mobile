using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CohesionBH: SteeringBehaviour
{
    public override Vector3 VectorCalc(Boid boid)
    {
        if (boid.boidMates.Count <= 0)
            return Vector3.zero;

        Vector3 posSumm = Vector3.zero;
        for (int i = 0; i < boid.boidMates.Count; i++)
        {
            posSumm += boid.boidMates[i].transform.position;
        }
        // behavior characteristic vector calculation
        return ((posSumm / boid.boidMates.Count) - boid.transform.position);
    }
}
