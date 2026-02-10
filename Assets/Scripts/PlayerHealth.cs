using System.Collections;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(PlayerController))]
public class PlayerHealth : MonoBehaviour
{
   [Header("Health Settings")]
   public float maxHealth = 100f;
   [Tooltip("Current health value (clamped between 0 and maxHealth)")]
   public float currentHealth = 100f;


   [Header("Rates (per second)")]
   [Tooltip("Damage per second while physically on hidden platforms")]
   public float damagePerSecondWhenHidden = 15f;
   [Tooltip("Additional damage per second while INSIDE the portal view (optional)")]
   public float portalExtraDamagePerSecond = 5f;
   [Tooltip("Healing per second when on normal layer")]
   public float healPerSecondWhenNormal = 10f;


   [Header("Lives & Respawn")]
   public int lives = 3;
   [Tooltip("Position to respawn to on life loss; defaults to starting position")]
   public Vector3 respawnPosition;
   [Tooltip("Invulnerability time after respawn (seconds)")]
   public float respawnInvulnerability = 1.0f;


   [Header("Events (optional HUD/GameManager hookup)")]
   public UnityEvent<float> OnHealthPercentChanged; // sends normalized health (0..1)
   public UnityEvent<int> OnLivesChanged;
   public UnityEvent OnPlayerDied;      // fires when last life lost
   public UnityEvent OnPlayerRespawn;   // fires when respawn occurs


   // Internal
   PlayerController player;
   bool inPortalView = false;
   bool isInvulnerable = false;
   GameManager gm;


   void Awake()
   {
       player = GetComponent<PlayerController>();
       gm = FindObjectOfType<GameManager>();


       // initial values
       currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
       respawnPosition = respawnPosition == Vector3.zero ? transform.position : respawnPosition;


       // ensure listeners are not null
       if (OnHealthPercentChanged == null) OnHealthPercentChanged = new UnityEvent<float>();
       if (OnLivesChanged == null) OnLivesChanged = new UnityEvent<int>();
   }


   void Start()
   {
       // notify initial states
       OnHealthPercentChanged.Invoke(currentHealth / maxHealth);
       OnLivesChanged.Invoke(lives);
   }


   void Update()
   {
       if (gm != null && !gm.isRunning) return; // pause behavior when game manager stops the level


       // handle health drain or heal based on state
       float delta = Time.deltaTime;
       float change = 0f;


       // If invulnerable (immediately after respawn), skip damage but allow healing
       if (isInvulnerable)
       {
           // heal while invulnerable so player isn't punished on respawn
           change += healPerSecondWhenNormal * delta;
       }
       else
       {
           // If player is physically on hidden layer, apply damage
           if (player != null && player.isOnHiddenLayer)
           {
               change -= damagePerSecondWhenHidden * delta;
           }
           else
           {
               // Player is on normal layer (or not on a layer), heal
               change += healPerSecondWhenNormal * delta;
           }


           // Portal view (extra damage) applies regardless of being physically on hidden layer,
           // but can be additive if player is also physically on hidden
           if (inPortalView)
           {
               change -= portalExtraDamagePerSecond * delta;
           }
       }


       if (Mathf.Abs(change) > Mathf.Epsilon)
       {
           ModifyHealth(change);
       }
   }


   void ModifyHealth(float amount)
   {
       float prev = currentHealth;
       currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);


       // fire health changed
       OnHealthPercentChanged.Invoke(currentHealth / maxHealth);


       if (currentHealth <= 0f && prev > 0f)
       {
           // died this frame
           HandleDeath();
       }
   }


   void HandleDeath()
   {
       // lose a life
       lives = Mathf.Max(0, lives - 1);
       OnLivesChanged.Invoke(lives);


       if (lives <= 0)
       {
           // game over immediately
           OnPlayerDied?.Invoke();
           gm?.GameOver(false);
           // optionally disable player controls
           var pc = GetComponent<PlayerController>();
           if (pc != null) pc.enabled = false;
       }
       else
       {
           // respawn
           StartCoroutine(RespawnCoroutine());
       }
   }


   IEnumerator RespawnCoroutine()
   {
       // disable movement briefly if you want (this doesn't disable physics)
       var pc = GetComponent<PlayerController>();
       if (pc != null) pc.enabled = false;


       // move to respawn position
       transform.position = respawnPosition;


       // fully restore health on respawn
       currentHealth = maxHealth;
       OnHealthPercentChanged.Invoke(currentHealth / maxHealth);


       // small invulnerability window
       isInvulnerable = true;
       OnPlayerRespawn?.Invoke();


       // re-enable movement after a frame so physics settle
       yield return null;
       if (pc != null) pc.enabled = true;


       // wait invulnerability duration
       yield return new WaitForSeconds(respawnInvulnerability);
       isInvulnerable = false;
   }


   // Public API for portal integration:
   // - Portal script should call SetPortalState(true) when player is viewing / inside portal
   // - and SetPortalState(false) when leaving portal.
   public void SetPortalState(bool insidePortal)
   {
       inPortalView = insidePortal;
   }


   // Optional manual damage/heal calls (e.g., spikes, pickups)
   public void ApplyDamage(float amount)
   {
       if (isInvulnerable) return;
       ModifyHealth(-Mathf.Abs(amount));
   }


   public void ApplyHeal(float amount)
   {
       ModifyHealth(Mathf.Abs(amount));
   }


   // Optional: change respawn position at checkpoints
   public void SetRespawnPosition(Vector3 pos)
   {
       respawnPosition = pos;
   }
}
