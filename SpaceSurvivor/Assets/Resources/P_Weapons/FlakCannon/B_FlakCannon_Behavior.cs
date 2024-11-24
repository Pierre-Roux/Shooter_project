using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_FlakCannon_Behavior : MonoBehaviour
{

    [HideInInspector] public int ExplosiveShot;
    [SerializeField] public GameObject explosionEffect; // Référence à l'effet de particule d'explosion
    [SerializeField] public float explosionRadius; // Rayon de l'explosion
    [SerializeField] public int explosionDamage; // Dégâts de l'explosion
    [HideInInspector] private bool hasExploded;

    void OnDrawGizmos()
    {
        // Configure la couleur du Gizmo
        Gizmos.color = Color.red;
        // Dessine un cercle pour représenter la distance de spawn
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    // Start is called before the first frame update
    void Start()
    {
        hasExploded = false;
        explosionRadius = 0;
        explosionDamage = 0;

        switch (ExplosiveShot)
        {
        case 1:
            explosionRadius = 1;
            explosionDamage = 1;
        break;
        case 2:
            explosionRadius = 2;
            explosionDamage = 2;
        break;
        case 3:
            explosionRadius = 3;
            explosionDamage = 3;
        break;
        default:
        break;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (ExplosiveShot != 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (hasExploded) return; // Empêche l'explosion multiple
        hasExploded = true;

        Debug.Log("explosionRadius : "+ explosionRadius);

        // Créer un effet de particules
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Infliger des dégâts aux objets proches dans le rayon de l'explosion
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D nearbyObject in colliders)
        {
            if (nearbyObject.CompareTag("Enemy"))
            {
                nearbyObject.GetComponent<EnemyBase>().TakeDamage(explosionDamage);
            }
        }

        // Après un délai, le projectil est détruit
        StartCoroutine(DestroyAfterExplosion());
    }

    private IEnumerator DestroyAfterExplosion()
    {
        yield return new WaitForSeconds(0.1f); // Temps pour l'effet d'explosion        
        Destroy(gameObject);
    }
}
