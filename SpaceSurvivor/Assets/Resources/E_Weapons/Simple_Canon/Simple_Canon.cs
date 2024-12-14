using System.Collections;
using UnityEngine;

public class Simple_Canon : WeaponBase
{
[Header("Other")] 
    [SerializeField] public Transform firepoint;
    [SerializeField] public Transform WeaponPosition;

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
        // Calcule l'angle de rotation basé sur la direction vers le joueur
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        GameObject bullet = Instantiate(bulletPrefab, firepoint.position, Quaternion.Euler(0, 0, angle+90f));
        bullet.GetComponent<Rigidbody2D>().AddForce(aimDirection * fireForce,ForceMode2D.Impulse);
    }

    IEnumerator ShortDelayFire()
    {
        // délai de 0.1 seconde
        yield return new WaitForSeconds(0.02f);
        Fire();
    }
}
