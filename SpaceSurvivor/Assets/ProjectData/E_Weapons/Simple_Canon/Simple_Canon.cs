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
        rb = GetComponent<Rigidbody2D> ();
        lastFireTime = -fireCooldown; 
    }

    void Update()
    {
            transform.position = WeaponPosition.position;
            
            Vector2 aimDirection = new Vector2(target.position.x,target.position.y) - rb.position;
            float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg -90f;
            rb.rotation = aimAngle;

            if (rb.rotation == aimAngle)
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
