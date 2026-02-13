using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTrigger : MonoBehaviour
{
    public GameObject winText;
    public GameObject nextLevelButton;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            // Stop player movement
            SimplePlayerMovement movement = other.GetComponent<SimplePlayerMovement>();
            if (movement != null)
                movement.enabled = false;

            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = Vector2.zero;

            // Show UI
            winText.SetActive(true);
            nextLevelButton.SetActive(true);
        }
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level 2");
    }
}
