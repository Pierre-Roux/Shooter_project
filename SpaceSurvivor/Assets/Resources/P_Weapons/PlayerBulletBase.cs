using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletBase : MonoBehaviour
{
[Header("Param")] 
    [SerializeField] public int damage;
    [SerializeField] public float lifeTime;

    [HideInInspector] private EnemyBase enemy;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    
    public void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            Debug.Log("Contact");
            enemy = coll.gameObject.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                if (!enemy.IsDead)
                {
                    enemy.TakeDamage(damage);
                    Destroy(gameObject);
                }
            }
        }
        if (coll.gameObject.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
    }

    
}
