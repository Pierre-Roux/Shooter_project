using System.Collections;
using UnityEngine;

public class B_LightBlaster_Behavior : MonoBehaviour
{
    [SerializeField] public float fireForce;
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] public Transform firepoint;

    [HideInInspector] public int ExplosiveShot;
    [HideInInspector] public GameObject enemyToIgnore;
    [SerializeField] public GameObject explosionEffect; // Référence à l'effet de particule d'explosion
    [SerializeField] public float explosionRadius; // Rayon de l'explosion
    [SerializeField] public float explosionDamage; // Dégâts de l'explosion

    [HideInInspector] public int SplitShot;
    [HideInInspector] public float SplitDamage;
    [HideInInspector] private bool hasExploded;

    // Start is called before the first frame update
    void Start()
    {
        hasExploded = false;

        switch (ExplosiveShot)
        {
        case 1:
            explosionRadius = 1;
        break;
        case 2:
            explosionRadius = 1.5f;
        break;
        case 3:
            explosionRadius = 2;
        break;
        default:
        break;
        }
    }

    void OnDrawGizmos()
    {
        // Configure la couleur du Gizmo
        Gizmos.color = Color.red;
        // Dessine un cercle pour représenter la distance de spawn
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }



    public void OnCollisionEnter2D(Collision2D coll)
    {
        enemyToIgnore = coll.gameObject;

       switch (SplitShot)
       {
        case 1:
            InstantiateBulletWithRotation(firepoint, 0);
            InstantiateBulletWithRotation(firepoint, -30f);
            InstantiateBulletWithRotation(firepoint, 30f);
        break;
        case 2:
            InstantiateBulletWithRotation(firepoint, -15);
            InstantiateBulletWithRotation(firepoint, -30);
            InstantiateBulletWithRotation(firepoint, 15f);
            InstantiateBulletWithRotation(firepoint, 30f);

        break;
        case 3:
            InstantiateBulletWithRotation(firepoint, 0);
            InstantiateBulletWithRotation(firepoint, -15);
            InstantiateBulletWithRotation(firepoint, 15f);
            InstantiateBulletWithRotation(firepoint, -30f);
            InstantiateBulletWithRotation(firepoint, 30f);
        break;

        default:
        break;
       }

        if (ExplosiveShot != 0)
        {
            Explode();
        }

    }

    public void InstantiateBulletWithRotation(Transform firepoint, float angle)
    {
        // Calculer la rotation supplémentaire
        Quaternion rotation = firepoint.rotation * Quaternion.Euler(0, 0, angle + 180);

        // Instancier la balle avec la rotation modifiée
        GameObject bullet = Instantiate(bulletPrefab, firepoint.position, rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(rotation * Vector3.up * fireForce, ForceMode2D.Impulse);
        bullet.GetComponent<PlayerBulletBase>().damage = SplitDamage;
        bullet.GetComponent<PlayerBulletBase>().enemyToIgnore = enemyToIgnore;
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
