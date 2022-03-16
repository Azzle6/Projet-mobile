using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [HideInInspector] public BehaveGroup group;
    [HideInInspector] public BoidStats stats;
    [HideInInspector] public Vector3 velocity;
    
    [HideInInspector] public List<GameObject> viewObj;
    [HideInInspector] public List<Boid> boidMates;
    public void Initialize( BehaveGroup group, BoidStats stats)
    {
        //initialize boid parameter
        this.group = group;
        this.stats = stats;
        Debug.Log(" min: " + stats.minSpeed + " max: " + stats.maxSpeed);
        velocity = Vector3.forward * Random.Range(stats.minSpeed, stats.maxSpeed);
        
    }

    public void ViewDetection()
    {
        
        viewObj.Clear();
        boidMates.Clear();

        Collider[] surrondObj = Physics.OverlapSphere(transform.position, stats.viewDistance, stats.viewLayer);
        if (surrondObj.Length <= 0)
            return;
       // Debug.Log(" Detection: ");
        for (int i = 0; i < surrondObj.Length; i++)
        {
            if(Vector3.Angle(transform.forward, (surrondObj[i].transform.position - transform.position)) < stats.viewAngle * 0.5f)
            {
                viewObj.Add(surrondObj[i].gameObject);
                Boid viewBoid = surrondObj[i].GetComponent<Boid>();
                if (viewBoid)
                {
                    boidMates.Add(viewBoid);
                    Debug.DrawLine(viewBoid.transform.position, transform.position);
                }
                    
            }
        }

    }

    float nearestBoidDist()
    {
        // return distance with nearest boid in boidMates list 
        return default;
    }

    public void UpdateVelocity( Vector3 velocity)
    {
        if (velocity == Vector3.zero)
            velocity = transform.forward;

        // change velocity, clamp by min speed && max speed
        Vector3 dir = velocity.normalized;
        float speed = velocity.magnitude;
        speed = Mathf.Clamp(speed,stats.minSpeed,stats.maxSpeed);

        this.velocity = dir*speed;
        
    }

    public void Move()
    {
        // boid displacement
        transform.forward = velocity.normalized;
        transform.position += velocity * Time.deltaTime;
      
    }
}
