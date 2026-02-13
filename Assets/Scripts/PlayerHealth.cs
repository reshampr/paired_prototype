using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    public float damagePerSecond = 20f;
    public float recoverPerSecond = 10f;

    public Image healthFill;   // ← only ONE declaration

    void Update()
    {
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
            Debug.Log("Taking Damage : " + currentHealth);
            currentHealth -= damagePerSecond * Time.deltaTime;
        }
        else if (touchingReal)
        {
            Debug.Log("Healing : " + currentHealth);
            currentHealth += recoverPerSecond * Time.deltaTime;
        }

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthFill != null)
        {
            healthFill.fillAmount = currentHealth / maxHealth;
        }
    }
}
