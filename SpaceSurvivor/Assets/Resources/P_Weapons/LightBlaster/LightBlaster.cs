using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightBlaster : WeaponBase
{
[Header("Other")] 
    [SerializeField] public Transform firepoint1;
    [SerializeField] public Transform firepoint2;

    [HideInInspector] public int SplitShot;
    [HideInInspector] public int ExplosiveShot;

    [HideInInspector] public float explosionDamage;

    [HideInInspector] private Boolean ShootedRight;
    [HideInInspector] private Color addColor;
    [HideInInspector] private int addIntensity;
    [HideInInspector] private float addDamage;
    [HideInInspector] private int NBShot;
    [HideInInspector] private float coneAngle;
    [HideInInspector] private Vector2 mousePosition;
    [HideInInspector] private float SubMunPercentDamage;


    // Start is called before the first frame update
    void Start()
    {
        lastFireTime = -fireCooldown; 
        ShootedRight = false;
        Level = 0;
        addColor = new Color(0,0,0);
        addIntensity = 0;
        NBShot = 1;
        SplitShot = 0;
        ExplosiveShot = 0;
        coneAngle = 30;
        addDamage = bulletPrefab.GetComponent<PlayerBulletBase>().damage;

        // Initialisation des upgrades possibles
        // string Identifier ,string pieceName, string name, float cooldown, Color color, int intensity, int damage, int bulletNumber, int range, string description
        availableUpgrades = new List<Upgrade>
        {
            new Upgrade("LBFireRateT1","LightBlaster","Cooldown Reduction", 0.05f, new Color(0,0,0), 0, 0, 0, 0,"Cooldown reduction - 0.05"),
            new Upgrade("LBDamageT1","LightBlaster","Damage Boost", 0f, new Color(0,0,0), 0, 50, 0, 0,"Damage boost + 50"),
            new Upgrade("LBDoubleShotT1","LightBlaster","Double Shot", -0.1f, new Color(0,0,0), 0, 0, 1, 0,"2 times harder \n Cooldown + 0.1"),
            new Upgrade("LBSpliShotT1","LightBlaster","Split Shot", 0f, new Color(0,0,0), 0, 0, 0, 0,"2 shards"),
            new Upgrade("LBExplosiveT1","LightBlaster","Explosive Shot", 0f, new Color(0,0,0), 0, 0, 0, 0,"Baboum"),
        };

        TierUpgrades = new List<Upgrade>
        {
            new Upgrade("LBFireRateT2","LightBlaster","Cooldown Reduction", 0.05f, new Color(0,0,0), 0, 0, 0, 0,"Cooldown reduction -0.05"),
            new Upgrade("LBFireRateT3","LightBlaster","Cooldown Reduction", 0.05f, new Color(0,0,0), 0, 0, 0, 0,"Cooldown reduction -0.05"),
            new Upgrade("LBDamageT2","LightBlaster","Damage Boost", 0f, new Color(0,0,0), 0, 50, 0, 0,"Damage boost + 50"),
            new Upgrade("LBDamageT3","LightBlaster","Damage Boost", 0f, new Color(0,0,0), 0, 50, 0, 0,"Damage boost + 50"),
            new Upgrade("LBDoubleShotT2","LightBlaster","Triple Shot", +0.1f, new Color(0,0,0), 0, 0, 2, 0,"3 times harder"),
            new Upgrade("LBDoubleShotT3","LightBlaster","Quad Shot", -0.1f, new Color(0,0,0), 0, 0, 2, 0,"3 times harder but \nCooldown -0.1\nSpread -15°"),
            new Upgrade("LBSpliShotT2","LightBlaster","Split Shot", 0f, new Color(0,0,0), 0, 0, 0, 0,"3 shards"),
            new Upgrade("LBSpliShotT3","LightBlaster","Split Shot", 0f, new Color(0,0,0), 0, 0, 0, 0,"4 shards"),
            new Upgrade("LBExplosiveT2","LightBlaster","Explosive Shot", 0f, new Color(0,0,0), 0, 0, 0, 0,"Baboum"),
            new Upgrade("LBExplosiveT3","LightBlaster","Explosive Shot", 0f, new Color(0,0,0), 0, 0, 0, 0,"Baboum"),

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
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 aimDirection = mousePosition - (Vector2)transform.position;
        float baseAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg +15f;

        float halfCone = coneAngle / 2f;
        float angleStep = coneAngle / NBShot;

        for (int i = 0; i < NBShot; i++)
        {
            float offsetAngle = baseAngle - halfCone + (angleStep * i);
            float radianAngle = offsetAngle * Mathf.Deg2Rad;

            Vector2 projectileDirection = new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle));

            GameObject bullet = Instantiate(bulletPrefab, firepoint.position, Quaternion.Euler(0, 0, offsetAngle+90f));
            bullet.GetComponent<Rigidbody2D>().AddForce(projectileDirection * fireForce, ForceMode2D.Impulse);
            bullet.GetComponent<Light2D>().color += addColor;
            bullet.GetComponent<Light2D>().intensity += addIntensity;
            bullet.GetComponent<PlayerBulletBase>().damage = addDamage;
            if (SplitShot > 0)
            {
                B_LightBlaster_Behavior BulletBehavior = bullet.GetComponent<B_LightBlaster_Behavior>();
                BulletBehavior.SplitShot = SplitShot;
                BulletBehavior.SplitDamage = addDamage * SubMunPercentDamage;
            }
            if (ExplosiveShot > 0)
            {
                B_LightBlaster_Behavior BulletBehavior = bullet.GetComponent<B_LightBlaster_Behavior>();
                BulletBehavior.ExplosiveShot = ExplosiveShot;
                BulletBehavior.explosionDamage = addDamage * explosionDamage;
            }
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
            SubMunPercentDamage = 0.25f;
        }
        else if (upgrade.ID == "LBSpliShotT2")
        {
            SplitShot = 2;
            SubMunPercentDamage = 0.35f;
        }
        else if (upgrade.ID == "LBSpliShotT3")
        {
            SplitShot = 3;
            SubMunPercentDamage = 0.45f;
        }

        if (upgrade.ID == "LBExplosiveT1")
        {
            ExplosiveShot = 1;
            explosionDamage = 0.3f;
        }
        else if (upgrade.ID == "LBExplosiveT2")
        {
            ExplosiveShot = 2;
            explosionDamage = 0.6f;
        }
        else if (upgrade.ID == "LBExplosiveT3")
        {
            ExplosiveShot = 3;
            explosionDamage = 0.9f;
        }


        if (upgrade.ID == "LBDoubleShotT3")
        {
            coneAngle -= 10;
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
