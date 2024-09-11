using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlakGun : WeaponBase
{
[Header("Param")] 
    public int bulletNumber;
[Header("Other")] 
    public Transform firepoint1;

    [HideInInspector] private Color addColor;
    [HideInInspector] private float addIntensity;
    

    void Start()
    {
        lastFireTime = -fireCooldown; 
        Level = 0;
        addColor = new Color(0,0,0);
        addIntensity = 0;
    }

    public override void Fire()
    {
        if (Time.time >= lastFireTime + fireCooldown)
        {
            PlayShootSound();
            for (int i = 0; i < bulletNumber; i++)
            {
                int randomAngle = UnityEngine.Random.Range(-30, 30);
                float randomForce = UnityEngine.Random.Range(-1f, 1f);
                GameObject bullet = Instantiate(bulletPrefab, firepoint1.position, firepoint1.rotation);
                Vector2 firingDirection = Quaternion.Euler(0, 0, randomAngle) * firepoint1.up;
                bullet.GetComponent<Rigidbody2D>().AddForce(firingDirection * (fireForce + randomForce),ForceMode2D.Impulse);
                bullet.GetComponent<Light2D>().color += addColor;
                bullet.GetComponent<Light2D>().intensity += addIntensity;
                
                // Optionnel : ajuster la rotation du projectile pour qu'il corresponde à la direction de tir
                //bullet.transform.rotation = Quaternion.Euler(0, 0, randomAngle);             
            }
            lastFireTime = Time.time;
        }  
    }

    public override void Upgrade()
    {
        Level += 1;
        Debug.Log(gameObject.name + " Level : " + Level);
        switch (Level)
        {
            case 1 :
                fireCooldown -= 0.5f;
                addColor = new Color(10,0,0);
                bulletNumber += 3;
            break;
            case 2 :
                fireCooldown -= 0.5f;
                addColor = new Color(20,0,0);
                addIntensity = 1;
                bulletNumber += 4;
            break;
            case 3 :
                fireCooldown -= 1f;
                addColor = new Color(40,0,0);
                addIntensity = 1.5f;
                bulletNumber += 5;
            break;
            default:
            break;
        }
    }
}
