using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    void Start()
    {
        // Make sure all hidden platforms start invisible (and on Hidden layer)
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("HiddenPlatform"))
        {
            SetHiddenVisual(obj, false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Enter with: " + other.name);

        if (other.CompareTag("HiddenPlatform"))
        {
            SetHiddenVisual(other.gameObject, true);
        }
    }

 
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("HiddenPlatform"))
        {
            SetHiddenVisual(other.gameObject, false);
        }
    }

    void SetHiddenVisual(GameObject obj, bool visible)
    {
        // Prefer the HiddenPlatform script (handles sprite + layer)
        HiddenPlatform hp = obj.GetComponent<HiddenPlatform>();
        if (hp != null)
        {
            hp.SetVisible(visible);
            return;
        }

        // Fallback: toggle sprite only (not recommended)
        var sr = obj.GetComponent<SpriteRenderer>();
        if (sr) sr.enabled = visible;
    }
}
