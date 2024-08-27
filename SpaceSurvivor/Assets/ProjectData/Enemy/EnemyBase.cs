using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using Pathfinding;

public abstract class EnemyBase : MonoBehaviour
{
    public int health;  // La santé de l'ennemi
    public int damage;  // Les dégâts que l'ennemi peut infliger
    public float speed; // La vitesse de déplacement de l'ennemi

    [HideInInspector] public bool IsDead;
    [HideInInspector] public bool hasLineOfSight;
    [HideInInspector] public GameObject target;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public WeaponBase[] weapons;

    public virtual void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            IsDead = true;
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    
    void Awake()
    {
        target = Player_controler.Instance.gameObject; 
        weapons = GetComponentsInChildren<WeaponBase>();
    }

    public virtual void CalculateLineOfSight(float large)
    {
        // Line of sight
        int layerToIgnore1 = LayerMask.NameToLayer("Enemy");
        int layerToIgnore2 = LayerMask.NameToLayer("EnemyParts");
        int layerToIgnore3 = LayerMask.NameToLayer("Bullets");
        int layerToIgnore4 = LayerMask.NameToLayer("EnemyBullets");
        int layerToIgnore5 = LayerMask.NameToLayer("FixEnemy");

        int layerMask = ~( (1 << layerToIgnore1) | (1 << layerToIgnore2) | (1 << layerToIgnore3) | (1 << layerToIgnore4) | (1 << layerToIgnore5));

        Vector2 direction = (target.transform.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, target.transform.position);
        Vector2 size = new Vector2(large, large);  // Size of the Raybox

        RaycastHit2D ray = Physics2D.BoxCast(transform.position, size, 0f, direction, distance, layerMask);
        if (ray.collider != null)
        {
            hasLineOfSight = ray.collider.CompareTag("Player");
        }
    }

    public virtual void LookPlayer()
    {
        Vector2 aimDirection = new Vector2(target.transform.position.x,target.transform.position.y) - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg -90f;
        rb.rotation = aimAngle;
    }
}
