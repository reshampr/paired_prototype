using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class GameManager : MonoBehaviour
{
   public float levelTime = 60f;
   float timeLeft;
   public bool isRunning = true;


   HUDController hud;


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
       hud?.ShowLevelComplete();
       // do win logic here:
       Debug.Log("Level Complete!");
   }


   public void GameOver(bool success)
   {
       isRunning = false;
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


   // Optional: restart
   public void RestartLevel()
   {
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
   }
}
