using UnityEngine;

public class ReflectivePart : MonoBehaviour
{
    [SerializeField] private float repulsionForce;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Rigidbody2D bulletRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                Vector2 repulsionDirection = (collision.transform.position - transform.position).normalized;

                float initialSpeed = bulletRb.velocity.magnitude;
                bulletRb.velocity = repulsionDirection * initialSpeed * repulsionForce;

                float angle = Mathf.Atan2(repulsionDirection.y, repulsionDirection.x) * Mathf.Rad2Deg + 90f;
                collision.transform.rotation = Quaternion.Euler(0, 0, angle);

            }
        }
    }
}
