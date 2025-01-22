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
    [HideInInspector] public int HomingShot;
    
    [HideInInspector] private float AngleMinus;
    [HideInInspector] private float AngleMax;

    void Start()
    {
        lastFireTime = -fireCooldown; 
        Level = 0;
        addColor = new Color(0,0,0);
        HomingShot = 0;
        addIntensity = 0;
        AngleMinus = -30;
        AngleMax = 30;

        // Initialisation des upgrades possibles
        // string Identifier ,string pieceName, string name, float cooldown, Color color, int intensity, int damage, int bulletNumber, int range, string description
        availableUpgrades = new List<Upgrade>
        {
            new Upgrade("FCFireRateT1","FlakCannon","Cooldown Reduction", 0.3f, new Color(0,0,0), 0, 0, 0, 0,"Cooldown reduction -0.3"),
            new Upgrade("FCDamageT1","FlakCannon","Damage Boost", 0f, new Color(0,0,0), 0, 80, 0, 0,"Damage boost + 80"),
            new Upgrade("FCShotNumberT1","FlakCannon","Bullet number", 0f, new Color(0,0,0), 0, 0, 3, 0,"Bullet + 3"),
            new Upgrade("FCRangeT1","FlakCannon","Range", 0f, new Color(0,0,0), 0, 0, 0, 0.2f,"Bullet speed + 20%\nAngle -8°"),
            new Upgrade("FCHomingShotT1","FlakCannon","Homing Shot", -1f, new Color(0,0,0), 0, 0, 0, 0,"Low Following bullets\nCooldown +1")
        };

        TierUpgrades = new List<Upgrade>
        {
            new Upgrade("FCFireRateT2","FlakCannon","Cooldown Reduction", 0.3f, new Color(0,0,0), 0, 0, 0, 0,"Cooldown reduction -0.3"),
            new Upgrade("FCDamageT2","FlakCannon","Damage Boost", 0f, new Color(0,0,0), 0, 70, 0, 0,"Damage boost + 70"),
            new Upgrade("FCShotNumberT2","FlakCannon","Bullet number", 0f, new Color(0,0,0), 0, 0, 3, 0,"Bullet + 3"),
            new Upgrade("FCRangeT2","FlakCannon","Range", 0f, new Color(0,0,0), 0, 0, 0, 0.2f,"Bullet speed + 20%\nAngle -8°"),
            new Upgrade("FCHomingShotT2","FlakCannon","Homing Shot", -0.5f, new Color(0,0,0), 0, 0, 0, 0,"Medium Following bullets\nCooldown +0.5"),

            new Upgrade("FCFireRateT3","FlakCannon","Cooldown Reduction", 0.3f, new Color(0,0,0), 0, 0, 0, 0,"Cooldown reduction -0.3"),
            new Upgrade("FCDamageT3","FlakCannon","Damage Boost", 0f, new Color(0,0,0), 0, 70, 0, 0,"Damage boost + 70"),
            new Upgrade("FCShotNumberT3","FlakCannon","Bullet number", 0f, new Color(0,0,0), 0, 0, 3, 0,"Bullet + 3"),
            new Upgrade("FCRangeT3","FlakCannon","Range", 0f, new Color(0,0,0), 0, 0, 0, 0.2f,"Bullet speed + 20%\nAngle -8°"),
            new Upgrade("FCHomingShotT3","FlakCannon","Homing Shot", -0.5f, new Color(0,0,0), 0, 0, 0, 0,"Hard Following bullets\nCooldown +0.5")
        };
    }

    public override void Fire()
    {
        if (Time.time >= lastFireTime + fireCooldown)
        {
            PlayShootSound();
            for (int i = 0; i < bulletNumber; i++)
            {
                float randomAngle = UnityEngine.Random.Range(AngleMinus, AngleMax);
                float randomForce = UnityEngine.Random.Range(-1f, 1f);
                GameObject bullet = Instantiate(bulletPrefab, firepoint1.position, firepoint1.rotation);
                Vector2 firingDirection = Quaternion.Euler(0, 0, randomAngle) * firepoint1.up;
                bullet.GetComponent<Rigidbody2D>().AddForce(firingDirection * (fireForce + randomForce),ForceMode2D.Impulse);
                float randomLifeTime = UnityEngine.Random.Range(-0.5f, 0.5f);
                bullet.GetComponent<PlayerBulletBase>().lifeTime = bullet.GetComponent<PlayerBulletBase>().lifeTime + addLifeTime + randomLifeTime;
                bullet.GetComponent<Light2D>().color += addColor;
                bullet.GetComponent<Light2D>().intensity += addIntensity;
                if (HomingShot > 0)
                {
                    B_FlakCannon_Behavior BulletBehavior = bullet.GetComponent<B_FlakCannon_Behavior>();
                    BulletBehavior.HomingShot = HomingShot;
                    BulletBehavior.Speed = fireForce;
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

        if (upgrade.ID == "FCHomingShotT1")
        {
            HomingShot = 1;
        }
        else if (upgrade.ID == "FCHomingShotT2")
        {
            HomingShot = 2;
        }
        else if (upgrade.ID == "FCHomingShotT3")
        {
            HomingShot = 3;
        }

        if (upgrade.ID == "FCRangeT1")
        {
            fireForce += fireForce * 0.2f;
            AngleMinus += 4f;
            AngleMax -= 4f;
        }
        else if (upgrade.ID == "FCRangeT2")
        {
            fireForce += fireForce * 0.2f;
            AngleMinus += 4f;
            AngleMax -= 4f;
        }
        else if (upgrade.ID == "FCRangeT3")
        {
            fireForce += fireForce * 0.2f;
            AngleMinus += 4f;
            AngleMax -= 4f;
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
