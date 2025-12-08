using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
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

        panel.alpha = 0;
        panel.interactable = false;
        panel.blocksRaycasts = false;
    }

    void Update()
    {
        if (!isActive) return;

        if (upAction.WasPressedThisFrame())
            MoveSelection(-1);

        if (downAction.WasPressedThisFrame())
            MoveSelection(1);

        if (nextAction.WasPressedThisFrame())
            ConfirmSelection();
    }

    public void ShowChoices(DialogueLine line, Action<int> callback)
    {
        onChoiceSelected = callback;
        isActive = true;

        panel.alpha = 1;
        panel.interactable = true;
        panel.blocksRaycasts = true;

        currentIndex = 0;
        activeOptionCount = line.options.Length;

        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < line.options.Length)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionLabels[i].text = line.options[i].text;

                int index = i;
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() =>
                {
                    SelectOption(index);
                });
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }

        HighlightCurrent();
    }

    private void MoveSelection(int direction)
    {
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
            ColorBlock colors = optionButtons[i].colors;

            if (i == currentIndex)
            {
                colors.normalColor = new Color(1f, 1f, 1f, 0.35f);
                colors.highlightedColor = colors.normalColor;
                colors.selectedColor = colors.normalColor;
            }
            else
            {
                colors.normalColor = Color.white;
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
        isActive = false;

        panel.alpha = 0;
        panel.interactable = false;
        panel.blocksRaycasts = false;

        
        isActive = false;

        panel.alpha = 0;
        panel.interactable = false;
        panel.blocksRaycasts = false;

        // ✅ CLEAR DIALOGUE INPUT SO IT DOESN'T SKIP NEXT LINE
        FindAnyObjectByType<DialogueManager>().ClearAdvanceInput();

        onChoiceSelected?.Invoke(index);
    }
}