using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Advanced_follower : WeaponBase
{
    public Transform firepoint;
    public Transform WeaponPosition;
    public float rotationSpeed;

    private Transform target;
    
    // Start is called before the first frame update
    void Start()
    {
        target = Player_controler.Instance.transform; 
        rb = GetComponent<Rigidbody2D> ();
        lastFireTime = -fireCooldown; 
    }

    void Update()
    {
        // Prédiction de la position future du joueur
        Vector2 playerVelocity = target.GetComponent<Rigidbody2D>().velocity;
        float timeToReachTarget = Vector2.Distance(transform.position, target.position) / fireForce;
        Vector2 futurePosition = (Vector2)target.position + playerVelocity * timeToReachTarget;

        Vector2 aimDirection = futurePosition - rb.position;
        float targetAimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;

        // Lissage de la rotation pour éviter les oscillations
        float currentRotation = rb.rotation;
        float smoothedRotation = Mathf.LerpAngle(currentRotation, targetAimAngle, rotationSpeed * Time.deltaTime);
        rb.rotation = smoothedRotation;

        if (hasLineOfSight)
        {
            StartCoroutine(ShortDelayFire());
        }
    }

    public override void Fire()
    {
        if (hasLineOfSight)
        {
            if (Time.time >= lastFireTime + fireCooldown)
            {
                GameObject bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);
                bullet.GetComponent<Rigidbody2D>().AddForce(firepoint.up * fireForce,ForceMode2D.Impulse);
                lastFireTime = Time.time;
            }        
        }
    }

    IEnumerator ShortDelayFire()
    {
        // délai de 0.1 seconde
        yield return new WaitForSeconds(0.02f);
        Fire();

    }
}
