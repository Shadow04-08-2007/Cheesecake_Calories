using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;

    public int totalEnemiesToSpawn = 5;
    public float spawnInterval = 2f;

    private int enemiesSpawned = 0;
    private int enemiesAlive = 0;

    public LevelExitDoor exitDoor;

    void Start()
    {
        InvokeRepeating("SpawnEnemy", 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (enemiesSpawned >= totalEnemiesToSpawn)
        {
            CancelInvoke("SpawnEnemy");
            return;
        }

        // 🔥 RANDOM SPAWN OFFSET
        Vector3 spawnOffset = new Vector3(
            Random.Range(-2f, 2f),
            Random.Range(-2f, 2f),
            0
        );

        GameObject enemy = Instantiate(
            enemyPrefab,
            transform.position + spawnOffset,
            Quaternion.identity
        );

        // Ground enemy
        Enemy e1 = enemy.GetComponent<Enemy>();
        if (e1 != null)
        {
            e1.player = player;
            e1.portal = this;
        }

        // Flying enemy
        FlyingEnemy e2 = enemy.GetComponent<FlyingEnemy>();
        if (e2 != null)
        {
            e2.player = player;
            e2.portal = this;
        }

        enemiesSpawned++;
        enemiesAlive++;
    }

    public void EnemyDied()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0 && enemiesSpawned >= totalEnemiesToSpawn)
        {
            DestroyPortal();
        }
    }

    void DestroyPortal()
    {
        Debug.Log("Portal Cleared!");

        if (exitDoor != null)
        {
            exitDoor.canExit = true; // 🔥 unlock door
        }

        Destroy(gameObject);
    }
}