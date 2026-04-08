using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public int health = 50;
    public int damage = 10;

    public Slider healthBar;
    public int maxHealth = 50;

    public Portal portal;

    void Update()
    {
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                speed * Time.deltaTime
            );
        }
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject
                .GetComponent<PlayerController>()
                .TakeDamage(damage);
        }
    }

    void Start()
    {
        health = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
    }
}