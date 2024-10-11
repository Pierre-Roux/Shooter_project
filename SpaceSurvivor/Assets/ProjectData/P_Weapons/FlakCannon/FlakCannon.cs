using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlakCannon : WeaponBase
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

        // Initialisation des upgrades possibles
        // string pieceName, string name, float cooldown, Color color, int intensity, int damage, int bulletNumber, int range, string description
        availableUpgrades = new List<Upgrade>
        {
            new Upgrade("FlakCannon","Cooldown Reduction", 0.05f, new Color(0,0,0), 0, 0, 0, 0,"Cooldown reduction -0.05"),
            new Upgrade("FlakCannon","Damage Boost", 0f, new Color(0,0,0), 0, 2, 0, 0,"Damage boost +2"),
            new Upgrade("FlakCannon","Bullet number", 0f, new Color(0,0,0), 0, 0, 3, 0,"Bullet +3")
        };
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

    // Appliquer l'upgrade
    public override void ApplyUpgrade(Upgrade upgrade)
    {
        fireCooldown -= upgrade.fireCooldownReduction;
        addColor += upgrade.colorBonus;
        addIntensity += upgrade.intensityBonus;
        bulletNumber += upgrade.BulletNumber;
    }
}
