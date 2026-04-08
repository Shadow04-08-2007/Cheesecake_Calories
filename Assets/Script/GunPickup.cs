using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public GameObject pickupText; // optional UI

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            player.UnlockGun(); // 🔥 clean call

            Debug.Log("Gun Picked!");

            if (pickupText != null)
                pickupText.SetActive(false);

            Destroy(gameObject);
        }
    }
}