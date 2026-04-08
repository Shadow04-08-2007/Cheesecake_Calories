using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public PlayerController player;

    public float minTime = 8f;
    public float maxTime = 15f;

    public float glitchDuration = 4f;

    void Start()
    {
        StartCoroutine(RandomGlitchLoop());
    }

    IEnumerator RandomGlitchLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(waitTime);

            yield return StartCoroutine(TriggerGlitch());
        }
    }

    IEnumerator TriggerGlitch()
    {
        Debug.Log("GLITCH START");

        // Invert controls
        player.InvertControls(glitchDuration);

        // Flip gravity (UP)
        Physics2D.gravity = new Vector2(0, 9.8f);

        // Push player upward
        player.rb.linearVelocity = new Vector2(player.rb.linearVelocity.x, 5f);

        // Push all enemies upward
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy e in enemies)
        {
            Rigidbody2D erb = e.GetComponent<Rigidbody2D>();
            if (erb != null)
            {
                erb.linearVelocity = new Vector2(erb.linearVelocity.x, 5f);
            }
        }

        yield return new WaitForSeconds(glitchDuration);

        // Reset gravity
        Physics2D.gravity = new Vector2(0, -9.8f);

        Debug.Log("GLITCH END");
    }
}