using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

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
    public Sprite firstVisitStartImage;
    public Sprite repeatVisitStartImage;

    [Header("Flow Images")]
    public Sprite callScreenImage;
    public Sprite pizzaScreenImage;
    public Sprite acceptScreenImage;

    [Header("Sound")]
    public AudioClip screenChangeSound;

    [Header("Buttons")]
    public GameObject declineCallButton;   // invisible but clickable
    public GameObject pizzaOrderButton;
    public GameObject acceptOrderButton;

    [Header("Pizza Order UI")]
    public GameObject orderPanel;
    public TMP_Text customerNameText;
    public TMP_Text customerAddressText;
    public TMP_Text pizzaOrderText;

    public System.Action OnPhoneCompleted;
    public System.Action OnDeclineCall;
    public System.Action OnPhoneOpened;

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
    // JSON ORDERS
    // -------------------------
    [System.Serializable]
    private class PizzaOrder
    {
        public int id;
        public string customerName;
        public string customerAddress;
        public string pizzaOrder;
    }

    [System.Serializable]
    private class PizzaOrderList
    {
        public PizzaOrder[] orders;
    }

    private PizzaOrderList loadedOrders;
    private PizzaOrder currentOrder;

    private const string OrderIndexKey = "PizzaOrderIndex";
    private bool orderAssignedThisScene = false;

    void Awake()
    {
        img = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }

    public void SetFirstVisit(bool first)
    {
        isFirstVisit = first;

        if (img != null && !phoneOpened)
        {
            img.sprite = isFirstVisit ? firstVisitStartImage : repeatVisitStartImage;
        }
    }

    // ✅ NEW: call this when starting the story (so customer 1 is guaranteed)
    public void ResetOrderSequence()
    {
        PlayerPrefs.SetInt(OrderIndexKey, 0);
        PlayerPrefs.Save();
    }

    // Called by CarSceneTriggers ONCE per CarScene entry
    public void AssignOrderForCarSceneEntry(bool advanceToNext)
    {
        if (orderAssignedThisScene) return;
        orderAssignedThisScene = true;

        TextAsset jsonAsset = Resources.Load<TextAsset>("PizzaOrders");
        if (jsonAsset == null)
        {
            Debug.LogWarning("PizzaOrders.json not found! Put it in Assets/Resources/PizzaOrders.json");
            return;
        }

        loadedOrders = JsonUtility.FromJson<PizzaOrderList>(jsonAsset.text);
        if (loadedOrders == null || loadedOrders.orders == null || loadedOrders.orders.Length == 0)
        {
            Debug.LogWarning("PizzaOrders.json has no orders.");
            return;
        }

        int count = loadedOrders.orders.Length;

        // read current index (default 0 if missing)
        int index = PlayerPrefs.GetInt(OrderIndexKey, 0);
        index = ((index % count) + count) % count;

        // advance only on repeat carscene entries
        if (advanceToNext)
        {
            index = (index + 1) % count;
            PlayerPrefs.SetInt(OrderIndexKey, index);
            PlayerPrefs.Save();
        }
        else
        {
            // keep whatever index is stored (but carscene trigger will reset it on true first entry)
            PlayerPrefs.SetInt(OrderIndexKey, index);
            PlayerPrefs.Save();
        }

        currentOrder = loadedOrders.orders[index];
    }

    void Start()
    {
        originalColor = img.color;
        originalScale = rect.localScale;
        originalPos = rect.anchoredPosition;
        originalRot = transform.rotation;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        if (!phoneOpened)
            img.sprite = isFirstVisit ? firstVisitStartImage : repeatVisitStartImage;

        SetBrightness(normalBrightness);

        SetupButton(declineCallButton, OnDeclineCallClicked, false);
        SetupButton(pizzaOrderButton, OnPizzaOrderClicked, false);
        SetupButton(acceptOrderButton, OnAcceptOrderClicked, false);

        SetButtonInvisible(declineCallButton);

        if (orderPanel != null)
            orderPanel.SetActive(false);

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

    private void ApplyOrderToUI()
    {
        if (currentOrder == null) return;

        if (customerNameText != null) customerNameText.text = currentOrder.customerName;
        if (customerAddressText != null) customerAddressText.text = currentOrder.customerAddress;
        if (pizzaOrderText != null) pizzaOrderText.text = currentOrder.pizzaOrder;
    }

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
        OnPhoneOpened?.Invoke();

        if (isFirstVisit) SetState(PhoneState.Call);
        else SetState(PhoneState.Pizza);
    }

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

        if (state == PhoneState.Call && declineCallButton != null) declineCallButton.SetActive(true);
        if (state == PhoneState.Pizza && pizzaOrderButton != null) pizzaOrderButton.SetActive(true);
        if (state == PhoneState.Accept && acceptOrderButton != null) acceptOrderButton.SetActive(true);
    }

    private void OnDeclineCallClicked()
    {
        OnDeclineCall?.Invoke();
        SetState(PhoneState.Pizza);
    }

    private void OnPizzaOrderClicked()
    {
        ApplyOrderToUI();
        if (orderPanel != null) orderPanel.SetActive(true);

        SetState(PhoneState.Accept);
    }

    private void OnAcceptOrderClicked()
    {
        OnPhoneCompleted?.Invoke();
    }

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
