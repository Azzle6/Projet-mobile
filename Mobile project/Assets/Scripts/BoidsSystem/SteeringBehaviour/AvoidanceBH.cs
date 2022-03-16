using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidanceBH : SteeringBehaviour
{
    public int rayStep = 10;
    public float rayWidth = 1;
    public LayerMask obstacleMask;
    public override Vector3 VectorCalc(Boid boid)
    {
        float radStep = Mathf.Deg2Rad * boid.stats.viewAngle * 0.5f / rayStep;
        float rayRad = Mathf.Atan2(boid.transform.forward.z, boid.transform.forward.x);

        Vector3 avoidVec = Vector3.zero;
        //Debug.DrawRay(boid.transform.position, rayDir * 10f, Color.blue);

        for (int i = 0;  i < rayStep * 2 ; i++)
        {
            float rayCoef = Mathf.Ceil(i * 0.5f) * Mathf.Sign(i%2 - 1);
            Vector3 rayDir = new Vector3(Mathf.Cos(rayRad + radStep * rayCoef), 0, Mathf.Sin(rayRad + radStep * rayCoef)).normalized;
            //Debug.DrawRay(boid.transform.position, rayDir * boid.stats.viewDistance, (i % 2 == 0) ? Color.blue : Color.cyan);

            Ray checkRay = new Ray(boid.transform.position, rayDir);
            RaycastHit rayHit;
            bool hit = Physics.SphereCast(checkRay, rayWidth, out rayHit, Mathf.Clamp(boid.stats.viewDistance - rayWidth,rayWidth, boid.stats.viewDistance), obstacleMask);
            if (hit)
                Debug.DrawLine(boid.transform.position, rayHit.point, Color.red);

            if (!hit)
                return rayDir * boid.targetVelocity.magnitude/Vector3.Distance(boid.transform.position,rayHit.point);

            if(i == rayStep * 2 -1)
                avoidVec = rayDir * boid.targetVelocity.magnitude / Vector3.Distance(boid.transform.position, rayHit.point);
        }
        return avoidVec;
    }
}
