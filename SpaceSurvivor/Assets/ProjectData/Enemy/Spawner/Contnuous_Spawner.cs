using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contnuous_Spawner : EnemyBase
{
    public float spawnCooldown;
    public GameObject enemy;
    public Transform spawnPoint;
    public float spawnDistance;

    private float lastSpawnTime;
    

    // Start is called before the first frame update
    void Start()
    {
        lastSpawnTime = -spawnCooldown; 
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
            if (Time.time >= lastSpawnTime + spawnCooldown)
            {
                GameObject Instance = Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
                lastSpawnTime = Time.time;
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
