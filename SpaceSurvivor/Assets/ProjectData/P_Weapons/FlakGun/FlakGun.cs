using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FlakGun : WeaponBase
{
    public Transform firepoint1;
    public int bulletNumber;
    

    void Start()
    {
        lastFireTime = -fireCooldown; 
    }

    public override void Fire()
    {
        if (Time.time >= lastFireTime + fireCooldown)
        {
            
            for (int i = 0; i < bulletNumber; i++)
            {
                int randomAngle = UnityEngine.Random.Range(-30, 30);
                float randomForce = UnityEngine.Random.Range(-1f, 1f);
                GameObject bullet = Instantiate(bulletPrefab, firepoint1.position, firepoint1.rotation);
                Vector2 firingDirection = Quaternion.Euler(0, 0, randomAngle) * firepoint1.up;
                bullet.GetComponent<Rigidbody2D>().AddForce(firingDirection * (fireForce + randomForce),ForceMode2D.Impulse);

                // Optionnel : ajuster la rotation du projectile pour qu'il corresponde à la direction de tir
                //bullet.transform.rotation = Quaternion.Euler(0, 0, randomAngle);             
            }
            lastFireTime = Time.time;
        }  
    }
}
