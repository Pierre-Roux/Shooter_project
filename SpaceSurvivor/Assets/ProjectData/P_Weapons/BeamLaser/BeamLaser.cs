using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio; 

public class BeamLaser : WeaponBase
{
[Header("Param")] 
    [SerializeField] public float maxLaserDistance;
    [SerializeField] public float hitCooldown;
    [SerializeField] public int damage;
[Header("Other")] 
    [SerializeField] public Transform firepoint;
    [SerializeField] public LineRenderer lineRenderer; 
    [SerializeField] public LayerMask hitLayerMask; 
    [SerializeField] public GameObject startVFX;
    [SerializeField] public GameObject endVFX;


    [HideInInspector] private List<ParticleSystem> particles = new List<ParticleSystem>();
    [HideInInspector] private bool Activated;
    [HideInInspector] private float addSpeed;
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
        addSpeed = -1;
        addIntensity = 8;
        addColor = new Color(191,47,7);
        ShootSoundInstance = RuntimeManager.CreateInstance(Shoot_soundEvent);
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

    public override void Upgrade()
    {
        Level += 1;
        Debug.Log(gameObject.name + " Level : " + Level);
        switch (Level)
        {
            case 1 :
                hitCooldown -= 0.05f;
                addIntensity = 8;
                addSpeed = -1.5f;
                addColor = new Color(200,47,7);
                maxLaserDistance += 10;
            break;
            case 2 :
                hitCooldown -= 0.05f;
                addIntensity = 7;
                addSpeed = -2f;
                addColor = new Color(210,47,7);
                maxLaserDistance += 10;
            break;
            case 3 :
                hitCooldown -= 0.1f;
                addIntensity = 6;
                addSpeed = -2.5f;
                damage += 1;
                addColor = new Color(230,47,7);
                maxLaserDistance += 10;
            break;
            default:
            break;
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
}