using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heavy_Mine : EnemyBase
{
[Header("Base_Param")]
    [SerializeField] public float followRange;
    [SerializeField] public GameObject explosionEffect; // Référence à l'effet de particule d'explosion
    [SerializeField] public float explosionRadius; // Rayon de l'explosion
    [SerializeField] public int explosionDamage; // Dégâts de l'explosion


    [HideInInspector] private String state;
    [HideInInspector] private Vector3 originPosition;
    [HideInInspector] private bool hasExploded;

    // Start is called before the first frame update
    void Start()
    {
        target = Player_controler.Instance.gameObject; 
        originPosition = transform.position;
        state = "Idle";
        hasExploded = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == "MoveToPlayer")
        {
            if(Vector3.Distance(transform.position, originPosition) <= followRange) 
            {
                MoveTowardsPlayer();
            }
            else
            {
                state = "MoveToOrigin";
            }
            
        }
        else if (state == "MoveToOrigin")
        {
            MoveTowardsOrigin();
            if (originPosition == transform.position)
            {
                state = "Idle";
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

    }

    private void MoveTowardsOrigin()
    {
        if (Vector3.Distance(transform.position, originPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originPosition, speed * Time.deltaTime);
        }
        else
        {
            state = "Idle";
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            state = "MoveToPlayer";
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            state = "MoveToOrigin";
        }        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (hasExploded) return; // Empêche l'explosion multiple
        hasExploded = true;

        // Créer un effet de particules
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Infliger des dégâts aux objets proches dans le rayon de l'explosion
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D nearbyObject in colliders)
        {
            if (nearbyObject.CompareTag("Player"))
            {
                nearbyObject.GetComponent<Player_controler>().TakeDamage(explosionDamage);
            }
            else if (nearbyObject.CompareTag("Enemy"))
            {
                nearbyObject.GetComponent<EnemyBase>().TakeDamage(explosionDamage);
            }
        }

        // Après un délai, la mine est détruite
        StartCoroutine(DestroyAfterExplosion());
    }

    private IEnumerator DestroyAfterExplosion()
    {
        yield return new WaitForSeconds(0.1f); // Temps pour l'effet d'explosion

        for (int i = 0; i < small_XP_Reward; i++)
        {
            Instantiate(small_XP,(Vector2)transform.position,transform.rotation);   
        }
        for (int i = 0; i < Medium_XP_Reward; i++)
        {
            Instantiate(Medium_XP,(Vector2)transform.position,transform.rotation);   
        }
        for (int i = 0; i < Large_XP_Reward; i++)
        {
            Instantiate(Large_XP,(Vector2)transform.position,transform.rotation);   
        }
        
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        // Debug
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    public override void Die()
    {
        Explode();
    }
}
