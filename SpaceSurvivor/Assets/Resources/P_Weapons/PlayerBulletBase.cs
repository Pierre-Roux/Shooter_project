using UnityEngine;

public class PlayerBulletBase : MonoBehaviour
{
[Header("Param")] 
    [SerializeField] public float damage;
    [SerializeField] public float lifeTime;

    [HideInInspector] public GameObject enemyToIgnore;
    [HideInInspector] private EnemyBase enemy;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    
    public void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
    }

    
}
