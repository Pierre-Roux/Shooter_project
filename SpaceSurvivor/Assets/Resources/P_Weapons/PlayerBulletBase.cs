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
        if (coll.gameObject.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
    }

    
}
