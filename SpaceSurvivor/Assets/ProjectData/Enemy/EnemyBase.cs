using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using Pathfinding;
using UnityEngine.Rendering.Universal;

public abstract class EnemyBase : MonoBehaviour
{
[Header("Base_Param")]
    [SerializeField] public int health; 
    [SerializeField] public int damage; 
    [SerializeField] public float speed; 
    [SerializeField] public float GlowIntensity;
[Header("XP")]
    [SerializeField] public GameObject small_XP;
    [SerializeField] public GameObject Medium_XP;
    [SerializeField] public GameObject Large_XP;
    [SerializeField] public int small_XP_Reward;
    [SerializeField] public int Medium_XP_Reward;
    [SerializeField] public int Large_XP_Reward;

    [HideInInspector] public bool IsDead;
    [HideInInspector] public bool hasLineOfSight;
    [HideInInspector] public GameObject target;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public WeaponBase[] weapons;
    
    [HideInInspector] private float GlowDuration = 0.1f;
    [HideInInspector] private float initialIntensity;
    [HideInInspector] private float initialRadius;
    
    public virtual void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            IsDead = true;
            Die();
        }
        else
        {
            StartCoroutine(GlowOnHit());
        }
    }

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
        target = Player_controler.Instance.gameObject; 
        weapons = GetComponentsInChildren<WeaponBase>();
        initialIntensity = GetComponent<Light2D>().intensity;
        initialRadius = GetComponent<Light2D>().pointLightOuterRadius;
    }

    public virtual void CalculateLineOfSight(float large)
    {
        // Line of sight
        int layerToIgnore1 = LayerMask.NameToLayer("Enemy");
        int layerToIgnore2 = LayerMask.NameToLayer("EnemyParts");
        int layerToIgnore3 = LayerMask.NameToLayer("Bullets");
        int layerToIgnore4 = LayerMask.NameToLayer("EnemyBullets");
        int layerToIgnore5 = LayerMask.NameToLayer("FixEnemy");
        int layerToIgnore6 = LayerMask.NameToLayer("Ignore Raycast");
        int layerToIgnore7 = LayerMask.NameToLayer("XP");

        int layerMask = ~( (1 << layerToIgnore1) | (1 << layerToIgnore2) | (1 << layerToIgnore3) | (1 << layerToIgnore4) | (1 << layerToIgnore5) | (1 << layerToIgnore6) | (1 << layerToIgnore7));

        Vector2 direction = (target.transform.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, target.transform.position);
        Vector2 size = new Vector2(large, large);  // Size of the Raybox

        RaycastHit2D ray = Physics2D.BoxCast(transform.position, size, 0f, direction, distance, layerMask);
        if (ray.collider != null)
        {
            hasLineOfSight = ray.collider.CompareTag("Player");
        }

        //For debug
        
        if (hasLineOfSight)
        {
            Debug.DrawLine(transform.position, target.transform.position, Color.green);
        }
        else
        {
            Debug.DrawLine(transform.position, target.transform.position,Color.red);
        }
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
}
