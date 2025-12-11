using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    [Header("Screen Change After Click")]
    public Sprite newScreenSprite;

    [Header("Sound")]
    public AudioClip screenChangeSound;
    private AudioSource audioSource;

    [Header("Objects That Appear After Click")]
    public GameObject objectToShow;   // SetActive(true) after the click animation

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

        // Create audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Start darker for hover
        SetBrightness(normalBrightness);

        // Hide button/object until phone is clicked
        if (objectToShow != null)
            objectToShow.SetActive(false);

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
    // CLICK HANDLER
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
    // CLICK ANIMATION
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

        // Final positions
        rect.localScale = endScale;
        rect.anchoredPosition = endPos;

        // Change screen image
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

        // Show the button/object
        if (objectToShow != null)
            objectToShow.SetActive(true);
    }
}
