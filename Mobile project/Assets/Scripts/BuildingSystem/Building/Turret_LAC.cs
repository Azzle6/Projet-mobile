using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_LAC : Building
{
    [HideInInspector]public TurretSO_LAC[] stats;
    public EnemyBoid enemyTarget;
    public LayerMask enemyMask;
    float attackDelay = 0;

    public int people;
    [Header("Attack")]
    public Transform shootPoint;
    public GameObject bulletPrefab;
    GameObject bullet;
    float travelBullet = 0;
    bool attacking;
    [Header("Debug")]
    //public GameObject target;
    //public MeshRenderer targetRenderer;
    public Material targetAttack, targetAim;

    [Header("Upgrades")] 
    public GameObject[] upgradableVisuals;
    public Material[] upgradesMat;

    [Header("Range")]
    public Transform rangeOrigin;
    public Material normal, aggro;
    public MeshRenderer rangeMesh;

    private void Start()
    {
        stats = Array.ConvertAll(statsSO, input => input as TurretSO_LAC);
    }

    public override void Upgrade()
    {
        base.Upgrade();
        foreach (GameObject GO in upgradableVisuals)
        {
            MeshRenderer meshRend = GO.GetComponent<MeshRenderer>();
            if (meshRend)
            {
                meshRend.material = upgradesMat[level];
            }
        }
        SetRange(stats[level].range);
    }

    public void Update()
    {
        UpdateTarget();
        if (enemyTarget)
        {
            rangeMesh.material = aggro;
            if(!attacking)
                attackDelay += Time.deltaTime;

            if(attackDelay > stats[level].attackSpeed)
            {
                
                attackDelay = 0;
                travelBullet = 0;
                attacking = true;  
                
                bullet = Instantiate(bulletPrefab, shootPoint);
                Debug.Log("bullet" + bullet.name);
                //Attack(enemyTarget);
            }
            if (attacking)
            {
                travelBullet += Time.deltaTime / stats[level].bulletDuration;
                bullet.transform.position = Vector3.Lerp(shootPoint.position, enemyTarget.transform.position, travelBullet);
                bullet.transform.forward = (enemyTarget.transform.position - shootPoint.position).normalized;

                if(travelBullet >= 1)
                {
                    Attack(enemyTarget);
                    Destroy(bullet);
                    attacking = false;
                }
            }

            // debug target
            //target.gameObject.SetActive(true);
            //target.transform.position = enemyTarget.transform.position;
        }
        else
        {
            rangeMesh.material = normal;
            if (bullet)
                Destroy(bullet);
            attacking = false;

            //target.gameObject.SetActive(false);
        }
            
    }

    public override void RegisterTile()
    {
        base.RegisterTile();
        RessourceManager_LAC.instance.defendTile += GetTile();
    }

    #region Defend
    public void UpdateTarget()
    {
        if (!enemyTarget)
        {
            float minDist = stats[level].range;
            EnemyBoid targetT = null;

            Collider[] targets = Physics.OverlapSphere(transform.position, stats[level].range, enemyMask);
            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i])
                {
                    float dist = Vector3.Distance(transform.position, targets[i].transform.position);
                    if(dist < minDist)
                    {
                        EnemyBoid enemy = targets[i].GetComponent<EnemyBoid>();
                        if (enemy)
                        {
                            minDist = dist;
                            targetT = enemy;
                        }
                    }
                }
            }
            enemyTarget = targetT;
        }
        else if (Vector3.Distance(transform.position, enemyTarget.transform.position) > stats[level].range)
            enemyTarget = null;
    }
    public void Attack(EnemyBoid enemytarget)
    {
        enemyTarget.TakeDamage(CurrentDamage());
        // debug
        //targetRenderer.material = targetAttack;
        StartCoroutine(ResetTargetMat(0.5f));
    }
    #endregion

    #region Pop management
    public void AddPop()
    {
        if (RessourceManager_LAC.instance.population <= 0 || people == stats[level].maxPeople)
            return;

        people++;
        RessourceManager_LAC.instance.population--;
    }

    public void RemovePop()
    {
        if (people <= 1)
            return;

        people--;
        RessourceManager_LAC.instance.population++;
    }

    public override void Remove()
    {
        base.Remove();
        RessourceManager_LAC.instance.population += people;
    }
    #endregion

    #region Get Stats
    public int CurrentDamage()
    {
        return (int)Mathf.Ceil(stats[level].damage + stats[level].peopleGainDamage * (people - 1));
    }
    public float CurrentRange()
    {
        return stats[level].range;
    }
    public float CurrentAttackSpeed()
    {
        return stats[level].attackSpeed + stats[level].peopleGainAttackSpeed * (people - 1);
    }
    #endregion
    public void SetRange(float range)
    {
        rangeOrigin.localScale = new Vector3(range * 0.5f, 1, range * 0.5f);
    }


    // debug
    IEnumerator ResetTargetMat(float delay)
    {
        yield return new WaitForSeconds(delay);
        //targetRenderer.material = targetAim;

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = (enemyTarget) ?  Color.magenta : Color.green;
        //Gizmos.DrawWireSphere(transform.position,stats[level].range);
    }
}
