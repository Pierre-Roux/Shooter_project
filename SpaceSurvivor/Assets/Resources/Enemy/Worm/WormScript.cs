using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class WormScript : EnemyBase
{
[Header("Param")] 
    [SerializeField] public float stopDistance;
    [SerializeField] public float attackCooldown ;

    [HideInInspector] private AIPath path;
    [HideInInspector] private float ditanceToTarget;
    [HideInInspector] private float lastAttackTime;
    
    
    // Start is called before the first frame update
    void Start()
    {
        path = GetComponent<AIPath>();
        lastAttackTime = -attackCooldown; 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        path.maxSpeed = speed;

        ditanceToTarget = Vector2.Distance(transform.position, target.transform.position);
        if (ditanceToTarget < stopDistance)
        {
            path.destination = transform.position;
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
            player.TakeDamage(damage,"Cac");
            lastAttackTime = Time.time;
        }
    }
}
