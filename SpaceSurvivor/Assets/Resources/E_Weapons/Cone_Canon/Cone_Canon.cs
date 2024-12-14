using System.Collections;
using UnityEngine;

public class Cone_Canon : WeaponBase
{
[Header("Other")] 
    [SerializeField] public Transform firepoint;
    [SerializeField] public Transform WeaponPosition;

    [SerializeField] public int numberOfProjectiles; // Nombre de projectiles dans le cône
    [SerializeField] public float coneAngle; // Angle total du cône en degrés

    [HideInInspector] private Transform target;
    [HideInInspector] private Vector2 aimDirection ;
    
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        lastFireTime = -fireCooldown; 
    }

    void FixedUpdate()
    {
            transform.position = WeaponPosition.position;
            
            // Calcule la direction vers le joueur
            aimDirection = (target.position - transform.position).normalized;

            if (hasLineOfSight && Time.time >= lastFireTime + fireCooldown)
            {
                lastFireTime = Time.time;
                StartCoroutine(ShortDelayFire());
            }
    }

    public override void Fire()
    {
        // Calcule l'angle central (direction vers le joueur)
        float baseAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        // Calcul des offsets pour répartir les projectiles dans le cône
        float halfCone = coneAngle / 2f;
        float angleStep = coneAngle / (numberOfProjectiles - 1);

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            float offsetAngle = baseAngle - halfCone + (angleStep * i);
            float radianAngle = offsetAngle * Mathf.Deg2Rad;

            // Direction du projectile
            Vector2 projectileDirection = new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle));

            // Instancier et appliquer la force
            GameObject bullet = Instantiate(bulletPrefab, firepoint.position, Quaternion.Euler(0, 0, offsetAngle+90f));
            bullet.GetComponent<Rigidbody2D>().AddForce(projectileDirection * fireForce, ForceMode2D.Impulse);
        }
    }

    IEnumerator ShortDelayFire()
    {
        // délai de 0.1 seconde
        yield return new WaitForSeconds(0.02f);
        Fire();
    }
}
