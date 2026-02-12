using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class HiddenPlatform : MonoBehaviour
{
    SpriteRenderer sprite;
    // keep a flag only if other code asks about visibility
    bool isVisible;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void SetVisible(bool visible)
    {
        isVisible = visible;
        sprite.enabled = visible;
        // IMPORTANT: do NOT change the layer here.
        // gameObject.layer = LayerMask.NameToLayer( visible ? "Real" : "Hidden" ); // remove this
    }

    public bool IsVisible() => isVisible;
}



//BACK UP
// using UnityEngine;


// // The scripts must be attached to an object that has a SpriteRenderer and boxcollider 2D
// // If forgotten to attched unity add them automatically
// [RequireComponent(typeof(SpriteRenderer))]
// [RequireComponent(typeof(BoxCollider2D))]
// public class HiddenPlatform : MonoBehaviour
// {
//     SpriteRenderer sprite; // controls how the object look
//     BoxCollider2D box; // reference to the collider 
//     bool isVisible; // bool value that represents whether the platform is visible or not

//     void Awake() //runs when the object is created
//     {
//         // Reference for the SpriteRenderer and BoxCollider2D
//         sprite = GetComponent<SpriteRenderer>();
//         box = GetComponent<BoxCollider2D>();
//     }

//     public void SetVisible(bool visible)
//     {
//         isVisible = visible; // to store the state of the platform : visible or invisible
//         sprite.enabled = visible; // true : sprite shows, false : srite disappears

//         // Changes the layer of the hidden platform depending on the value of visible
//         gameObject.layer = LayerMask.NameToLayer( visible ? "Real" : "Hidden" );
//     }

//     public bool IsVisible() => isVisible; // This is used by the Player script 
// }
