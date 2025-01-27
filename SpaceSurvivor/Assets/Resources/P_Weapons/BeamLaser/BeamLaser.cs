using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class BeamLaser : WeaponBase
{
[Header("Param")] 
    [SerializeField] public float maxLaserDistance;
    [SerializeField] public float hitCooldown;
    [SerializeField] public int damage;
    [SerializeField] public int SlowAmount;
    [SerializeField] public float MaxSlow;
[Header("Other")] 
    [SerializeField] public Transform firepoint;
    [SerializeField] public LineRenderer lineRenderer; 
    [SerializeField] public LayerMask hitLayerMask; 
    [SerializeField] public GameObject startVFX;
    [SerializeField] public GameObject endVFX;


    [HideInInspector] private List<ParticleSystem> particles = new List<ParticleSystem>();
    [HideInInspector] private bool Activated;
    //[HideInInspector] private float addSpeed;
    [HideInInspector] private float addIntensity;
    [HideInInspector] private Color addColor;
    [HideInInspector] private bool isFiring;

    // Start is called before the first frame update
    void Start()
    {
        FillList();
        lastFireTime = -fireCooldown; 
        Activated = true;
        Level = 0;
        //addSpeed = -1;
        addIntensity = 8;
        addColor = new Color(191,47,7);
        ShootSoundInstance = RuntimeManager.CreateInstance(Shoot_soundEvent);
        SlowAmount = 0;

        // Initialisation des upgrades possibles
        // string Identifier ,string pieceName, string name, float cooldown, Color color, int intensity, int damage, int bulletNumber, int range, string description
        availableUpgrades = new List<Upgrade>
        {
            new Upgrade("BLRangeT1","BeamLaser","Range Boost", 0f, new Color(0,0,0), 0, 0, 0, 4,"Range + 4"),
            new Upgrade("BLDamageT1","BeamLaser","Damage Boost", 0f, new Color(0,0,0), 0, 30, 0, 0,"Damage boost + 30"),
            new Upgrade("BLSlowT1","BeamLaser","Slow", 0f, new Color(0,0,0), 0, 2, 0, 0,"SlowMax 30%\n SlowPerHit 5%")
        };

        TierUpgrades = new List<Upgrade>
        {
            new Upgrade("BLRangeT2","BeamLaser","Range Boost", 0.05f, new Color(0,0,0), 0, 0, 0, 4,"Range + 4"),
            new Upgrade("BLDamageT2","BeamLaser","Damage Boost", 0f, new Color(0,0,0), 0, 30, 0, 0,"Damage boost + 30"),
            new Upgrade("BLSlowT2","BeamLaser","Slow", 0f, new Color(0,0,0), 0, 0, 0, 0,"SlowMax 50%\n SlowPerHit 10%"),
            new Upgrade("BLRangeT3","BeamLaser","Range Boost", 0.05f, new Color(0,0,0), 0, 0, 0, 4,"Range + 4"),
            new Upgrade("BLDamageT3","BeamLaser","Damage Boost", 0f, new Color(0,0,0), 0, 40, 0, 0,"Damage boost + 40"),
            new Upgrade("BLSlowT3","BeamLaser","Slow", 0f, new Color(0,0,0), 0, 0, 0, 0,"SlowMax 70%\n SlowPerHit 12%"),
        };
    }

    void Update()
    {
        // Continuously update the laser position if firing
        if (isFiring)
        {
            UpdateLaser();
            PlayShootSound();
        }
        else
        {
            StopShootSound();
        }
    }

    public override void Fire()
    {
        if (!Activated)
        {
            
            isFiring = true;
            Activated = true;
            lineRenderer.enabled = true;  
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Play();
            }
        }
    }

    public override void CancelFire()
    {
        if (Activated)
        {
            isFiring = false;
            Activated = false;
            lineRenderer.enabled = false;   
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Stop();
            }     
        }       
    }

    void UpdateLaser()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        Material lineMaterial = lineRenderer.material;
        lineMaterial.SetFloat("LaserThickness", addIntensity);
        lineMaterial.SetFloat("LaserThickness", addIntensity);
        lineMaterial.SetColor("Color", addColor);

        // Raycast to check for hit
        RaycastHit2D hitInfo = Physics2D.Raycast(firepoint.position, firepoint.up, maxLaserDistance, hitLayerMask);

        startVFX.transform.position = firepoint.position;

        if (hitInfo)
        {
            lineRenderer.SetPosition(0, firepoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);

            // Apply damage
            if (hitInfo.collider != null)
            {
                if (hitInfo.collider.tag == "Enemy")
                {
                    if (Time.time >= lastFireTime + hitCooldown)
                    {
                        EnemyBase Enemy = hitInfo.collider.gameObject.GetComponent<EnemyBase>();
                        if (Enemy != null)
                        {
                            Enemy.TakeDamage(damage);
                            if (SlowAmount > 0)
                            {
                                Enemy.TakeSlow(SlowAmount,MaxSlow);
                            }
                            lastFireTime = Time.time;
                        }
                    }
                }
            }

            endVFX.transform.position = lineRenderer.GetPosition(1);
        }
        else
        {
            lineRenderer.SetPosition(0, firepoint.position);
            lineRenderer.SetPosition(1, firepoint.position + firepoint.up * maxLaserDistance);
            endVFX.transform.position = firepoint.position + firepoint.up * maxLaserDistance;
        }
    }

    void FillList()
    {
        for (int i = 0; i < startVFX.transform.childCount; i++)
        {
            ParticleSystem particleSystem = startVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                particles.Add(particleSystem);
            }
        }

        for (int i = 0; i < endVFX.transform.childCount; i++)
        {
            var particleSystem = endVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                particles.Add(particleSystem);
            }
        }
    }

    public override void PlayShootSound()
    {
        FMOD.Studio.PLAYBACK_STATE playbackState;
        ShootSoundInstance.getPlaybackState(out playbackState);

        if (playbackState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            ShootSoundInstance.start();
        } 
    }

    public void StopShootSound()
    {
        FMOD.Studio.PLAYBACK_STATE playbackState;
        ShootSoundInstance.getPlaybackState(out playbackState);

        if (playbackState != FMOD.Studio.PLAYBACK_STATE.STOPPED)
        {
            ShootSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    // Appliquer l'upgrade
    public override void ApplyUpgrade(Upgrade upgrade)
    {
        fireCooldown -= upgrade.fireCooldownReduction;
        maxLaserDistance +=  upgrade.Range;

        if (upgrade.ID == "BLSlowT1")
        {
            SlowAmount = 2;
            MaxSlow = 30;
        }
        else if (upgrade.ID == "BLSlowT2")
        {
            SlowAmount = 4;
            MaxSlow = 50;
        }
        else if (upgrade.ID == "BLSlowT3")
        {
            SlowAmount = 6;
            MaxSlow = 70;
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