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
        StopPlayerMovement();
        hud?.ShowLevelComplete();
        Debug.Log("Level Complete!");
        //if completed successfully, ensure restart UI isn't shown
        if (restartButton != null) restartButton.SetActive(false);
    }

    public void GameOver(bool success)
    {
        //make sure we only run once
        if (!isRunning) return;
        isRunning = false;

        StopPlayerMovement();

        if (success)
        {
            hud?.ShowVictory();
            //no restart on success
            if (restartButton != null) restartButton.SetActive(false);
        }
        else
        {
            hud?.ShowGameOver();

            //show the restart button after an optional delay (so any "game over" animation/text can appear)
            if (restartButton != null)
            {
                if (showRestartDelay <= 0f)
                    restartButton.SetActive(true);
                else
                    StartCoroutine(ShowRestartAfterDelay(showRestartDelay));
            }
        }

        Debug.Log("Game Over. Success: " + success);
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

    //called from UI Button or keyboard
    public void RestartLevel()
    {
        if (restartButton != null) restartButton.SetActive(false);

        //reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
