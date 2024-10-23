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

    [HideInInspector] public int SplitShot;
    [HideInInspector] public int HomingShot;

    [HideInInspector] private Boolean ShootedRight;
    [HideInInspector] private Color addColor;
    [HideInInspector] private int addIntensity;
    [HideInInspector] private int addDamage;
    [HideInInspector] private int NBShot;


    // Start is called before the first frame update
    void Start()
    {
        lastFireTime = -fireCooldown; 
        ShootedRight = false;
        Level = 0;
        addColor = new Color(0,0,0);
        addIntensity = 0;
        addDamage = 0;
        NBShot = 1;
        SplitShot = 0;
        HomingShot = 0;

        // Initialisation des upgrades possibles
        // string Identifier ,string pieceName, string name, float cooldown, Color color, int intensity, int damage, int bulletNumber, int range, string description
        availableUpgrades = new List<Upgrade>
        {
            new Upgrade("LBFireRateT1","LightBlaster","Cooldown Reduction", 0.05f, new Color(0,0,0), 0, 0, 0, 0,"Cooldown reduction -0.05"),
            new Upgrade("LBDamageT1","LightBlaster","Damage Boost", 0f, new Color(0,0,0), 0, 2, 0, 0,"Damage boost +2"),
            new Upgrade("LBDoubleShotT1","LightBlaster","Double Shot", 0f, new Color(0,0,0), 0, 0, 1, 0,"2 times harder"),
            new Upgrade("LBSpliShotT1","LightBlaster","Split Shot", 0f, new Color(0,0,0), 0, 0, 0, 0,"2 shards"),
            new Upgrade("LBHomingShotT1","LightBlaster","Homing Shot", 0f, new Color(0,0,0), 0, 0, 0, 0,"Low Following bullets")
        };

        TierUpgrades = new List<Upgrade>
        {
            new Upgrade("LBFireRateT2","LightBlaster","Cooldown Reduction", 0.05f, new Color(0,0,0), 0, 0, 0, 0,"Cooldown reduction -0.05"),
            new Upgrade("LBDamageT2","LightBlaster","Damage Boost", 0f, new Color(0,0,0), 0, 2, 0, 0,"Damage boost +2"),
            new Upgrade("LBDoubleShotT2","LightBlaster","Triple Shot", 0f, new Color(0,0,0), 0, 0, 2, 0,"3 times harder"),
            new Upgrade("LBDoubleShotT3","LightBlaster","Quad Shot", 0f, new Color(0,0,0), 0, 0, 3, 0,"4 times harder"),
            new Upgrade("LBSpliShotT2","LightBlaster","Split Shot", 0f, new Color(0,0,0), 0, 0, 0, 0,"3 shards"),
            new Upgrade("LBSpliShotT3","LightBlaster","Split Shot", 0f, new Color(0,0,0), 0, 0, 0, 0,"4 shards"),
            new Upgrade("LBHomingShotT2","LightBlaster","Homing Shot", 0f, new Color(0,0,0), 0, 0, 0, 0,"Medium Following bullets"),
            new Upgrade("LBHomingShotT3","LightBlaster","Homing Shot", 0f, new Color(0,0,0), 0, 0, 0, 0,"Hard Following bullets")
        };
    }

    public override void Fire()
    {
        if (Time.time >= lastFireTime + fireCooldown)
        {
            PlayShootSound();

            if (ShootedRight)
            {
                ShootBullets(firepoint1);
                ShootedRight = false;
            }
            else
            {
                ShootBullets(firepoint2);
                ShootedRight = true;
            }

            lastFireTime = Time.time;
        }     
    }

    private void ShootBullets(Transform firepoint)
    {
        float offsetAmount = 0.5f;

        if (NBShot == 4)
        {
            InstantiateBullet(firepoint, firepoint.right * offsetAmount);
            InstantiateBullet(firepoint, -firepoint.right * offsetAmount);
            InstantiateBullet(firepoint, firepoint.right * (offsetAmount /2.5f));
            InstantiateBullet(firepoint, -firepoint.right *(offsetAmount /2.5f));
        }
        else if (NBShot == 3)
        {
            InstantiateBullet(firepoint, firepoint.right * offsetAmount);
            InstantiateBullet(firepoint, -firepoint.right * offsetAmount);
            InstantiateBullet(firepoint, Vector3.zero);
        }
        else if (NBShot == 2)
        {
            InstantiateBullet(firepoint, firepoint.right * offsetAmount);
            InstantiateBullet(firepoint, -firepoint.right * offsetAmount);
        }
        else
        {
            InstantiateBullet(firepoint, Vector3.zero);
        }
    }

    public void InstantiateBullet(Transform firepoint, Vector3 offset)
    {
        GameObject bullet = Instantiate(bulletPrefab, firepoint.position + offset, firepoint.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(firepoint.up  * fireForce,ForceMode2D.Impulse);
        bullet.GetComponent<Light2D>().color += addColor;
        bullet.GetComponent<Light2D>().intensity += addIntensity;
        bullet.GetComponent<PlayerBulletBase>().damage += addDamage;
        if (SplitShot > 0)
        {
            B_LightBlaster_Behavior BulletBehavior = bullet.GetComponent<B_LightBlaster_Behavior>();
            BulletBehavior.Speed = fireForce;
            BulletBehavior.SplitShot = SplitShot;
        }
        if (SplitShot > 0)
        {
            B_LightBlaster_Behavior BulletBehavior = bullet.GetComponent<B_LightBlaster_Behavior>();
            BulletBehavior.HomingShot = HomingShot;
        }
        
    }

    // Appliquer l'upgrade
    public override void ApplyUpgrade(Upgrade upgrade)
    {

        fireCooldown -= upgrade.fireCooldownReduction;
        addColor += upgrade.colorBonus;
        addIntensity += upgrade.intensityBonus;
        addDamage += upgrade.damageBonus;
        if (upgrade.BulletNumber != 0)
        {
            NBShot = upgrade.BulletNumber + 1;
        }

        if (upgrade.ID == "LBSpliShotT1")
        {
            SplitShot = 1;
        }
        else if (upgrade.ID == "LBSpliShotT2")
        {
            SplitShot = 2;
        }
        else if (upgrade.ID == "LBSpliShotT3")
        {
            SplitShot = 3;
        }

        if (upgrade.ID == "LBHomingShotT1")
        {
            HomingShot = 1;
        }
        else if (upgrade.ID == "LBHomingShotT2")
        {
            HomingShot = 2;
        }
        else if (upgrade.ID == "LBHomingShotT3")
        {
            HomingShot = 3;
        }

        bool foundUpgrade = false;
        string nextTierID = upgrade.ID.Substring(0, upgrade.ID.Length - 2) + "T" + (int.Parse(upgrade.ID.Substring(upgrade.ID.Length - 1)) + 1);

        Upgrade upgradeTierToAdd = new Upgrade("","","", 0f, new Color(0,0,0), 0, 0, 0, 0,"");
        foreach (Upgrade item in TierUpgrades)
        {
            if (item.ID == nextTierID)
            {
                upgradeTierToAdd = item;
                foundUpgrade = true;
                break;
            }
        }

        if (foundUpgrade == true)
        {
            availableUpgrades.Add(upgradeTierToAdd);
        }

        availableUpgrades.RemoveAll(u => u.ID == upgrade.ID);
    }
}
