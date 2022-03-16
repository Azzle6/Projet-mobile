using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidanceBH : SteeringBehaviour
{
    public float rayStep;
    public override Vector3 VectorCalc(Boid boid)
    {
        float radStep = Mathf.Deg2Rad * boid.stats.viewAngle * 0.5f / rayStep;
        float rayRad = Mathf.Atan2(boid.transform.forward.z, boid.transform.forward.x);

        Vector3 avoidVec = boid.transform.forward;
        //Debug.DrawRay(boid.transform.position, rayDir * 10f, Color.blue);

        for (int i = 0;  i < rayStep ; i++)
        {
            Vector3 rayDir1 = new Vector3(Mathf.Cos(rayRad + radStep * i ), 0, Mathf.Sin(rayRad + radStep * i ));
            Vector3 rayDir2 = new Vector3(Mathf.Cos(rayRad - radStep * i ), 0, Mathf.Sin(rayRad - radStep * i ));

            Debug.DrawRay(boid.transform.position, rayDir1 * 3f, Color.blue);
            Debug.DrawRay(boid.transform.position, rayDir2 * 3f, Color.cyan);
        }
        return Vector3.zero;
    }
}
