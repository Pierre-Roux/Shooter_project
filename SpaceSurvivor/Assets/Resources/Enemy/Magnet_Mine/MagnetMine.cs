using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MagnetMine : EnemyBase 
{
[Header("Base_Param")]
    [SerializeField] public float followRange;
    [SerializeField] public GameObject explosionEffect; // Référence à l'effet de particule d'explosion
    [SerializeField] public float explosionRadius; // Rayon de l'explosion
    [SerializeField] public int explosionDamage; // Dégâts de l'explosion
    [SerializeField] public float launchForce;


    [HideInInspector] private String state;
    [HideInInspector] private Vector3 originPosition;

    // Start is called before the first frame update
    void Start()
    {
        target = Player_controler.Instance.gameObject; 
        rb = GetComponent<Rigidbody2D>();
        originPosition = transform.position;
        state = "Idle";
    }

    // Update is called once per frame
    void Update()
    {
        if (state == "MoveToPlayer")
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        rb.velocity = Vector3.zero;
        Vector2 direction = (target.transform.position - transform.position).normalized;
        rb.AddForce(direction * launchForce, ForceMode2D.Impulse);


        state = "Launched";
    }

    // Méthodes appelées par les scripts enfants pour gérer les collisions
    public void OnEnterInnerZone(Collider2D other)
    {
        GetComponent<Light2D>().color = Color.red;
        Debug.Log("Go Fight");
        if (other.gameObject.tag == "Player")
        {
            state = "MoveToPlayer";
        }
    }

    public void OnEnterOuterZone(Collider2D other)
    {
        Debug.Log("SpeedUPLight");
        GetComponent<Light2D>().color = Color.yellow;
    }

    public void OnExitOuterZone(Collider2D other)
    {
        Debug.Log("SlowDownLight");
        GetComponent<Light2D>().color = Color.white;
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

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        // Debug
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
