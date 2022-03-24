using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaveGroup : MonoBehaviour
{
    public BoidStats boidStats;
    public GameObject boidPrefab;
    public int spawnBoids;
    public float spawnRadius;

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

        for (int i = 0; i < spawnBoids; i++)
        {
            Vector3 position = transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, Random.Range(-spawnRadius, spawnRadius));
            AddBoid(position);
        }


    }
    private void Update()
    {
        // run all steering behaviour throug out boids
        if(boids != null)
        {
            for (int i = 0;i <boids.Count;i++)
            {
                boids[i].UpdateState();
                boids[i].ViewDetection();
                boids[i].UpdateTargetVelocity(BehavioursVector(behaviours, boids[i]));
                //Debug.DrawRay(boids[i].transform.position, boids[i].targetVelocity);

                if(boids[i].boidState == Boid.BoidState.MOVE)
                    boids[i].Move(); 
            }
        }
    }

    Vector3 BehavioursVector( List<SteeringBehaviour> behaviours, Boid boid)
    {
        Vector3 vector = Vector3.zero;
        for(int i = 0; i < behaviours.Count; i++)
        {
            vector += behaviours[i].VectorCalc(boid) * behaviours[i].weight;
        }
        return vector;
    }
    public Vector3 GroupCenter(List<Boid> boids)
    {
        Vector2 summPosition = transform.position;
        foreach (Boid b in boids)
        {
            summPosition += (Vector2)b.transform.position;
        }
        return summPosition / boids.Count;
    }
    public void AddBoid( Vector3 position)
    {
        GameObject boid0bj = Instantiate(boidPrefab, position, transform.rotation, transform);
        Boid boid = boid0bj.GetComponent<Boid>();

        boid.Initialize(this,boidStats);
        boids.Add(boid);

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
