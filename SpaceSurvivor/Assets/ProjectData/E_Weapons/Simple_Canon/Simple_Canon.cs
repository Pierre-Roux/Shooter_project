using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Simple_Canon : WeaponBase
{
    public Transform firepoint;
    public Transform WeaponPosition;
    private Transform target;
    
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        lastFireTime = -fireCooldown; 
    }

    void FixedUpdate()
    {
            transform.position = WeaponPosition.position;
            
            Vector2 aimDirection = (Vector2)(target.position - transform.position);
            float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg -90f;
            transform.rotation = Quaternion.Euler(0, 0, aimAngle);
            if (hasLineOfSight)
            {
                if (Time.time >= lastFireTime + fireCooldown)
                {
                    StartCoroutine(ShortDelayFire());
                }
            }
    }

    public override void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(firepoint.up * fireForce,ForceMode2D.Impulse);
        lastFireTime = Time.time;   
    }

    IEnumerator ShortDelayFire()
    {
        // délai de 0.1 seconde
        yield return new WaitForSeconds(0.02f);
        Fire();
    }
}
