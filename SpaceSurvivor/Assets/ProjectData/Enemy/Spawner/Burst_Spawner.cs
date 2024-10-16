using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burst_Spawner : EnemyBase
{
[Header("Param")] 
    [SerializeField] public GameObject enemy;
    [SerializeField] public float spawnCooldown;
    [SerializeField] public float spawnDistance;
    [SerializeField] public float burstCooldown;
    [SerializeField] public float spawnDelay;
    [SerializeField] public float enemyNumber;
[Header("Other")] 
    [SerializeField] public Transform spawnPoint;
    
    [HideInInspector] private float lastSpawnTime;
    [HideInInspector] private float lastBurstTime;
    [HideInInspector] private float enemyInQueue;


    // Start is called before the first frame update
    void Start()
    {
        lastBurstTime = -burstCooldown; 
        lastSpawnTime = -spawnDelay;
    }

    // Update is called once per frame
    void FixedUpdate()
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
