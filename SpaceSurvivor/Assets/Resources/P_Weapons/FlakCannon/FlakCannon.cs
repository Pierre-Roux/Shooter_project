using System.Collections.Generic;
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
    [HideInInspector] private float addLifeTime;
    [HideInInspector] public int ExplosiveShot;
    

    void Start()
    {
        lastFireTime = -fireCooldown; 
        Level = 0;
        addColor = new Color(0,0,0);
        addIntensity = 0;

        // Initialisation des upgrades possibles
        // string Identifier ,string pieceName, string name, float cooldown, Color color, int intensity, int damage, int bulletNumber, int range, string description
        availableUpgrades = new List<Upgrade>
        {
            new Upgrade("FCFireRateT1","FlakCannon","Cooldown Reduction", 0.1f, new Color(0,0,0), 0, 0, 0, 0,"Cooldown reduction -0.1"),
            new Upgrade("FCDamageT1","FlakCannon","Damage Boost", 0f, new Color(0,0,0), 0, 2, 0, 0,"Damage boost +2"),
            new Upgrade("FCShotNumberT1","FlakCannon","Bullet number", 0f, new Color(0,0,0), 0, 0, 3, 0,"Bullet +3"),
            new Upgrade("FCRangeT1","FlakCannon","Range", 0f, new Color(0,0,0), 0, 0, 0, 0.25f,"Bullet range +0.25"),
            new Upgrade("FCExplosiveT1","FlakCannon","Explosive Shot", 0f, new Color(0,0,0), 0, 0, 0, 0,"Baboum")
        };

        TierUpgrades = new List<Upgrade>
        {
            new Upgrade("FCFireRateT2","FlakCannon","Cooldown Reduction", 0.2f, new Color(0,0,0), 0, 0, 0, 0,"Cooldown reduction -0.2"),
            new Upgrade("FCDamageT2","FlakCannon","Damage Boost", 0f, new Color(0,0,0), 0, 2, 0, 0,"Damage boost +2"),
            new Upgrade("FCShotNumberT2","FlakCannon","Bullet number", 0f, new Color(0,0,0), 0, 0, 8, 0,"Bullet +3"),
            new Upgrade("FCRangeT2","FlakCannon","Range", 0f, new Color(0,0,0), 0, 0, 0, 0.25f,"Bullet range +0.25"),
            new Upgrade("FCExplosiveT2","FlakCannon","Explosive Shot", 0f, new Color(0,0,0), 0, 0, 0, 0,"Baboum"),
            new Upgrade("FCFireRateT3","FlakCannon","Cooldown Reduction", 0.3f, new Color(0,0,0), 0, 0, 0, 0,"Cooldown reduction -0.3"),
            new Upgrade("FCDamageT3","FlakCannon","Damage Boost", 0f, new Color(0,0,0), 0, 2, 0, 0,"Damage boost +2"),
            new Upgrade("FCShotNumberT3","FlakCannon","Bullet number", 0f, new Color(0,0,0), 0, 0, 3, 0,"Bullet +3"),
            new Upgrade("FCRangeT3","FlakCannon","Range", 0f, new Color(0,0,0), 0, 0, 0, 0.25f,"Bullet range +0.25"),
            new Upgrade("FCExplosiveT3","FlakCannon","Explosive Shot", 0f, new Color(0,0,0), 0, 0, 0, 0,"Baboum")
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
                float randomLifeTime = UnityEngine.Random.Range(-0.5f, 0.5f);
                bullet.GetComponent<PlayerBulletBase>().lifeTime = bullet.GetComponent<PlayerBulletBase>().lifeTime + addLifeTime + randomLifeTime;
                bullet.GetComponent<Light2D>().color += addColor;
                bullet.GetComponent<Light2D>().intensity += addIntensity;
                if (ExplosiveShot > 0)
                {
                    B_FlakCannon_Behavior BulletBehavior = bullet.GetComponent<B_FlakCannon_Behavior>();
                    BulletBehavior.ExplosiveShot = ExplosiveShot;
                }
                
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
        addLifeTime += upgrade.Range;
        bulletNumber += upgrade.BulletNumber;

        if (upgrade.ID == "FCExplosiveT1")
        {
            ExplosiveShot = 1;
            Debug.Log("ExplosiveShot : "+ ExplosiveShot);
        }
        else if (upgrade.ID == "FCExplosiveT2")
        {
            ExplosiveShot = 2;
            Debug.Log("ExplosiveShot : "+ ExplosiveShot);
        }
        else if (upgrade.ID == "FCExplosiveT3")
        {
            ExplosiveShot = 3;
            Debug.Log("ExplosiveShot : "+ ExplosiveShot);
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
