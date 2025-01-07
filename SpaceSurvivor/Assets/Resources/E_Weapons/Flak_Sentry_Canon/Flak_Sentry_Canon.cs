using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Flak_Sentry_Canon : WeaponBase
{
[Header("Param")] 
    [SerializeField] public float rotationSpeed;
    [SerializeField] public float bulletNumber;
[Header("Other")] 
    [SerializeField] public Transform firepoint;
    [SerializeField] public Transform WeaponPosition;
    [HideInInspector] private Transform target;
    [HideInInspector] private float closeDistanceThreshold = 10f;
    
    // Start is called before the first frame update
    void Start()
    {
        target = Player_controler.Instance.transform; 
        lastFireTime = -fireCooldown; 
    }

    void Update()
    {
        if (target != null && transform != null)
        {
            // Prédiction de la position future du joueur
            Vector2 playerVelocity = target.GetComponent<Rigidbody2D>().velocity;
            float timeToReachTarget = Vector2.Distance(transform.position, target.position) / fireForce;
            Vector2 futurePosition = (Vector2)target.position + playerVelocity * timeToReachTarget;

            if (Vector2.Distance(futurePosition, transform.position) <= closeDistanceThreshold)
            {
                Vector2 aimDirection = (Vector2)(target.position - transform.position);
                float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg -90f;
                transform.DORotateQuaternion(Quaternion.Euler(0, 0, aimAngle), rotationSpeed);

                if (hasLineOfSight)
                {
                    if (Time.time >= lastFireTime + fireCooldown)
                    {
                        StartCoroutine(ShortDelayFire());
                    }
                }
            }
            else
            {
                Vector2 aimDirection = futurePosition - (Vector2)transform.position;
                float targetAimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
                transform.DORotateQuaternion(Quaternion.Euler(0, 0, targetAimAngle), rotationSpeed);
            }

            if (hasLineOfSight)
            {
                StartCoroutine(ShortDelayFire());
            }
        }
    }

    public override void Fire()
    {
        if (hasLineOfSight)
        {
            if (Time.time >= lastFireTime + fireCooldown)
            {
                PlayShootSound();
                lastFireTime = Time.time;
                for (int i = 0; i < bulletNumber; i++)
                {
                    int randomAngle = UnityEngine.Random.Range(-30, 30);
                    float randomForce = UnityEngine.Random.Range(-1f, 1f);
                    GameObject bullet = Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);
                    Vector2 firingDirection = Quaternion.Euler(0, 0, randomAngle) * firepoint.up;
                    bullet.GetComponent<Rigidbody2D>().AddForce(firingDirection * (fireForce + randomForce),ForceMode2D.Impulse);
                    
                    // Optionnel : ajuster la rotation du projectile pour qu'il corresponde à la direction de tir
                    //bullet.transform.rotation = Quaternion.Euler(0, 0, randomAngle);             
                }
            }        
        }
    }

    IEnumerator ShortDelayFire()
    {
        // délai de 0.1 seconde
        yield return new WaitForSeconds(0.02f);
        Fire();

    }
}
