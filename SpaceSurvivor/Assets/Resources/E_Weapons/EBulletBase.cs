using UnityEngine;

public class EBulletBase : MonoBehaviour
{
[Header("Base_Param")] 
    [SerializeField] public int damage;
    [SerializeField] public float lifeTime;

    [HideInInspector] private Player_controler player;

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
                player.TakeDamage(damage,"Range");
            }
        }
        Destroy(gameObject);
    }
}
