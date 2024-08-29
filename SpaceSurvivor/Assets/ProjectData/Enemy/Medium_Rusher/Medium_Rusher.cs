using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;
using System;

public class Medium_Rusher : EnemyBase
{
    private AIPath path;
    public float stopDistance;
    public float attackCooldown ; 
    public float dashCooldown ; 
    public float dashSpeed;
    public float dashDuration;
    private float ditanceToTarget;
    private float lastAttackTime;
    private float lastDashTime;
    private String Etat;
    private Vector2 dashDirection;
    private float dashTimeRemaining;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        path = GetComponent<AIPath>();
        lastAttackTime = -attackCooldown; 
        lastDashTime = -dashCooldown; 
        Etat = "Following";
    }

    // Update is called once per frame
    void Update()
    {
        if (Etat == "Following")
        {
            CalculateLineOfSight(0.5f);

            path.maxSpeed = speed;

            ditanceToTarget = Vector2.Distance(transform.position, target.transform.position);
            if (ditanceToTarget < stopDistance)
            {
                Debug.Log("line of sight : " + hasLineOfSight);
                if (!hasLineOfSight)
                {
                    path.destination = target.transform.position;
                    path.canMove = true;
                }
                else
                {
                    Debug.Log("DashMethod2");
                    if (Time.time >= lastDashTime + dashCooldown)
                    {
                        Debug.Log("Dash");
                        path.canMove = false;

                        dashTimeRemaining = dashDuration;
                        lastDashTime = Time.time;
                        Etat = "Dashing";

                        StartCoroutine(Dash());
                    }
                    else
                    {
                        Debug.Log("NotDash");
                        path.canMove = false;
                        LookPlayer(); 
                    }
                }
            }
            else
            {
                path.canMove = true;
                path.destination = target.transform.position;
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
    IEnumerator Dash()
    {            
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

        // 1ère partie : L'ennemi regarde le joueur
        float lookDuration = 0.5f; // Durée pendant laquelle l'ennemi ajuste son orientation
        float lookTimeElapsed = 0f;

        while (lookTimeElapsed < lookDuration)
        {
            LookPlayer(); // Appelle votre fonction LookPlayer pour ajuster la rotation
            lookTimeElapsed += Time.deltaTime;
            yield return null;  // Attendre la prochaine frame
        }

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        dashDirection = (target.transform.position - transform.position).normalized;

        // 2ème partie : Appliquer une force pour le dash
        rb.velocity = Vector2.zero;
        rb.AddForce(dashDirection * dashSpeed, ForceMode2D.Impulse);

        // Attendre la fin de la durée du dash
        yield return new WaitForSeconds(dashDuration);

        rb.constraints = RigidbodyConstraints2D.None;
        rb.velocity = Vector2.zero;
        Etat = "Following";
        path.canMove = true;
    }
}
