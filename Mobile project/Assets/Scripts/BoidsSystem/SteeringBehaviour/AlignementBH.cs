using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignementBH : SteeringBehaviour
{
    public override Vector3 VectorCalc(Boid boid)
    {
        if (boid.boidMates.Count <= 0)
            return Vector3.zero;

        Vector3 velocitySumm = Vector3.zero;
        for(int i = 0; i < boid.boidMates.Count; i++)
        {
            velocitySumm += boid.boidMates[i].velocity;
        }
        // behavior characteristic vector calculation
        return (velocitySumm /boid.boidMates.Count) - boid.velocity;
    }
}
