using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DoubleBarrel : WeaponBase
{
[Header("Other")] 
    [SerializeField] public Transform firepoint1;
    [SerializeField] public Transform firepoint2;

    [HideInInspector] private Boolean ShootedRight;
    [HideInInspector] private Color addColor;
    [HideInInspector] private int addIntensity;
    [HideInInspector] private int addDamage;

    // Start is called before the first frame update
    void Start()
    {
        lastFireTime = -fireCooldown; 
        ShootedRight = false;
        Level = 0;
        addColor = new Color(0,0,0);
        addIntensity = 0;
        addDamage = 0;
    }

    public override void Fire()
    {
        if (Time.time >= lastFireTime + fireCooldown)
        {
            if (ShootedRight)
            {
                PlayShootSound();
                GameObject bullet = Instantiate(bulletPrefab, firepoint1.position, firepoint1.rotation);
                bullet.GetComponent<Rigidbody2D>().AddForce(firepoint1.up * fireForce,ForceMode2D.Impulse);
                bullet.GetComponent<Light2D>().color += addColor;
                bullet.GetComponent<Light2D>().intensity += addIntensity;
                ShootedRight = false; 
            }
            else
            {
                PlayShootSound();
                GameObject bullet = Instantiate(bulletPrefab, firepoint2.position, firepoint2.rotation);
                bullet.GetComponent<Rigidbody2D>().AddForce(firepoint2.up * fireForce,ForceMode2D.Impulse);
                bullet.GetComponent<Light2D>().color += addColor;
                bullet.GetComponent<Light2D>().intensity += addIntensity;
                bullet.GetComponent<PlayerBulletBase>().damage += addDamage;
                ShootedRight = true;
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
                fireCooldown -= 0.05f;
                addColor = new Color(10,0,0);
            break;
            case 2 :
                fireCooldown -= 0.05f;
                addColor = new Color(30,0,0);
                addIntensity = 1;
                addDamage +=1;
            break;
            case 3 :
                fireCooldown -= 0.1f;
                addColor = new Color(50,0,0);
                addIntensity = 2;
                addDamage +=1;
            break;
            default:
            break;
        }
    }
}
