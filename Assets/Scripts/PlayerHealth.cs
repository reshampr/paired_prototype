using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth = 100f;
    public int lives = 3;
    public float damagePerSecondWhenHidden = 15f;
    public float healPerSecondWhenNormal = 10f;

    PlayerController player;

    void Awake()
    {
        player = GetComponent<PlayerController>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        float delta = Time.deltaTime;

        bool isHidden =
            gameObject.layer == LayerMask.NameToLayer("PlayerHidden");

        if (isHidden)
        {
            currentHealth -= damagePerSecondWhenHidden * delta;
        }
        else
        {
            currentHealth += healPerSecondWhenNormal * delta;
        }

        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }
}
