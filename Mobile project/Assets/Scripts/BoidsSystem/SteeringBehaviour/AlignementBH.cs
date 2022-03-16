using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignementBH : SteeringBehaviour
{
    public override Vector3 VectorCalc(Boid boid)
    {
        // behavior characteristic vector calculation
        return Vector2.right;
    }
}
