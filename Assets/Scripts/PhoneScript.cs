using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PhoneScript : MonoBehaviour,
    IPointerClickHandler,
    IPointerEnterHandler,
    IPointerExitHandler
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
    public float scaleMultiplier = 2.5f;
    public float moveX = -100f;
    public float animDuration = 0.5f;

    [Header("Start Images")]
    public Sprite firstVisitStartImage;    // shown before click (first time)
    public Sprite repeatVisitStartImage;   // shown before click (every time after)

    [Header("Flow Images")]
    public Sprite callScreenImage;         // first visit after click (decline)
    public Sprite pizzaScreenImage;        // pizza order screen
    public Sprite acceptScreenImage;       // accept screen

    [Header("Sound")]
    public AudioClip screenChangeSound;

    [Header("Buttons")]
    public GameObject declineCallButton;   // invisible but clickable
    public GameObject pizzaOrderButton;
    public GameObject acceptOrderButton;

    public System.Action OnPhoneCompleted;
    public System.Action OnDeclineCall;

    private Image img;
    private Color originalColor;
    private RectTransform rect;
    private Vector3 originalScale;
    private Vector2 originalPos;
    private Quaternion originalRot;

    private bool clicked;
    private bool phoneOpened;
    private bool isFirstVisit = true;

    private AudioSource audioSource;

    private enum PhoneState { Closed, Call, Pizza, Accept }
    private PhoneState state = PhoneState.Closed;

    // -------------------------
    // EXTERNAL SETUP
    // -------------------------
    public void SetFirstVisit(bool first)
    {
        isFirstVisit = first;
    }

    void Start()
    {
        img = GetComponent<Image>();
        rect = GetComponent<RectTransform>();

        originalColor = img.color;
        originalScale = rect.localScale;
        originalPos = rect.anchoredPosition;
        originalRot = transform.rotation;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Starting image (before click)
        if (isFirstVisit)
        {
            if (firstVisitStartImage != null) img.sprite = firstVisitStartImage;
        }
        else
        {
            if (repeatVisitStartImage != null) img.sprite = repeatVisitStartImage;
        }

        SetBrightness(normalBrightness);

        SetupButton(declineCallButton, OnDeclineCallClicked, false);
        SetupButton(pizzaOrderButton, OnPizzaOrderClicked, false);
        SetupButton(acceptOrderButton, OnAcceptOrderClicked, false);

        SetButtonInvisible(declineCallButton);

        InvokeRepeating(nameof(StartVibration), startDelay, loopTime);
    }

    private void SetupButton(GameObject go, UnityEngine.Events.UnityAction action, bool active)
    {
        if (go == null) return;

        go.SetActive(active);
        Button btn = go.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(action);
        }
    }

    private void SetButtonInvisible(GameObject buttonGO)
    {
        if (buttonGO == null) return;

        Image i = buttonGO.GetComponent<Image>();
        if (i != null)
        {
            Color c = i.color;
            c.a = 0f;
            i.color = c;
        }

        Text t = buttonGO.GetComponentInChildren<Text>();
        if (t != null)
        {
            Color c = t.color;
            c.a = 0f;
            t.color = c;
        }
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
        if (clicked) return;
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
        if (phoneOpened) return;

        clicked = true;
        CancelInvoke(nameof(StartVibration));
        SetBrightness(normalBrightness);

        StartCoroutine(ClickAnimation());
    }

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

        rect.localScale = endScale;
        rect.anchoredPosition = endPos;

        phoneOpened = true;

        if (isFirstVisit)
            SetState(PhoneState.Call);
        else
            SetState(PhoneState.Pizza);
    }

    // -------------------------
    // STATES
    // -------------------------
    private void SetState(PhoneState newState)
    {
        state = newState;

        switch (state)
        {
            case PhoneState.Call:
                if (callScreenImage != null) img.sprite = callScreenImage;
                break;
            case PhoneState.Pizza:
                if (pizzaScreenImage != null) img.sprite = pizzaScreenImage;
                break;
            case PhoneState.Accept:
                if (acceptScreenImage != null) img.sprite = acceptScreenImage;
                break;
        }

        img.color = new Color(
            Mathf.Clamp01(originalColor.r * 2f),
            Mathf.Clamp01(originalColor.g * 2f),
            Mathf.Clamp01(originalColor.b * 2f),
            originalColor.a
        );

        if (screenChangeSound != null)
            audioSource.PlayOneShot(screenChangeSound);

        if (declineCallButton != null) declineCallButton.SetActive(false);
        if (pizzaOrderButton != null) pizzaOrderButton.SetActive(false);
        if (acceptOrderButton != null) acceptOrderButton.SetActive(false);

        if (state == PhoneState.Call && declineCallButton != null)
            declineCallButton.SetActive(true);

        if (state == PhoneState.Pizza && pizzaOrderButton != null)
            pizzaOrderButton.SetActive(true);

        if (state == PhoneState.Accept && acceptOrderButton != null)
            acceptOrderButton.SetActive(true);
    }

    // -------------------------
    // BUTTON ACTIONS
    // -------------------------
    private void OnDeclineCallClicked()
    {
        OnDeclineCall?.Invoke();
        SetState(PhoneState.Pizza);
    }

    private void OnPizzaOrderClicked()
    {
        SetState(PhoneState.Accept);
    }

    private void OnAcceptOrderClicked()
    {
        OnPhoneCompleted?.Invoke();
    }

    // -------------------------
    // AUTO DECLINE SUPPORT
    // -------------------------
    public bool IsOnCallScreen()
    {
        return phoneOpened && state == PhoneState.Call;
    }

    public void AutoDeclineCall()
    {
        if (IsOnCallScreen())
            OnDeclineCallClicked();
    }
}
