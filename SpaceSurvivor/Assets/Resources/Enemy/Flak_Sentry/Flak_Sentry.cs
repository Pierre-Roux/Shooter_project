using UnityEngine;

public class Flak_Sentry : EnemyBase
{
[Header("Param")] 
    [SerializeField] public float attackCooldown ;
    
    [HideInInspector] private float lastAttackTime;

    void Start()
    {
        DistanceCheck = 60f;
    }

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
                    CalculateLineOfSight(1f);
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
            TakeDamage(collision.gameObject.GetComponent<PlayerBulletBase>().damage);
            Destroy(collision.gameObject);
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
