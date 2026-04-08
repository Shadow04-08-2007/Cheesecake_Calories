using UnityEngine;
using UnityEngine.UI;

public class FlyingEnemy : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;

    public int maxHealth = 40;
    private int health;

    public int damage = 10;

    public Portal portal;

    public Slider healthBar;

    void Start()
    {
        health = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = health;
        }
    }

    void Update()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;

        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;

        if (healthBar != null)
            healthBar.value = health;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (portal != null)
        {
            portal.EnemyDied();
        }

        Destroy(gameObject);
    }

    private float lastAttackTime;
    public float attackCooldown = 1f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Time.time > lastAttackTime + attackCooldown)
            {
                collision.GetComponent<PlayerController>().TakeDamage(damage);
                lastAttackTime = Time.time;
            }
        }
    }
}