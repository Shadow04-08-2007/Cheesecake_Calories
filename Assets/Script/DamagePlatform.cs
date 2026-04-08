using UnityEngine;

public class DamagePlatform : MonoBehaviour
{
    [Header("Damage Settings")]
    public int normalDamage = 10;
    public float damageInterval = 1f;

    [Header("Knockback Settings")]
    public float knockbackForce = 7f;

    private float lastDamageTime;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (Time.time > lastDamageTime + damageInterval)
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            if (player != null)
            {
                // 🔥 RANDOM EFFECT
                int randomEffect = Random.Range(0, 3); // 0,1,2

                switch (randomEffect)
                {
                    case 0:
                        // 💥 NORMAL DAMAGE
                        GetComponent<SpriteRenderer>().color = Color.red; 
                        
                        Debug.Log("Platform: Normal Damage");
                        player.TakeDamage(normalDamage);
                        break;

                    case 1:
                        // 💀 INSTANT KILL
                        GetComponent<SpriteRenderer>().color = Color.black;
                        Debug.Log("Platform: Instant Kill");
                        player.TakeDamage(999);
                        break;

                    case 2:
                        // 🚀 KNOCKBACK + SMALL DAMAGE
                        GetComponent<SpriteRenderer>().color = Color.yellow;
                        Debug.Log("Platform: Knockback");

                        player.TakeDamage(normalDamage);

                        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                        if (rb != null)
                        {
                            rb.linearVelocity = new Vector2(
                                Random.Range(-1f, 1f) * 5f,
                                knockbackForce
                            );
                        }
                        break;
                }

                lastDamageTime = Time.time;
            }
        }
    }
}