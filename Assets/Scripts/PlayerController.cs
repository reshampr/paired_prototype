using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
   [Header("Movement")]
   [Tooltip("Horizontal speed in units/sec")]
   public float moveSpeed = 5f;


   [Tooltip("Vertical speed in units/sec")]
   public float verticalSpeed = 5f;


   [Tooltip("Enable jump (requires ground check if used)")]
    public bool allowJump = false;
    public float jumpForce = 7f;


   [Header("Layer / Portal")]
   [Tooltip("True = player is physically on the hidden (red) layer")]
   public bool isOnHiddenLayer = false;


   // Called when portal/platform/other wants to notify player entered/exited portal (view mode).
   public UnityEvent OnPortalEnter;
   public UnityEvent OnPortalExit;


   // Optional: expose an event when layer changes
   public UnityEvent OnLayerChangedToHidden;
   public UnityEvent OnLayerChangedToNormal;


   Rigidbody2D rb;
   PlayerHealth healthComponent;
   Vector2 moveVelocity;


   // Optional: simple ground check fields if you want to enable jump in future
   [Header("Ground Check (optional)")]
   public Transform groundCheckPoint;
   public float groundCheckRadius = 0.1f;
   public LayerMask groundLayer;
   bool isGrounded = true;


   void Awake()
   {
       rb = GetComponent<Rigidbody2D>();
       healthComponent = GetComponent<PlayerHealth>();
       if (healthComponent == null)
       {
           Debug.LogWarning("[PlayerController] No PlayerHealth component found on Player. Attach PlayerHealth if you want health/lives.");
       }
   }


   void Update()
   {
       HandleInput();
   }


   void FixedUpdate()
   {
       // apply horizontal movement
       rb.linearVelocity = moveVelocity;


       // optional ground check
       if (groundCheckPoint != null)
       {
           isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer) != null;
       }
   }


   void HandleInput()
   {
       float h = Input.GetAxisRaw("Horizontal"); // A/D
       float v = Input.GetAxisRaw("Vertical");   // W/S


       moveVelocity = new Vector2(
           h * moveSpeed,
           v * verticalSpeed
       );


       // Optional sprite flip
       if (h > 0.01f) transform.localScale = new Vector3(1, 1, 1);
       else if (h < -0.01f) transform.localScale = new Vector3(-1, 1, 1);
   }


   // Called by the portal script when the player enters the portal area (viewing / stepping into portal)
   public void NotifyPortalEntered()
   {
       // portal view mode: player may still be physically on the normal layer
       OnPortalEnter?.Invoke();


       // also tell health about portal state if Health is present
       if (healthComponent != null) healthComponent.SetPortalState(true);
   }


   // Called by the portal script when player leaves portal
   public void NotifyPortalExited()
   {
       OnPortalExit?.Invoke();
       if (healthComponent != null) healthComponent.SetPortalState(false);
   }


   // Explicitly set player's current physical layer (hidden = true, normal = false)
   public void SetLayer(bool hidden)
   {
       bool changed = (isOnHiddenLayer != hidden);
       isOnHiddenLayer = hidden;


       if (changed)
       {
           if (isOnHiddenLayer) OnLayerChangedToHidden?.Invoke();
           else OnLayerChangedToNormal?.Invoke();
       }
   }


   // Collision/triggers with platforms and destination
   private void OnTriggerEnter2D(Collider2D other)
   {
       if (other.CompareTag("HiddenPlatform"))
       {
           SetLayer(true);
       }
       else if (other.CompareTag("NormalPlatform"))
       {
           SetLayer(false);
       }
       else if (other.CompareTag("Portal"))
       {
           // If your portal uses trigger colliders, it can also call NotifyPortalEntered directly.
           NotifyPortalEntered();
       }
       else if (other.CompareTag("Destination"))
       {
           FindObjectOfType<GameManager>()?.LevelComplete();
       }
   }


   private void OnTriggerExit2D(Collider2D other)
   {
       if (other.CompareTag("HiddenPlatform"))
       {
           // leaving a hidden platform: revert to normal by default
           SetLayer(false);
       }
       else if (other.CompareTag("Portal"))
       {
           NotifyPortalExited();
       }
   }
}


