using System.Collections;
using UnityEngine;

public class B_FlakCannon_Behavior : MonoBehaviour
{

    [SerializeField] public float Speed;
    [SerializeField] public CircleCollider2D HomingCollider;
    [SerializeField] public float ActivationTime;


    [HideInInspector] public float steerForce;
    [HideInInspector] public int HomingShot;

    [HideInInspector] private float ActivationTimeTimer;
    [HideInInspector] private EnemyBase HomingtargetEnemy;  // Ennemi ciblé
    [HideInInspector] private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Récupérer le Rigidbody2D du projectile
        steerForce = 0f;
        ActivationTimeTimer = Time.time + ActivationTime;


        switch (HomingShot)
        {
        case 1:
            HomingCollider.GetComponent<CircleCollider2D>().radius = 20f;
            steerForce = 100f;
        break;
        case 2:
            HomingCollider.GetComponent<CircleCollider2D>().radius = 20f;
            steerForce = 300f;
        break;
        case 3:
            HomingCollider.GetComponent<CircleCollider2D>().radius = 20f;
            steerForce = 500f;
        break;
        default:
        break;
        }
    }

    public void FixedUpdate()
    {
        if (Time.time > ActivationTimeTimer)
        {
            if (HomingtargetEnemy != null && HomingShot > 0)
            {
                rb.velocity = transform.up * Speed;
                Vector2 direction = (HomingtargetEnemy.transform.position - transform.position).normalized;
                float rotationSteer = Vector3.Cross(transform.up, direction).z;
                rb.angularVelocity = rotationSteer * steerForce;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Enemy"))
        {
            if (HomingtargetEnemy == null)
            {
                Debug.Log("EnemyFound");
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
}
