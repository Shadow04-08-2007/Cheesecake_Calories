using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float sprintMultiplier = 2f;

    [Header("References")]
    public Rigidbody2D rb;
    public Transform firePoint;
    public GameObject bulletPrefab;

    [Header("UI")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI ammoText;

    [Header("Stats")]
    public int health = 100;
    public bool hasGun = false;

    private float moveInput;
    private bool isGrounded;
    private bool isSprinting = false;
    private bool controlsInverted = false;
    private bool isDead = false;

    private bool isFacingRight = true;

    [Header("Game Over")]
    public GameOverUI gameOverUI;
    public Animator animator;

    // 🔥 GUN SYSTEM
    [System.Serializable]
    public class Gun
    {
        public string gunName;
        public int damage;
        public float bulletSpeed;
        public int maxAmmo;
    }

    [Header("Guns")]
    public Gun gun1;
    public Gun gun2;

    private Gun currentGun;
    private int currentGunIndex = 0;
    private int currentAmmo;

    void Start()
    {
        currentGun = gun1;
        currentAmmo = currentGun.maxAmmo;

        UpdateHealthUI();

        if (hasGun)
            UpdateAmmoUI();
        else if (ammoText != null)
            ammoText.text = "";
    }

    void Update()
    {
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        if (isDead) return;

        moveInput = Input.GetAxis("Horizontal");

        if (controlsInverted)
            moveInput = -moveInput;

        // 🔄 FLIP PLAYER
        if (moveInput > 0 && !isFacingRight)
            Flip();
        else if (moveInput < 0 && isFacingRight)
            Flip();

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // Shoot
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        // Sprint
        isSprinting = Input.GetKey(KeyCode.LeftShift);

        // 🔥 SWITCH GUN
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchGun();
        }
    }

    void FixedUpdate()
    {
        if (isDead) return;

        float currentSpeed = isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;

        rb.linearVelocity = new Vector2(currentSpeed * moveInput, rb.linearVelocity.y);
    }

    // 🔄 FLIP
    void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // 🔫 SHOOT
    void Shoot()
    {
        if (hasGun)
        {
            animator.SetTrigger("Attack");
        }
        if (!hasGun)
        {
            Debug.Log("No Gun Yet!");
            return;
        }

        if (currentAmmo <= 0)
        {
            Debug.Log("No Ammo!");
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        float direction = isFacingRight ? 1f : -1f;

        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.linearVelocity = new Vector2(direction * currentGun.bulletSpeed, 0f);

        Bullet b = bullet.GetComponent<Bullet>();
        b.damage = currentGun.damage;

        currentAmmo--;
        UpdateAmmoUI();
    }

    // 🔄 SWITCH GUN
    void SwitchGun()
    {
        if (!hasGun) return;

        currentGunIndex = 1 - currentGunIndex;
        currentGun = (currentGunIndex == 0) ? gun1 : gun2;

        currentAmmo = currentGun.maxAmmo;

        UpdateAmmoUI();

        Debug.Log("Switched to: " + currentGun.gunName);
    }

    // ❤️ DAMAGE
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;

        if (health <= 0)
        {
            health = 0;
            Die();
        }

        Debug.Log("Player Health: " + health);
        UpdateHealthUI();
    }

    void Die()
    {
        isDead = true;

        Debug.Log("PLAYER DEAD");

        rb.linearVelocity = Vector2.zero;
        moveSpeed = 0;

        if (gameOverUI != null)
            gameOverUI.ShowGameOver();
    }

    // 🔄 CONTROL INVERT
    public void InvertControls(float duration)
    {
        StartCoroutine(InvertRoutine(duration));
    }

    IEnumerator InvertRoutine(float duration)
    {
        controlsInverted = true;
        yield return new WaitForSeconds(duration);
        controlsInverted = false;
    }

    // 🧱 GROUND CHECK
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }

    // 📦 AMMO PICKUP
    public void AddAmmo(int amount)
    {
        currentAmmo += amount;

        if (currentAmmo > currentGun.maxAmmo)
            currentAmmo = currentGun.maxAmmo;

        UpdateAmmoUI();
    }

    // 🔫 UNLOCK GUN
    public void UnlockGun()
    {
        hasGun = true;
        currentGun = gun1;
        currentAmmo = currentGun.maxAmmo;
        UpdateAmmoUI();
    }

    // 🖥️ UI
    void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = "Health: " + health;
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = currentGun.gunName + " | Ammo: " + currentAmmo;
    }

    public void ResetVelocity()
    {
        rb.linearVelocity = Vector2.zero;
    }
}