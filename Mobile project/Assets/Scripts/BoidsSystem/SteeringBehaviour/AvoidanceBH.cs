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
        float rayLength = ( boid.stats.viewDistance);

        Vector3 avoidVec = Vector3.zero;
        int hitVecCoeff = 0;

        bool findWay = false;
        //Debug.DrawRay(boid.transform.position, rayDir * 10f, Color.blue);

        for (int i = 0;  i < rayStep * 2 ; i++)
        {
            
            float rayCoef = Mathf.Ceil(i * 0.5f) * Mathf.Sign(i%2 - 1);
            Vector3 rayDir = new Vector3(Mathf.Cos(rayRad + radStep * rayCoef), 0, Mathf.Sin(rayRad + radStep * rayCoef)).normalized;
            //Debug.DrawRay(boid.transform.position, rayDir * boid.velocity.magnitude, (i % 2 == 0) ? Color.blue : Color.cyan);

            Ray checkRay = new Ray(boid.transform.position, rayDir * rayLength);
            RaycastHit rayHit;
            bool hit = Physics.SphereCast(checkRay, rayWidth, out rayHit,  rayLength, obstacleMask);
            //bool hit = Physics.Raycast(checkRay, out rayHit, boid.velocity.magnitude, obstacleMask);
         
            //Debug.DrawRay(boid.transform.position, rayDir * rayLength, (hit)? Color.red : Color.green);

            if (hit)
                hitVecCoeff = 1;

            if (!hit && !findWay)
            {
                //Debug.DrawRay(boid.transform.position, rayDir *2, Color.green);
                avoidVec = rayDir ;
                findWay = true;
            }
                

            if(i == rayStep * 2 -1 && !findWay)
                avoidVec = rayDir;
        }
        Debug.DrawRay(boid.transform.position, avoidVec, Color.blue);
        return avoidVec * hitVecCoeff;
    }
}
