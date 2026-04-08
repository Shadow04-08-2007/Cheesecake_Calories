using UnityEngine;
using System.Collections;

public class WorldFlip : MonoBehaviour
{
    public Transform world;
    public PlayerController player;

    [Header("Flip Settings")]
    public float flipDuration = 0.5f;
    public float minTime = 5f;
    public float maxTime = 12f;

    private bool isFlipped = false;
    private bool isFlipping = false;

    void Start()
    {
        StartCoroutine(RandomFlipRoutine());
    }

    IEnumerator RandomFlipRoutine()
    {
        while (true)
        {
            // ⏱️ wait random time
            float waitTime = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(waitTime);

            // 🔄 flip world
            if (!isFlipping)
            {
                yield return StartCoroutine(FlipWorld());
            }
        }
    }

    IEnumerator FlipWorld()
    {
        isFlipping = true;

        // Stop player movement (important)
        if (player != null)
            player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

        float startZ = world.rotation.eulerAngles.z;
        float targetZ = isFlipped ? 0f : 180f;

        float elapsed = 0f;

        while (elapsed < flipDuration)
        {
            elapsed += Time.deltaTime;

            float z = Mathf.LerpAngle(startZ, targetZ, elapsed / flipDuration);
            world.rotation = Quaternion.Euler(0, 0, z);

            yield return null;
        }

        world.rotation = Quaternion.Euler(0, 0, targetZ);

        isFlipped = !isFlipped;
        isFlipping = false;
    }
}