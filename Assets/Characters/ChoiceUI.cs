using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;

public class ChoiceUI : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup panel;
    public Button[] optionButtons;
    public TMP_Text[] optionLabels;

    [Header("Input")]
    public PlayerInput playerInput;

    private InputAction nextAction;
    private InputAction upAction;
    private InputAction downAction;

    private Action<int> onChoiceSelected;
    private int currentIndex = 0;
    private bool isActive = false;
    private int activeOptionCount = 0;

    void Awake()
    {
        nextAction = playerInput.actions["Next"];
        upAction = playerInput.actions["Up"];
        downAction = playerInput.actions["Down"];

        HideChoices(); // start hidden safely
    }

    void Update()
    {
        if (!isActive) return;

        if (upAction != null && upAction.WasPressedThisFrame())
            MoveSelection(-1);

        if (downAction != null && downAction.WasPressedThisFrame())
            MoveSelection(1);

        if (nextAction != null && nextAction.WasPressedThisFrame())
            ConfirmSelection();
    }

    public void ShowChoices(DialogueLine line, Action<int> callback)
    {
        // Safety: if line/options is null, just hide and do nothing
        if (line == null || line.options == null || line.options.Length == 0)
        {
            HideChoices();
            return;
        }

        onChoiceSelected = callback;
        isActive = true;

        // Show panel (DO NOT disable this GameObject)
        panel.alpha = 1f;
        panel.interactable = true;
        panel.blocksRaycasts = true;

        currentIndex = 0;
        activeOptionCount = Mathf.Min(line.options.Length, optionButtons.Length);

        // Setup buttons
        for (int i = 0; i < optionButtons.Length; i++)
        {
            bool shouldBeActive = (i < activeOptionCount);

            optionButtons[i].gameObject.SetActive(shouldBeActive);

            if (shouldBeActive)
            {
                optionLabels[i].text = line.options[i].text;

                int index = i;

                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => SelectOption(index));

                AddHoverSync(optionButtons[i], index);
            }
        }

        HighlightCurrent();
    }

    public void HideChoices()
    {
        // Hide the panel ONLY — keep the script alive for later ShowChoices calls.
        isActive = false;

        if (panel != null)
        {
            panel.alpha = 0f;
            panel.interactable = false;
            panel.blocksRaycasts = false;
        }

        // Clear selection so Unity doesn't keep a stale selected button
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
    }

    private void AddHoverSync(Button button, int index)
    {
        if (button == null) return;

        EventTrigger trigger = button.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = button.gameObject.AddComponent<EventTrigger>();

        trigger.triggers.Clear();

        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };

        entry.callback.AddListener((_) =>
        {
            // Only allow hover selection when choices are active
            if (!isActive) return;

            currentIndex = index;
            HighlightCurrent();
        });

        trigger.triggers.Add(entry);
    }

    private void MoveSelection(int direction)
    {
        if (activeOptionCount <= 0) return;

        currentIndex += direction;

        if (currentIndex < 0)
            currentIndex = activeOptionCount - 1;

        if (currentIndex >= activeOptionCount)
            currentIndex = 0;

        HighlightCurrent();
    }

    private void HighlightCurrent()
    {
        for (int i = 0; i < optionButtons.Length; i++)
        {
            // Skip inactive buttons so we don't highlight hidden ones
            if (!optionButtons[i].gameObject.activeInHierarchy)
                continue;

            ColorBlock colors = optionButtons[i].colors;

            if (i == currentIndex)
            {
                colors.normalColor = new Color(1f, 1f, 1f, 0.35f);
                colors.highlightedColor = colors.normalColor;
                colors.selectedColor = colors.normalColor;

                if (EventSystem.current != null)
                    EventSystem.current.SetSelectedGameObject(optionButtons[i].gameObject);
            }
            else
            {
                colors.normalColor = Color.white;
                colors.highlightedColor = Color.white;
                colors.selectedColor = Color.white;
            }

            optionButtons[i].colors = colors;
        }
    }

    private void ConfirmSelection()
    {
        SelectOption(currentIndex);
    }

    private void SelectOption(int index)
    {
        // Prevent selecting out of range
        if (index < 0 || index >= activeOptionCount)
        {
            HideChoices();
            return;
        }

        // Hide UI immediately and stop input
        isActive = false;

        if (panel != null)
        {
            panel.alpha = 0f;
            panel.interactable = false;
            panel.blocksRaycasts = false;
        }

        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);

        onChoiceSelected?.Invoke(index);
    }
}