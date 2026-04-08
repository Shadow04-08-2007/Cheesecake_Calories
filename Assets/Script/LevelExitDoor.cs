using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExitDoor : MonoBehaviour
{
    public string nextSceneName;

    private bool playerInRange = false;
    public bool canExit = false;

    public GameObject pressEText;

    void Update()
    {
        if (playerInRange && canExit && Input.GetKeyDown(KeyCode.E))
        {
            LoadNextLevel();
        }
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;

            if (pressEText != null)
                pressEText.SetActive(true);

            Debug.Log("Press E to enter");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;

            if (pressEText != null)
                pressEText.SetActive(false);
        }
    }
}