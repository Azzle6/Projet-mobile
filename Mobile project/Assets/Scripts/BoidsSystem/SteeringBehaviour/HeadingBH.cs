using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadingBH : SteeringBehaviour
{
    public Transform head;
    public float headRadius = 1.5f;
    public override Vector3 VectorCalc(Boid boid)
    {
       
        Vector3 headVec = Vector3.zero;
        if (!head)
        {
            Debug.LogWarning("Missing Head");
            return headVec;
        }

        if (Vector3.Distance(boid.transform.position, head.position) > headRadius)
            headVec = (head.position - boid.transform.position).normalized;

        return headVec;
    }
}
