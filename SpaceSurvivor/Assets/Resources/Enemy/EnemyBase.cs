using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using FMODUnity;
using Unity.VisualScripting;

public abstract class EnemyBase : MonoBehaviour
{
[Header("Audio")]
    [SerializeField] public EventReference Death_soundEvent;
    [SerializeField] public EventReference Hit_soundEvent;
[Header("Base_Param")]
    [SerializeField] public float health; 
    [SerializeField] public float damage; 
    [SerializeField] public float speed; 
    [SerializeField] public float GlowIntensity;
[Header("XP")]
    [SerializeField] public GameObject small_XP;
    [SerializeField] public GameObject Medium_XP;
    [SerializeField] public GameObject Large_XP;
    [SerializeField] public int small_XP_Reward;
    [SerializeField] public int Medium_XP_Reward;
    [SerializeField] public int Large_XP_Reward;
[Header("Other")]
    [SerializeField] public int unitPowerMesure;
    [SerializeField] public GameObject reactivationUnitPrefab;
    [SerializeField] public float deactivationDistance;
    [HideInInspector] public bool IsDead;
    [HideInInspector] public bool hasLineOfSight;
    [HideInInspector] public GameObject target;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public WeaponBase[] weapons;
    [HideInInspector] public float checkInterval = 0.2f;  // Temps entre chaque appel du calcul du lign of sight
    [HideInInspector] public float checkTimer = 0; // Timer pour le lign of sight
    [HideInInspector] public float DistanceCheck = 100f; // Distance de check du lign of sight (modifié sur chaque ennemi)
    
    [HideInInspector] private float GlowDuration = 0.1f;
    [HideInInspector] private float CurrentSlow;
    [HideInInspector] private float TimeStopSlow;
    [HideInInspector] private float SlowTime = 3;
    [HideInInspector] private float StartSpeed;
    [HideInInspector] private Coroutine SlowCoroutine;
    [HideInInspector] private float initialIntensity;

    [HideInInspector] public FMOD.Studio.EventInstance DeathInstance;
    [HideInInspector] public FMOD.Studio.EventInstance HitInstance;

    public virtual void Die()
    {
        for (int i = 0; i < small_XP_Reward; i++)
        {
            Instantiate(small_XP,(Vector2)transform.position,transform.rotation);   
        }
        for (int i = 0; i < Medium_XP_Reward; i++)
        {
            Instantiate(Medium_XP,(Vector2)transform.position,transform.rotation);   
        }
        for (int i = 0; i < Large_XP_Reward; i++)
        {
            Instantiate(Large_XP,(Vector2)transform.position,transform.rotation);   
        }

        Destroy(gameObject);
    }

    void Awake()
    {
        target = null;
        weapons = GetComponentsInChildren<WeaponBase>();
        initialIntensity = GetComponent<Light2D>().intensity;
        StartSpeed = speed;
    }

    void Update()
    {
        if (target == null && Player_controler.Instance != null)
        {
            target = Player_controler.Instance.gameObject; 
        }

        deactivationDistance = 160f; // Variable de distance de disparition de l'ennemi, obligatoirement 160 ou plus
        if (target != null)
        {
            if (Vector2.Distance(transform.position, target.transform.position) > deactivationDistance)
            {
                CreateReactivationUnit();
                gameObject.SetActive(false);  // Désactiver l'ennemi
            }
        }
    }

    private void CreateReactivationUnit()
    {
        // Instancier une ReactivationUnit à la position actuelle de l'ennemi
        Instantiate(reactivationUnitPrefab, transform.position, Quaternion.identity).GetComponent<ReactivationUnit>().enemyToReactivate = this;
    }

    public virtual void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            IsDead = true;
            PlayDeathSound();
            Die();
        }
        else
        {
            StartCoroutine(GlowOnHit());
            PlayHitSound();
        }
    }

    public virtual void TakeSlow(float SlowAmount, float MaxAmount)
    {
        if (CurrentSlow + SlowAmount >= MaxAmount)
        {
            CurrentSlow = MaxAmount;
            TimeStopSlow = Time.time + SlowTime;
        }
        else
        {
            CurrentSlow += SlowAmount;
            TimeStopSlow = Time.time + SlowTime;
        }

        Debug.Log("SlowTaked : " + CurrentSlow);

        if (SlowCoroutine == null)
        {
            SlowCoroutine = StartCoroutine(SlowEffect());
        }
    }

    public IEnumerator SlowEffect()
    {
        while (Time.time < TimeStopSlow)
        {
            speed = StartSpeed - (StartSpeed * CurrentSlow /100);
            yield return null;
        }

        CurrentSlow = 0;
        speed = StartSpeed;
        SlowCoroutine = null;
        Debug.Log("SlowEnded");
        yield return null;
    }

    public virtual void CalculateLineOfSight(float large)
    {
        // Line of sight
        int layerMask = LayerMask.GetMask("Player", "Obstacle");

        Vector2 direction = (target.transform.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, target.transform.position);
        Vector2 size = new Vector2(large, large);  // Size of the Raybox

        RaycastHit2D ray = Physics2D.BoxCast(transform.position, size, 0f, direction, distance, layerMask);
        if (ray.collider != null)
        {
            hasLineOfSight = ray.collider.CompareTag("Player");
        }

        //For debug
        #if UNITY_EDITOR
        Color debugColor = hasLineOfSight ? Color.green : Color.red;
        Debug.DrawLine(transform.position, target.transform.position, debugColor);
        #endif
    }

    public virtual void LookPlayer()
    {
        Vector2 aimDirection = new Vector2(target.transform.position.x,target.transform.position.y) - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg -90f;
        rb.rotation = aimAngle;
    }

    IEnumerator GlowOnHit()
    {
        float elapsedTime = 0f;
        
        //Debug.Log("Intensity INIT" + GetComponent<Light2D>().intensity + " Objective : " + GetComponent<Light2D>().pointLightOuterRadius);

        while (elapsedTime < GlowDuration)
        {
            elapsedTime += Time.deltaTime;
            float lerpFactor = elapsedTime / GlowDuration;

            // Augmenter le glow
            GetComponent<Light2D>().intensity = Mathf.Lerp(initialIntensity, initialIntensity + GlowIntensity, lerpFactor);
            //GetComponent<Light2D>().pointLightOuterRadius = Mathf.Lerp(initialRadius, initialRadius + GlowRadius, lerpFactor);

            yield return null;
        }

        // Revenir à la valeur d'origine
        elapsedTime = 0f;
        while (elapsedTime < GlowDuration)
        {
            elapsedTime += Time.deltaTime;
            float lerpFactor = elapsedTime / GlowDuration;

            GetComponent<Light2D>().intensity = Mathf.Lerp(initialIntensity + GlowIntensity, initialIntensity, lerpFactor);
            //GetComponent<Light2D>().pointLightOuterRadius = Mathf.Lerp(initialRadius + GlowRadius, initialRadius, lerpFactor);

            yield return null;
        }

        //Debug.Log("Intensity INIT" + GetComponent<Light2D>().intensity + " Objective : " + GetComponent<Light2D>().pointLightOuterRadius);
    }

    public virtual void PlayDeathSound()
    {
        DeathInstance = RuntimeManager.CreateInstance(Death_soundEvent);
        DeathInstance.start();
    }

    public virtual void PlayHitSound()
    {
        HitInstance = RuntimeManager.CreateInstance(Hit_soundEvent);
        HitInstance.start();
    }
}
