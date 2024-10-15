using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_LightBlaster_Behavior : MonoBehaviour
{

    [SerializeField] public float fireForce;
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] public Transform firepoint;
    [SerializeField] public GameObject HomingColliderGameObject;

    [HideInInspector] public int SplitShot;
    [HideInInspector] public int HomingShot;

    [HideInInspector] private EnemyBase HomingtargetEnemy;  // Ennemi ciblé
    [HideInInspector] private Rigidbody2D rb;
    [HideInInspector] private Vector2 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Récupérer le Rigidbody2D du projectile
        fireForce = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (HomingtargetEnemy != null && HomingShot > 0 && HomingShot < 4)
        {
            switch (HomingShot)
            {
            case 1:
                HomingColliderGameObject.GetComponent<CircleCollider2D>().radius = 10f;
            break;
            case 2:
                HomingColliderGameObject.GetComponent<CircleCollider2D>().radius = 15f;
            break;
            case 3:
                HomingColliderGameObject.GetComponent<CircleCollider2D>().radius = 20f;
            break;
            }

        }

        if (HomingtargetEnemy != null && HomingShot > 0)
        {
            Debug.Log("coucou");
            Vector2 direction = (HomingtargetEnemy.transform.position - transform.position).normalized;
            moveDirection = direction;

            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle - 90);
            float rotationSpeed = 2f;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Appliquer une force constante dans la direction du mouvement
            rb.velocity = moveDirection * fireForce;
        
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Enemy"))
        {
            if (HomingtargetEnemy == null)
            {
                HomingtargetEnemy = coll.GetComponent<EnemyBase>();  // Détecter et stocker la cible
            }
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Enemy") && coll.GetComponent<EnemyBase>() == HomingtargetEnemy)
        {
            // Réinitialiser la cible lorsqu'elle sort du rayon
            HomingtargetEnemy = null;
        }
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
       switch (SplitShot)
       {
        case 1:
            InstantiateBulletWithRotation(firepoint, -90f);
            InstantiateBulletWithRotation(firepoint, 90f);
        break;
        case 2:
            InstantiateBullet(firepoint, Vector3.zero);
            InstantiateBulletWithRotation(firepoint, -90f);
            InstantiateBulletWithRotation(firepoint, 90f);
        break;
        case 3:
            InstantiateBullet(firepoint, Vector3.zero);
            InstantiateBulletWithRotation(firepoint, -180);
            InstantiateBulletWithRotation(firepoint, -90f);
            InstantiateBulletWithRotation(firepoint, 90f);
        break;

        default:
        break;
       }
    }

    public void InstantiateBullet(Transform firepoint, Vector3 offset)
    {
        GameObject bullet = Instantiate(bulletPrefab, firepoint.position + offset, firepoint.rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(firepoint.up  * fireForce,ForceMode2D.Impulse);
    }

    public void InstantiateBulletWithRotation(Transform firepoint, float angle)
    {
        // Calculer la rotation supplémentaire
        Quaternion rotation = firepoint.rotation * Quaternion.Euler(0, 0, angle);

        // Instancier la balle avec la rotation modifiée
        GameObject bullet = Instantiate(bulletPrefab, firepoint.position, rotation);
        bullet.GetComponent<Rigidbody2D>().AddForce(rotation * Vector3.up * fireForce, ForceMode2D.Impulse);
    }
}
