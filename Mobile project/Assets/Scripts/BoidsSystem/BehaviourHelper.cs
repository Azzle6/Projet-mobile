using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BehaviourHelper 
{
    public static Vector3 BehavioursVec( Boid boid, List<SteeringBehaviour> bhs)
    {
        Vector3 vecSumm = Vector3.zero;
        foreach(SteeringBehaviour bh in bhs)
        {
            vecSumm += BehaviourVec(boid, bh) * bh.weight;
        }
        return vecSumm;
    }
    public static Vector3 BehaviourVec(Boid boid, SteeringBehaviour bh)
    {
        if (bh is SeparationBH)
            return SeparationVec(boid, (SeparationBH)bh);

        if (bh is RepulseBH)
            return RepulseVec(boid, (RepulseBH)bh);

        if (bh is HeadingBH)
            return HeadingVec(boid, (HeadingBH)bh);

        if (bh is CohesionBH)
            return CohesionVec(boid, (CohesionBH)bh);

        if (bh is AvoidanceBH)
            return AvoidanceVec(boid, (AvoidanceBH)bh);

        if (bh is AlignementBH)
            return AlignementVec(boid, (AlignementBH)bh);

        return Vector3.zero;
    }
    public static HeadingBH GetHeadingBH(ref List<SteeringBehaviour> bhs)
    {
        HeadingBH head = null;
        for(int i = 0; i< bhs.Count; i++)
        {
            if (bhs[i] is HeadingBH)
                return bhs[i] as HeadingBH;
        }
        return head;
    }
    #region specific behaviours
    static Vector3 SeparationVec(Boid boid,SeparationBH bh)
    {
        if (boid.boidMates.Count <= 0)
            return Vector3.zero;

        Vector3 sepVec = Vector3.zero;
        for (int i = 0; i < boid.boidMates.Count; i++)
        {
            float dist = Vector3.Distance(boid.transform.position, boid.boidMates[i].transform.position);
            if (dist != 0)
                sepVec += (boid.transform.position - boid.boidMates[i].transform.position) / dist;
        }
        // behavior characteristic vector calculation
        return (sepVec / boid.boidMates.Count);
    }
    static Vector3 RepulseVec(Boid boid,RepulseBH bh)
    {
        Vector3 repulsVec = Vector3.zero;
        Collider[] repulseCol = Physics.OverlapSphere(boid.transform.position, bh.repulseRadius, bh.repulseLayer);
        for (int i = 0; i < repulseCol.Length; i++)
        {
            RaycastHit hit;
            Physics.Raycast(boid.transform.position, (repulseCol[i].transform.position - boid.transform.position), out hit, bh.repulseLayer);

            if (hit.distance != 0)
                repulsVec += (boid.transform.position - hit.point) / hit.distance;
        }

        if (repulseCol.Length > 0)
            return (repulsVec / repulseCol.Length);
        else
            return Vector3.zero;
    }
    static Vector3 HeadingVec(Boid boid, HeadingBH bh)
    {

        Vector3 headVec = Vector3.zero;
        if (!bh.head)
        {
            //Debug.LogWarning("Missing Head");
            return headVec;
        }

        if (Vector3.Distance(boid.transform.position, bh.head.position) > bh.headRadius)
            headVec = (bh.head.position - boid.transform.position).normalized;

        return headVec;
    }
    static Vector3 CohesionVec(Boid boid,CohesionBH bh)
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
    static Vector3 AvoidanceVec(Boid boid,AvoidanceBH bh)
    {
        float radStep = Mathf.Deg2Rad * boid.stats.viewAngle * 0.5f / bh.rayStep;
        float rayRad = Mathf.Atan2(boid.transform.forward.z, boid.transform.forward.x);
        float rayLength = (boid.stats.viewDistance);

        Vector3 avoidVec = Vector3.zero;
        int hitVecCoeff = 0;

        bool findWay = false;
        //Debug.DrawRay(boid.transform.position, rayDir * 10f, Color.blue);

        for (int i = 0; i < bh.rayStep * 2; i++)
        {

            float rayCoef = Mathf.Ceil(i * 0.5f) * Mathf.Sign(i % 2 - 1);
            Vector3 rayDir = new Vector3(Mathf.Cos(rayRad + radStep * rayCoef), 0, Mathf.Sin(rayRad + radStep * rayCoef)).normalized;
            //Debug.DrawRay(boid.transform.position, rayDir * boid.velocity.magnitude, (i % 2 == 0) ? Color.blue : Color.cyan);

            Ray checkRay = new Ray(boid.transform.position, rayDir * rayLength);
            RaycastHit rayHit;
            bool hit = Physics.SphereCast(checkRay, bh.rayWidth, out rayHit, rayLength, bh.obstacleMask);
            //bool hit = Physics.Raycast(checkRay, out rayHit, boid.velocity.magnitude, obstacleMask);

            //Debug.DrawRay(boid.transform.position, rayDir * rayLength, (hit)? Color.red : Color.green);

            if (hit)
                hitVecCoeff = 1;

            if (!hit && !findWay)
            {
                //Debug.DrawRay(boid.transform.position, rayDir *2, Color.green);
                avoidVec = rayDir;
                findWay = true;
            }


            if (i == bh.rayStep * 2 - 1 && !findWay)
                avoidVec = rayDir;
        }
        Debug.DrawRay(boid.transform.position, avoidVec, Color.blue);
        return avoidVec * hitVecCoeff;
    }
    static Vector3 AlignementVec(Boid boid,AlignementBH bh)
    {
        if (boid.boidMates.Count <= 0)
            return Vector3.zero;

        Vector3 velocitySumm = Vector3.zero;
        for (int i = 0; i < boid.boidMates.Count; i++)
        {
            velocitySumm += boid.boidMates[i].velocity;
        }
        // behavior characteristic vector calculation
        return (velocitySumm / boid.boidMates.Count) - boid.velocity;
    }
    #endregion
}
