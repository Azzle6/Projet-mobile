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
    public BehavioursPreset presetBH;
    //public List<SteeringBehaviour> behaviours;

    protected void Start()
    {
        // initialize boids
        for(int i= 0; i < transform.childCount; i++)
        {
            Boid boid = transform.GetChild(i).GetComponent<Boid>();
            if (boid)
            {
                AddBoid(boid);
                boid.Initialize(this);
            }
        }

        for (int i = 0; i < spawnBoids; i++)
        {
            Vector3 position = transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, Random.Range(-spawnRadius, spawnRadius));
            InstantiateBoid(position);
        }

    }
    public void Update()
    {
        // run all steering behaviour throug out boids
        if(boids != null)
        {
            for (int i = 0;i <boids.Count;i++)
            {
                boids[i].ViewDetection();
                boids[i].UpdateTargetVelocity(BehaviourHelper.BehavioursVec(boids[i],presetBH.behaviours));
                //Debug.DrawRay(boids[i].transform.position, boids[i].targetVelocity);
                boids[i].Move(); 
            }
        }
    }

    #region Group
    public Vector3 GroupCenter(List<Boid> boids)
    {
        Vector2 summPosition = transform.position;
        foreach (Boid b in boids)
        {
            summPosition += (Vector2)b.transform.position;
        }
        return summPosition / boids.Count;
    }
    public void InitilaizePreset()
    {
        presetBH = Instantiate(presetBH);
        for(int i= 0; i < presetBH.behaviours.Count; i++)
        {
            if (presetBH.behaviours[i] is HeadingBH)
                presetBH.behaviours[i] = Instantiate(presetBH.behaviours[i] as HeadingBH);
        }
    }
    #endregion

    #region boid management
    public void InstantiateBoid( Vector3 position)
    {
        GameObject boid0bj = Instantiate(boidPrefab, position, transform.rotation, transform);
        Boid boid = boid0bj.GetComponent<Boid>();

        boid.Initialize(this);
        AddBoid(boid);

        // instantiate boid at position 
        // add boid in boids
        // load boid stats
    }
    public void AddBoid(Boid boid)
    {
        boids.Add(boid);
    }

    public void RemoveBoid(int boidIndex)
    {
        // remove boid from list && destroy this
    }

    public void RemoveBoid(Boid boid)
    {
        // remove boid from list && destroy this
    }
    #endregion

}
