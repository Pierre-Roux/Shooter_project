using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaussCanon : WeaponBase
{
    public Transform firepoint;
    public LineRenderer lineRenderer; 
    public float maxLaserDistance;
    public LayerMask hitLayerMask; 
    public float hitCooldown;
    public int damage;
    public GameObject startVFX;
    public GameObject endVFX;
    private List<ParticleSystem> particles = new List<ParticleSystem>();
    private bool Activated;


    private bool isFiring;

    // Start is called before the first frame update
    void Start()
    {
        FillList();
        lastFireTime = -fireCooldown; 
        Activated = true;
    }

    void Update()
    {
        // Continuously update the laser position if firing
        if (isFiring)
        {
            UpdateLaser();
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
                Debug.Log(particles[i].name);
                particles[i].Stop();
            }     
        }       
    }

    void UpdateLaser()
    {
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
}