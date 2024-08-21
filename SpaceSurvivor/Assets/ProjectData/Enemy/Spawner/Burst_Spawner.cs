using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burst_Spawner : EnemyBase
{
    public float burstCooldown;
    public float spawnDelay;
    public float enemyNumber;
    public GameObject enemy;
    public Transform spawnPoint;
    public float spawnDistance;

    private float lastSpawnTime;
    private float lastBurstTime;
    private float enemyInQueue;
    


    // Start is called before the first frame update
    void Start()
    {
        lastBurstTime = -burstCooldown; 
        lastSpawnTime = -spawnDelay;
    }

    // Update is called once per frame
    void Update()
    {
        spawn();
    }

    void spawn()
    {
        if (Vector2.Distance(transform.position, target.transform.position) <= spawnDistance)
        {
            if (Time.time >= lastBurstTime + burstCooldown)
            {
                lastBurstTime = Time.time;
                enemyInQueue = enemyNumber;
            }        

            for (int i = 0; i < enemyInQueue; i++)
            {
                if (Time.time >= lastSpawnTime + spawnDelay)
                {
                    enemyInQueue -= 1;
                    GameObject Instance = Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
                    lastSpawnTime = Time.time;
                }                
            }
        }
    }

    void OnDrawGizmos()
    {
        // Configure la couleur du Gizmo
        Gizmos.color = Color.red;
        // Dessine un cercle pour représenter la distance de spawn
        Gizmos.DrawWireSphere(transform.position, spawnDistance);
    }
}
