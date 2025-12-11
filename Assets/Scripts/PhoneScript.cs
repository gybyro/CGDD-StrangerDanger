using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PhoneScript : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Hover Brightness")]
    public float normalBrightness = 0.8f;
    public float hoverBrightness = 1.2f;

    [Header("Vibration")]
    public float vibrationDuration = 0.5f;
    public float vibrationStrength = 10f;
    public float startDelay = 3f;
    public float loopTime = 5f;

    [Header("Click Animation")]
    public float scaleMultiplier = 2.5f;   // 250%
    public float moveX = -100f;            // -100 X movement
    public float animDuration = 0.5f;

    [Header("Screen Change After Phone Click")]
    public Sprite newScreenSprite;         // first screen (after opening phone)

    [Header("Second Screen After Button Click")]
    public Sprite secondScreenSprite;      // second screen (after order button)

    [Header("Sound")]
    public AudioClip screenChangeSound;    // plays on screen changes
    private AudioSource audioSource;

    [Header("Objects That Appear After Phone Click")]
    public GameObject firstOrderButton;    // the first button shown after phone opens

    [Header("Text Revealed After Order Button Click")]
    public GameObject textToShow;          // text/panel shown after clicking firstOrderButton

    [Header("Buttons After Text")]
    public GameObject acceptButton;        // appears after firstOrderButton click
    public GameObject declineButton;       // appears after firstOrderButton click

    [Header("Accept Order Scene")]
    public string acceptSceneName;         // name of scene to load on Accept

    private Image img;
    private Color originalColor;
    private RectTransform rect;
    private Vector3 originalScale;
    private Vector2 originalPos;
    private Quaternion originalRot;

    private bool clicked = false;

    void Start()
    {
        img = GetComponent<Image>();
        rect = GetComponent<RectTransform>();

        originalColor = img.color;
        originalScale = rect.localScale;
        originalPos = rect.anchoredPosition;
        originalRot = transform.rotation;

        // Audio setup
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Start slightly darker for hover contrast
        SetBrightness(normalBrightness);

        // Hide / wire first order button
        if (firstOrderButton != null)
        {
            firstOrderButton.SetActive(false);

            Button btn = firstOrderButton.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(OnFirstOrderButtonClicked);
            }
        }

        // Hide text
        if (textToShow != null)
            textToShow.SetActive(false);

        // Hide / wire accept & decline buttons
        if (acceptButton != null)
        {
            acceptButton.SetActive(false);
            Button btn = acceptButton.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(OnAcceptOrderClicked);
            }
        }

        if (declineButton != null)
        {
            declineButton.SetActive(false);
            Button btn = declineButton.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(OnDeclineOrderClicked);
            }
        }

        // Start vibration loop
        InvokeRepeating(nameof(StartVibration), startDelay, loopTime);
    }

    // -------------------------
    // HOVER
    // -------------------------
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (clicked) return;
        SetBrightness(hoverBrightness);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (clicked) return;
        SetBrightness(normalBrightness);
    }

    void SetBrightness(float mult)
    {
        img.color = new Color(
            Mathf.Clamp01(originalColor.r * mult),
            Mathf.Clamp01(originalColor.g * mult),
            Mathf.Clamp01(originalColor.b * mult),
            originalColor.a
        );
    }

    // -------------------------
    // VIBRATION
    // -------------------------
    void StartVibration()
    {
        if (clicked) return;   // stop vibration forever after click
        StartCoroutine(Vibrate());
    }

    System.Collections.IEnumerator Vibrate()
    {
        float t = 0f;
        while (t < vibrationDuration)
        {
            float rot = Mathf.Sin(Time.time * 60f) * vibrationStrength;
            transform.rotation = Quaternion.Euler(0, 0, rot);
            t += Time.deltaTime;
            yield return null;
        }
        transform.rotation = originalRot;
    }

    // -------------------------
    // PHONE CLICK
    // -------------------------
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!clicked)
        {
            clicked = true;
            CancelInvoke(nameof(StartVibration)); // stop ALL future vibration
            SetBrightness(normalBrightness);      // override hover
        }

        StartCoroutine(ClickAnimation());
    }

    // -------------------------
    // PHONE CLICK ANIMATION
    // -------------------------
    System.Collections.IEnumerator ClickAnimation()
    {
        float t = 0f;

        Vector3 startScale = rect.localScale;
        Vector3 endScale = originalScale * scaleMultiplier;

        Vector2 startPos = rect.anchoredPosition;
        Vector2 endPos = originalPos + new Vector2(moveX, 0);

        while (t < animDuration)
        {
            t += Time.deltaTime;
            float p = t / animDuration;

            rect.localScale = Vector3.Lerp(startScale, endScale, p);
            rect.anchoredPosition = Vector2.Lerp(startPos, endPos, p);

            yield return null;
        }

        // Final pose
        rect.localScale = endScale;
        rect.anchoredPosition = endPos;

        // First screen change (phone opened)
        if (newScreenSprite != null)
            img.sprite = newScreenSprite;

        // Brightness = 2
        img.color = new Color(
            Mathf.Clamp01(originalColor.r * 2f),
            Mathf.Clamp01(originalColor.g * 2f),
            Mathf.Clamp01(originalColor.b * 2f),
            originalColor.a
        );

        // Play sound
        if (screenChangeSound != null)
            audioSource.PlayOneShot(screenChangeSound);

        // Show the first order button
        if (firstOrderButton != null)
            firstOrderButton.SetActive(true);
    }

    // -------------------------
    // FIRST ORDER BUTTON CLICK
    // -------------------------
    private void OnFirstOrderButtonClicked()
    {
        // Hide the first button
        if (firstOrderButton != null)
            firstOrderButton.SetActive(false);

        // Show the text
        if (textToShow != null)
            textToShow.SetActive(true);

        // Change to second screen image
        if (secondScreenSprite != null)
            img.sprite = secondScreenSprite;

        // Optional: play same sound again
        if (screenChangeSound != null)
            audioSource.PlayOneShot(screenChangeSound);

        // Show accept/decline buttons
        if (acceptButton != null)
            acceptButton.SetActive(true);

        if (declineButton != null)
            declineButton.SetActive(true);
    }

    // -------------------------
    // ACCEPT ORDER
    // -------------------------
    private void OnAcceptOrderClicked()
    {
        if (!string.IsNullOrEmpty(acceptSceneName))
        {
            SceneManager.LoadScene(acceptSceneName);
        }
        else
        {
            Debug.LogWarning("PhoneScript: acceptSceneName is empty, cannot load scene.");
        }
    }

    // -------------------------
    // DECLINE ORDER
    // -------------------------
    private void OnDeclineOrderClicked()
    {
        // Hide text
        if (textToShow != null)
            textToShow.SetActive(false);

        // Hide accept/decline buttons
        if (acceptButton != null)
            acceptButton.SetActive(false);

        if (declineButton != null)
            declineButton.SetActive(false);

        // Show the first order button again
        if (firstOrderButton != null)
            firstOrderButton.SetActive(true);

        // Return phone to "menu" state (first opened screen)
        if (newScreenSprite != null)
            img.sprite = newScreenSprite;

        // Keep brightness at 2 (since phone is still open)
        img.color = new Color(
            Mathf.Clamp01(originalColor.r * 2f),
            Mathf.Clamp01(originalColor.g * 2f),
            Mathf.Clamp01(originalColor.b * 2f),
            originalColor.a
        );
    }
}
