using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    public float visibleTime = 2f;
    public float hiddenTime = 2f;

    private SpriteRenderer sr;
    private Collider2D col;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        float randomDelay = Random.Range(0f, 2f);
        Invoke("StartToggle", randomDelay);
    }

    void StartToggle()
    {
        StartCoroutine(TogglePlatform());
    }

    System.Collections.IEnumerator TogglePlatform()
    {
        while (true)
        {
            // Visible
            sr.enabled = true;
            col.enabled = true;
            yield return new WaitForSeconds(visibleTime);

            // Hidden
            sr.enabled = false;
            col.enabled = false;
            yield return new WaitForSeconds(hiddenTime);
        }
    }
}