using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBarrel : WeaponBase
{
    public Transform firepoint1;
    public Transform firepoint2;
    Boolean ShootedRight;

    // Start is called before the first frame update
    void Start()
    {
        lastFireTime = -fireCooldown; 
        ShootedRight = false;
    }

    public override void Fire()
    {
        if (Time.time >= lastFireTime + fireCooldown)
        {
            if (ShootedRight)
            {
                GameObject bullet = Instantiate(bulletPrefab, firepoint1.position, firepoint1.rotation);
                bullet.GetComponent<Rigidbody2D>().AddForce(firepoint1.up * fireForce,ForceMode2D.Impulse);
                ShootedRight = false; 
            }
            else
            {
                GameObject bullet = Instantiate(bulletPrefab, firepoint2.position, firepoint2.rotation);
                bullet.GetComponent<Rigidbody2D>().AddForce(firepoint2.up * fireForce,ForceMode2D.Impulse);
                ShootedRight = true;
            }

            lastFireTime = Time.time;
        }        
    }

}
