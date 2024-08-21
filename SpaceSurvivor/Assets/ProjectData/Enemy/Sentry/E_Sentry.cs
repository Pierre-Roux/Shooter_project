using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Sentry : EnemyBase
{
    public float attackCooldown ; 
    private float lastAttackTime;
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
