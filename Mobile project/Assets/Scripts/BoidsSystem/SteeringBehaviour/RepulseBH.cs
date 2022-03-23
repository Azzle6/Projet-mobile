using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepulseBH : SteeringBehaviour
{
    public LayerMask repulseLayer;
    public float repulseRadius;
    public override Vector3 VectorCalc(Boid boid)
    {
        Vector3 repulsVec = Vector3.zero;
        Collider[] repulseCol = Physics.OverlapSphere(boid.transform.position, repulseRadius, repulseLayer);
        for(int i = 0; i < repulseCol.Length; i++)
        {
            RaycastHit hit;
            Physics.Raycast(boid.transform.position, (repulseCol[i].transform.position - boid.transform.position),out hit, repulseLayer);
            
            if(hit.distance != 0)
                repulsVec += (boid.transform.position - hit.point) / hit.distance;
        }

        if (repulseCol.Length > 0)
            return (repulsVec / repulseCol.Length);
        else
            return Vector3.zero;
    }
}
