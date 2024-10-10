using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightBlaster : WeaponBase
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

        // Initialisation des upgrades possibles
        // string pieceName, string upgradeName, float cooldown, Color color, int intensity, int damage
        availableUpgrades = new List<Upgrade>
        {
            new Upgrade("LightBlaster","Cooldown Reduction", 0.05f, new Color(0,0,0), 0, 0),
            new Upgrade("LightBlaster","Damage Boost", 0f, new Color(0,0,0), 0, 2),
            new Upgrade("LightBlaster","Intensity Boost", 0f, new Color(50,0,0), 2, 0)
        };
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

    // Appliquer l'upgrade
    public override void ApplyUpgrade(Upgrade upgrade)
    {
        fireCooldown -= upgrade.fireCooldownReduction;
        addColor += upgrade.colorBonus;
        addIntensity += upgrade.intensityBonus;
        addDamage += upgrade.damageBonus;
    }

}
