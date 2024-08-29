using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    public int damage;
    private Player_controler player;
    public float lifeTime;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            player = coll.gameObject.GetComponent<Player_controler>();
            if (player != null)
            {
                player.PlayerHealth -= damage;
            }
        }
        Destroy(gameObject);
    }
}
