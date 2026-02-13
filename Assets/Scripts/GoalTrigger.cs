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

            // Disable movement (try known movement scripts)
            var simpleMove = other.GetComponent<SimplePlayerMovement>();
            if (simpleMove != null) simpleMove.enabled = false;

            var playerController = other.GetComponent<PlayerController>();
            if (playerController != null) playerController.enabled = false;

            // Stop physics motion
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                // optionally: rb.simulated = false;
            }

            // Tell the GameManager the level is complete (stops timer / shows HUD)
            var gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                gm.LevelComplete();
            }
            else
            {
                Debug.LogWarning("[GoalTrigger] GameManager not found in scene.");
            }

            // Show local UI (if you still want these)
            if (winText != null) winText.SetActive(true);
            if (nextLevelButton != null) nextLevelButton.SetActive(true);
        }
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level 2");
    }
}
