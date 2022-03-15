using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaveGroup : MonoBehaviour
{
    public BoidStats boidStats;

    [HideInInspector] public List<Boid> boids;
    [HideInInspector] public List<SteeringBehaviour> behaviours;
    private void Start()
    {
        // initialize boids
        for(int i= 0; i < transform.childCount; i++)
        {
            Boid boid = transform.GetChild(i).GetComponent<Boid>();
            if (boid)
            {
                boids.Add(boid);
                boid.Initialize(this, boidStats);
            }
        }
    }
    private void Update()
    {
        // run all steering behaviour throug out boids
        if(boids != null)
        {
            for (int i = 0;i <boids.Count;i++)
            {
                boids[i].ViewDetection();
                boids[i].UpdateVelocity(BehavioursVector(behaviours, boids[i]));
                boids[i].Move(); 
            }
        }
    }

    Vector2 BehavioursVector( List<SteeringBehaviour> behaviours, Boid boid)
    {
        Vector2 vector = Vector2.zero;
        for(int i = 0; i < behaviours.Count; i++)
        {
            vector += behaviours[i].VectorCalc(boid) * behaviours[i].weight;
        }
        return vector;
    }
    public Vector2 GroupCenter(List<Boid> boids)
    {
        Vector2 summPosition = transform.position;
        foreach (Boid b in boids)
        {
            summPosition += (Vector2)b.transform.position;
        }
        return summPosition / boids.Count;
    }
    public void AddBoid( Vector2 position)
    {
        // instantiate boid at position 
        // add boid in boids
        // load boid stats
    }

    public void RemoveBoid(int boidIndex)
    {
        // remove boid from list && destroy this
    }

    public void RemoveBoid(Boid boid)
    {
        // remove boid from list && destroy this
    }

}
