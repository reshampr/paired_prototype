using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    void Start()
    {
        // Hides all the hidden layer automatically at the very start
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("HiddenPlatform"))
        {
            SetHiddenVisual(obj, false);
        }
    }

    //Runs when Protal's trigger collider overllaps on the other collider
    private void OnTriggerEnter2D(Collider2D other) 
    {
        // Only reacts to the object which has tag = "HiddenPlatform"
        // For us the hidden layer has "HiddenPlatform" tag
        if (other.CompareTag("HiddenPlatform")) 
        {
            //Makes the platform visible, turns on the SpriteRenderer
            SetHiddenVisual(other.gameObject, true);

        }
    }

    //Runs when portal stops overlapping with the hidden platform
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("HiddenPlatform"))
        {
            //Makes the platform invisible, turns off the SpriteRenderer
            SetHiddenVisual(other.gameObject, false);
        }
    }

    // Helper function
    void SetHiddenVisual(GameObject obj, bool visible)
    {
        // Finds platoforms SpriteRenderer
        var sr = obj.GetComponent<SpriteRenderer>();
        // Based on the bool value, it enables or disables
        if (sr) sr.enabled = visible;
    }
}
