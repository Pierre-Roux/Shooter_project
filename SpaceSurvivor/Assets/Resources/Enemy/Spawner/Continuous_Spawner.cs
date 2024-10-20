using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Continuous_Spawner : EnemyBase
{
[Header("Param")] 
    [SerializeField] public GameObject enemy;
    [SerializeField] public float spawnCooldown;
    [SerializeField] public float spawnDistance;
[Header("Other")] 
    [SerializeField] public Transform spawnPoint;
    
    [HideInInspector] private float lastSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        lastSpawnTime = -spawnCooldown; 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        spawn();
    }

    void spawn()
    {
        if (target != null)
        {
            if (Vector2.Distance(transform.position, target.transform.position) <= spawnDistance)
            {
                if (Time.time >= lastSpawnTime + spawnCooldown)
                {
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
