using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
   public float levelTime = 60f;
   float timeLeft;
   public bool isRunning = true;

   HUDController hud;

   // ADD THIS: assign your Player GameObject's PlayerController component in the Inspector
   public PlayerController playerController;

   private void Awake()
   {
       timeLeft = levelTime;
       hud = FindObjectOfType<HUDController>();
   }

   private void Update()
   {
       if (!isRunning) return;

       timeLeft -= Time.deltaTime;
       hud?.UpdateTimer(timeLeft);

       if (timeLeft <= 0f)
       {
           GameOver(false);
       }
   }

   public void LevelComplete()
   {
       isRunning = false;

       // stop player movement
       StopPlayerMovement();

       hud?.ShowLevelComplete();
       Debug.Log("Level Complete!");
   }

   public void GameOver(bool success)
   {
       isRunning = false;

       // stop player movement
       StopPlayerMovement();

       if (success)
       {
           hud?.ShowVictory();
       }
       else
       {
           hud?.ShowGameOver();
       }
       Debug.Log("Game Over. Success: " + success);
   }

   void StopPlayerMovement()
   {
       if (playerController != null)
       {
           // disable the MonoBehaviour so Update/FixedUpdate stop running
           playerController.enabled = false;

           // also zero out physics so player doesn't keep sliding
           var rb = playerController.GetComponent<Rigidbody2D>();
           if (rb != null)
           {
               rb.linearVelocity = Vector2.zero;
               rb.angularVelocity = 0f;
               // Optionally fully pause physics on this body:
               // rb.simulated = false;
               // OR: rb.bodyType = RigidbodyType2D.Kinematic;
           }
       }
       else
       {
           Debug.LogWarning("[GameManager] playerController not assigned - cannot disable movement.");
       }
   }

   // Optional: restart
   public void RestartLevel()
   {
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
   }
}
