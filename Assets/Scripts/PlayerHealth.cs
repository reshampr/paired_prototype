using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    public float damagePerSecond = 20f;
    public float recoverPerSecond = 10f;

    // Optional UI image for health (can be unused if you prefer Slider on HUD)
    public Image healthFill;

    // prevent calling Die() repeatedly
    bool isDead = false;

    void Awake()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }

    void Update()
    {
        // don't change health if already dead
        if (isDead) return;

        bool touchingHidden = false;
        bool touchingReal = false;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.1f);

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.layer == LayerMask.NameToLayer("Hidden"))
                touchingHidden = true;

            if (hit.gameObject.layer == LayerMask.NameToLayer("Real"))
                touchingReal = true;
        }

        if (touchingHidden)
        {
            // taking damage
            currentHealth -= damagePerSecond * Time.deltaTime;
        }
        else if (touchingReal)
        {
            // recovering
            currentHealth += recoverPerSecond * Time.deltaTime;
        }

        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (healthFill != null)
        {
            healthFill.fillAmount = currentHealth / maxHealth;
        }

        if (currentHealth <= 0f && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        // Optional: disable player movement immediately
        var pc = GetComponent<PlayerController>();
        if (pc != null) pc.enabled = false;

        var simpleMove = GetComponent<SimplePlayerMovement>();
        if (simpleMove != null) simpleMove.enabled = false;

        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            // rb.simulated = false; // optional, but may stop collision callbacks
        }

        // Notify GameManager that the player died / game over
        var gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.GameOver(true);
        }
        else
        {
            Debug.LogWarning("[PlayerHealth] GameManager not found to notify GameOver.");
        }
    }
}
