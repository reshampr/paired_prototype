using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Rigidbody2D))]
// [RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
   public float moveSpeed = 5f;


   Rigidbody2D rb;
   Vector2 moveInput;
   Vector3 startPos;

   void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
    }

    void Update() {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(h,v);
    }

    void FixedUpdate() {
        rb.linearVelocity = moveInput * moveSpeed;    
    }

    public void ResetToStart()
    {
        rb.linearVelocity = Vector2.zero;
        transform.position = startPos;
    }

    // public void SetHiddenLayer(bool hidden)
    // {
    //     if (hidden)
    //     {
    //         gameObject.layer = LayerMask.NameToLayer("PlayerHidden");
    //     }
    //     else
    //     {
    //         gameObject.layer = LayerMask.NameToLayer("PlayerNormal");
    //     }
    // }
}


