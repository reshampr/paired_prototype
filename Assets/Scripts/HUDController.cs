using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public Text timerText;
    public Slider healthBar;
    public Text livesText;

    // Optional: If you have a button to restart, assign it in Inspector.
    public GameObject restartButton;

    // messageText is optional in inspector — if missing, we'll create one at runtime.
    public Text messageText;

    PlayerHealth playerHealth;

    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player) playerHealth = player.GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        var canvas = GetComponentInParent<Canvas>();
        if (canvas == null) canvas = FindObjectOfType<Canvas>();

        if (canvas != null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = Mathf.Max(canvas.sortingOrder, 100);
        }

        if (timerText != null) timerText.text = "Time Left : 1:00";
        if (restartButton != null) restartButton.SetActive(false);

        if (messageText == null)
        {
            var found = GameObject.Find("MessageText");
            if (found != null) messageText = found.GetComponent<Text>();
        }

        if (messageText == null)
        {
            Debug.Log("[HUD] messageText not assigned - creating one at runtime.");

            if (canvas == null)
            {
                var newCanvasGO = new GameObject("HUD_Canvas");
                canvas = newCanvasGO.AddComponent<Canvas>();
                newCanvasGO.AddComponent<CanvasScaler>();
                newCanvasGO.AddComponent<GraphicRaycaster>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.sortingOrder = 200;
            }

            var msgGO = new GameObject("MessageText", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
            msgGO.transform.SetParent(canvas.transform, false);

            var rt = msgGO.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = new Vector2(900, 150);

            messageText = msgGO.GetComponent<Text>();
            messageText.raycastTarget = false;
            messageText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            messageText.fontSize = 48;
            messageText.alignment = TextAnchor.MiddleCenter;
            messageText.color = Color.white;
            messageText.text = "";
        }
        else
        {
            messageText.gameObject.SetActive(false);
        }

        Debug.Log($"[HUD] Start: timerText assigned? {(timerText != null)}; messageText assigned? {(messageText != null)}; restart assigned? {(restartButton != null)}");

        if (messageText != null) messageText.transform.SetAsLastSibling();

        if (healthBar != null && playerHealth != null)
            healthBar.value = playerHealth.currentHealth / playerHealth.maxHealth;
    }

    private void Update()
    {
        if (playerHealth != null && healthBar != null)
        {
            healthBar.value = playerHealth.currentHealth / playerHealth.maxHealth;
        }
    }

    public void UpdateTimer(float secondsLeft)
    {
        if (timerText)
        {
            secondsLeft = Mathf.Max(0f, secondsLeft);
            int secs = Mathf.CeilToInt(secondsLeft);
            int minutes = secs / 60;
            int seconds = secs % 60;
            timerText.text = $"Time Left : {minutes}:{seconds:00}";
        }
    }

    public void ShowGameOver(bool playerDied)
{
    if (messageText != null)
    {
        messageText.gameObject.SetActive(true);

        if (playerDied)
            messageText.text = "Player has died!";
        else
            messageText.text = "Game Over!";

        messageText.color = Color.white;
        messageText.fontSize = 30;
        messageText.fontStyle = FontStyle.Bold;
        messageText.alignment = TextAnchor.MiddleCenter;

        var rt = messageText.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = new Vector2(1000, 200);
        }

        messageText.transform.SetAsLastSibling();
    }

    if (restartButton != null)
        restartButton.SetActive(true);

    if (timerText != null)
        timerText.color = Color.red;
}

}
