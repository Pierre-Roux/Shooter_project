using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletBase : MonoBehaviour
{
    // Start is called before the first frame update
    public int damage;
    private EnemyBase enemy;
    public float lifeTime;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    
    public void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
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
