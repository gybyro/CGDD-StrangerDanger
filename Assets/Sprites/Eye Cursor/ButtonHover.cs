using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Brightness")]
    public float normalBrightness = 1f;
    public float hoverBrightness = 1.2f;

    [Header("Sound")]
    public AudioClip hoverSound;  // Drag sound here
    private AudioSource audioSource;

    private Image img;
    private Color originalColor;

    void Awake()
    {
        img = GetComponent<Image>();
        originalColor = img.color;

        // Add an AudioSource to this button
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    // ---------------- HOVER ENTER ----------------
    public void OnPointerEnter(PointerEventData eventData)
    {
        SetBrightness(hoverBrightness);

        // Play hover sound once
        if (hoverSound != null)
            audioSource.PlayOneShot(hoverSound);

        UICursorAnimator.Instance.OnHoverStart();
        CursorFadeController.Instance.OnHoverStart();
    }

    // ---------------- HOVER EXIT ----------------
    public void OnPointerExit(PointerEventData eventData)
    {
        SetBrightness(normalBrightness);

        UICursorAnimator.Instance.OnHoverEnd();
        CursorFadeController.Instance.OnHoverEnd();
    }

    // ---------------- BRIGHTNESS ----------------
    private void SetBrightness(float mult)
    {
        img.color = new Color(
            Mathf.Clamp01(originalColor.r * mult),
            Mathf.Clamp01(originalColor.g * mult),
            Mathf.Clamp01(originalColor.b * mult),
            originalColor.a // keep alpha the same
        );
    }
}
