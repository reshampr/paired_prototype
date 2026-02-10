using UnityEngine;
using UnityEngine.UI;


public class HUDController : MonoBehaviour
{
   public Text timerText;
   public Slider healthBar;
   public Text livesText;
   public Text messageText;


   PlayerHealth playerHealth;


   private void Start()
   {
       var player = GameObject.FindGameObjectWithTag("Player");
       if (player) playerHealth = player.GetComponent<PlayerHealth>();
   }


   private void Update()
   {
       if (playerHealth != null && healthBar != null)
       {
           healthBar.value = playerHealth.currentHealth / playerHealth.maxHealth;
           if (livesText) livesText.text = "Lives: " + playerHealth.lives;
       }
   }


   public void UpdateTimer(float secondsLeft)
   {
       if (timerText)
       {
           secondsLeft = Mathf.Max(0, secondsLeft);
           timerText.text = "Time: " + Mathf.CeilToInt(secondsLeft).ToString();
       }
   }


   public void ShowGameOver()
   {
       if (messageText) messageText.text = "Game Over";
   }


   public void ShowLevelComplete()
   {
       if (messageText) messageText.text = "Level Complete!";
   }


   public void ShowVictory()
   {
       if (messageText) messageText.text = "Victory!";
   }
}
