using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;   
using System.Collections;

public class GameManager : MonoBehaviour
{
    public float levelTime = 60f;
    float timeLeft;
    public bool isRunning = true;

    HUDController hud;

    //Character GameObject
    public GameObject player;

    public GameObject restartButton;

    //delay before showing restart UI so animations can play
    public float showRestartDelay = 0.25f;

    private void Awake()
    {
        timeLeft = levelTime;
        hud = FindObjectOfType<HUDController>();

        if (player == null)
        {
            var found = GameObject.FindWithTag("Player");
            if (found != null) player = found;
        }

        //hide restart button on start
        if (restartButton != null) restartButton.SetActive(false);
    }

    private void Update()
    {
        if (!isRunning) return;

        timeLeft -= Time.deltaTime;
        hud?.UpdateTimer(timeLeft);

        if (timeLeft <= 0f)
        {
            //timer ran out -> game over (failure)
            GameOver(false);
        }

        //keyboard shortcut to restart anytime for quick testing (R)
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
    }

    public void LevelComplete()
    {
        isRunning = false;
        // intentionally not updating HUD for victory - handled elsewhere if needed
        Debug.Log("Level Complete!");
    }

    bool gameEnded = false;

    public void GameOver(bool playerDied)
    {
        if (gameEnded) return;

        gameEnded = true;
        isRunning = false;

        Debug.Log("GameOver called. playerDied = " + playerDied);

        StopPlayerMovement();
        hud?.ShowGameOver(playerDied);
    }

    IEnumerator ShowRestartAfterDelay(float t)
    {
        yield return new WaitForSecondsRealtime(t); //realtime so UI works even if timeScale changed
        if (restartButton != null) restartButton.SetActive(true);
    }

    void StopPlayerMovement()
    {
        if (player == null)
        {
            Debug.LogWarning("[GameManager] player GameObject not assigned and no 'Player' tag found.");
            return;
        }

        //disable PlayerController if present
        var pc = player.GetComponent<PlayerController>();
        if (pc != null) pc.enabled = false;

        //disable SimplePlayerMovement if present
        var spm = player.GetComponent<SimplePlayerMovement>();
        if (spm != null) spm.enabled = false;

        //zero out physics on root Rigidbody2D (if present)
        var rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        foreach (var childRb in player.GetComponentsInChildren<Rigidbody2D>())
        {
            childRb.linearVelocity = Vector2.zero;
            childRb.angularVelocity = 0f;
        }
    }

    // called from UI Button or keyboard
    public void RestartLevel()
    {
        Debug.Log("[GameManager] RestartLevel called.");

        // Make sure any timeScale freeze is lifted
        Time.timeScale = 1f;

        // Hide restart UI (if managed by GameManager)
        if (restartButton != null) restartButton.SetActive(false);

        // Reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}