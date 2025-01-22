using System.Collections;
using UnityEngine;
using Pathfinding;
using System;

public class Medium_Rusher : EnemyBase
{
[Header("Param")] 
    [SerializeField] public float stopDistance;
    [SerializeField] public float attackCooldown ; 
    [SerializeField] public float dashCooldown ; 
    [SerializeField] public float dashSpeed;
    [SerializeField] public float dashDuration;
    [SerializeField] public int DashDamage;

    [HideInInspector] private AIPath path;
    [HideInInspector] private float ditanceToTarget;
    [HideInInspector] private float lastAttackTime;
    [HideInInspector] private float lastDashTime;
    [HideInInspector] private String Etat;
    [HideInInspector] private Vector2 dashDirection;
    [HideInInspector] private float dashTimeRemaining;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        path = GetComponent<AIPath>();
        lastAttackTime = -attackCooldown; 
        lastDashTime = -dashCooldown; 
        Etat = "Following";
        DistanceCheck = 30f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (Etat == "Following")
        {
            if (target != null)
            {
                if (Vector2.Distance(transform.position, target.transform.position) < DistanceCheck)  // Seulement si à portée
                {
                    checkTimer += Time.fixedDeltaTime;
                    if (checkTimer >= checkInterval)
                    {
                        checkTimer = 0;
                        CalculateLineOfSight(0.5f);
                    }
                }
                else
                {
                    hasLineOfSight = false;
                }

                path.maxSpeed = speed;

                ditanceToTarget = Vector2.Distance(transform.position, target.transform.position);
                if (ditanceToTarget < stopDistance)
                {
                    if (!hasLineOfSight)
                    {
                        path.destination = target.transform.position;
                        path.canMove = true;
                    }
                    else
                    {
                        if (Time.time >= lastDashTime + dashCooldown)
                        {
                            path.canMove = false;

                            dashTimeRemaining = dashDuration;
                            lastDashTime = Time.time;
                            Etat = "Dashing";

                            StartCoroutine(Dash());
                        }
                        else
                        {
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
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AttackPlayer(collision.gameObject.GetComponent<Player_controler>());
        }
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (collision.gameObject.GetComponent<PlayerBulletBase>().enemyToIgnore != gameObject)
            {
                TakeDamage(collision.gameObject.GetComponent<PlayerBulletBase>().damage);
                Destroy(collision.gameObject);
            }
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
            if (Etat == "Dashing")
            {
                player.TakeDamage(DashDamage,"Cac");
                lastAttackTime = Time.time;
            }
            else
            {
                player.TakeDamage(damage,"Cac");
                lastAttackTime = Time.time;
            }
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

        int layerMask = LayerMask.GetMask("Enemy", "Obstacle");
        GetComponent<CapsuleCollider2D>().forceReceiveLayers = layerMask;

        // 2ème partie : Appliquer une force pour le dash
        rb.velocity = Vector2.zero;
        rb.AddForce(dashDirection * dashSpeed, ForceMode2D.Impulse);

        // Attendre la fin de la durée du dash
        yield return new WaitForSeconds(dashDuration);

        rb.constraints = RigidbodyConstraints2D.None;
        rb.velocity = Vector2.zero;
        damage -= 700;
        Etat = "Following";
        path.canMove = true;
    }
}
