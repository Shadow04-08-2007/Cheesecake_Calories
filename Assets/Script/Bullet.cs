using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 20;

    void Start()
    {
        Destroy(gameObject, 2f); // auto destroy after time
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy e1 = collision.GetComponent<Enemy>();
        if (e1 != null)
        {
            e1.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        FlyingEnemy e2 = collision.GetComponent<FlyingEnemy>();
        if (e2 != null)
        {
            e2.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }
    }
}