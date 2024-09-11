using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.Mathematics;

public class Artillery : EnemyBase
{
[Header("Param")] 
    [SerializeField] public float rotationSpeed;
    [SerializeField] public float MeleAttackCooldown;
    [SerializeField] public float turnDelay;

    [HideInInspector] private float lastAttackTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D> ();
        StartCoroutine(RandomMove());
        StartCoroutine(Rotate());
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
        if (Time.time >= lastAttackTime + MeleAttackCooldown)
        {
            player.TakeDamage(damage);
            lastAttackTime = Time.time;
        }
    }

    IEnumerator RandomMove()
    {            
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        while (!IsDead)
        {
            int randomAngle = UnityEngine.Random.Range(-180, 180);
            Vector2 moveDirection = Quaternion.Euler(0, 0, randomAngle) * transform.up;

            // 2ème partie : Appliquer une force pour le dash
            rb.velocity = Vector2.zero;
            rb.AddForce(moveDirection * speed, ForceMode2D.Impulse);
            yield return new WaitForSeconds(7f);
        }
    }


    IEnumerator Rotate()
    {            
        
        // Initialisation de l'angle de rotation initial
        float currentAngle = 0f;
        float targetAngle = 0f;

        while (!IsDead)
        {
            targetAngle += 45f;

            float elapsedTime = 1f;
            float timeToRotate = rotationSpeed; // Durée de la rotation progressive

            while (elapsedTime > 0)
            {
                
                // Faire une rotation progressive de l'angle actuel vers l'angle cible
                currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, timeToRotate);

                // Appliquer l'angle actuel au Rigidbody2D
                transform.rotation = Quaternion.Euler(0, 0, currentAngle);

                elapsedTime -= Time.deltaTime;
                yield return null; // Attendre la prochaine frame
            }

            foreach (WeaponBase weapon in weapons)
            {
                weapon.Fire();
            }

            // Attendre une seconde avant de tourner à nouveau
            yield return new WaitForSeconds(turnDelay);
        }
    }
}
