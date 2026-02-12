using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SimplePlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector3 startPos;

    Rigidbody2D rb;
    Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(h, v).normalized;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed; // use velocity (rb.linearVelocity is older)
    }

    void GoToStart()
    {
        rb.linearVelocity = Vector2.zero;
        transform.position = startPos;
    }

    // Solid collider collision (platform collider is NOT a trigger)
    void OnCollisionEnter2D(Collision2D collision)
    {
        CheckAndTeleportIfHiddenPlatform(collision.gameObject);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        CheckAndTeleportIfHiddenPlatform(collision.gameObject);
    }

    // In case some hidden platforms are triggers (not recommended for blocking)
    void OnTriggerEnter2D(Collider2D other)
    {
        CheckAndTeleportIfHiddenPlatform(other.gameObject);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        CheckAndTeleportIfHiddenPlatform(other.gameObject);
    }

    void CheckAndTeleportIfHiddenPlatform(GameObject other)
    {
        var hp = other.GetComponent<HiddenPlatform>();
        if (hp != null && !hp.IsVisible())
        {
            // Debug.Log("Touching hidden platform while hidden -> go to start");
            GoToStart();
        }
    }
}