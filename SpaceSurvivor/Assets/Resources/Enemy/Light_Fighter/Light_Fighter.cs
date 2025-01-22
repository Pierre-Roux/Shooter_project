using UnityEngine;
using Pathfinding;

public class Light_Fighter : EnemyBase
{
[Header("Param")] 
    [SerializeField] private float stopDistance;
    [SerializeField] public float attackCooldown ;

    [HideInInspector] private AIPath path;
    [HideInInspector] private float lastAttackTime;
    [HideInInspector] private float ditanceToTarget;

    // Start is called before the first frame update
    void Start()
    {
        path = GetComponent<AIPath>();
        hasLineOfSight = false;
        DistanceCheck = 60f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null)
        {
            if (Vector2.Distance(transform.position, target.transform.position) < DistanceCheck)  // Seulement si à portée
            {
                checkTimer += Time.fixedDeltaTime;
                if (checkTimer >= checkInterval)
                {
                    checkTimer = 0;
                    CalculateLineOfSight(1.5f);
                }
            }
            else
            {
                hasLineOfSight = false;
            }

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
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AttackPlayer(collision.gameObject.GetComponent<Player_controler>());
        }
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (collision.gameObject.GetComponent<PlayerBulletBase>().enemyToIgnore != this)
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
            player.TakeDamage(damage,"Cac");
            lastAttackTime = Time.time;
        }
    }
}
