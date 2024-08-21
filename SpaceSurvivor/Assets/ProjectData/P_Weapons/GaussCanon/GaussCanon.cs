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

    private bool isFiring;

    // Start is called before the first frame update
    void Start()
    {
        lastFireTime = -fireCooldown; 
        lineRenderer.enabled = false;
        isFiring = false;
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
        isFiring = true;
        lineRenderer.enabled = true;  
    }

    public override void CancelFire()
    {
        isFiring = false;
        lineRenderer.enabled = false;               
    }

    void UpdateLaser()
    {
        // Raycast to check for hit
        RaycastHit2D hitInfo = Physics2D.Raycast(firepoint.position, firepoint.up, maxLaserDistance, hitLayerMask);
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
        }
        else
        {
            lineRenderer.SetPosition(0, firepoint.position);
            lineRenderer.SetPosition(1, firepoint.position + firepoint.up * maxLaserDistance);
        }
    }
}