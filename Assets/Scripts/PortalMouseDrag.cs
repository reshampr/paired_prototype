using UnityEngine;
using UnityEngine.InputSystem;

public class PortalMouseFollow : MonoBehaviour
{
    // Stores a reference to the camera
    // To convert mouse screen co-ordinates to global co-ordinates
    Camera cam;

    void Start()
    {
        cam = Camera.main; // Runs when the scene starts
    }

    void Update()
    {
        // Ensures that a mouse device exists
        // Prevents error, if a device has no mouse
        if (Mouse.current == null) return;

        // Gets the position of the mouse
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();

        //Converts screen co-ordinates into world co-ordinates, uses camera's projection
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(mouseScreenPos);

        // Forces the portal to stay in 2D plane
        mouseWorldPos.z = 0f;

        //instantly moves the portal where the mouse is
        transform.position = mouseWorldPos;
    }
}
