using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;
using System;

public class E_Gunner : EnemyBase
{
    private AIPath path;
    [SerializeField] private float stopDistance;
    public float attackCooldown ; 
    private float lastAttackTime;
    private float ditanceToTarget;

    // Start is called before the first frame update
    void Start()
    {
        path = GetComponent<AIPath>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateLineOfSight(1f);

        // Weapon block if ligneofsight
        if (hasLineOfSight)
        {
            foreach (WeaponBase weapon in weapons)
            {
                weapon.hasLineOfSight = true;
            }
        }
        else
        {
            foreach (WeaponBase weapon in weapons)
            {
                weapon.hasLineOfSight = false;
            }
        }

        // Pathfinding
        path.maxSpeed = speed;
        ditanceToTarget = Vector2.Distance(transform.position, target.transform.position);
        if (ditanceToTarget < stopDistance)
        {
            if (!hasLineOfSight)
            {
                path.destination = target.transform.position;
            }
            else
            {
                path.destination = transform.position;
            }
        }
        else
        {
            path.destination = target.transform.position;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AttackPlayer(collision.gameObject.GetComponent<Player_controler>());
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AttackPlayer(collision.gameObject.GetComponent<Player_controler>());
        }
    }
    void AttackPlayer(Player_controler player)
    {
        // Vérifie si le cooldown est terminé
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            player.PlayerHealth -= damage;
            lastAttackTime = Time.time;
        }
    }
}
