
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueSystem : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public UnityEngine.UI.Image portrait;

    //private PlayerInput playerInput;
    private DialogueSequence current;
    private int index;

    // void Awake()
    // {
    //     playerInput = new PlayerInput();
    // }

    // void OnEnable()
    // {
    //     playerInput.UI.Enable();
    //     playerInput.UI.Advance.performed += NextLine;
    // }

    // void OnDisable()
    // {
    //     input.UI.Advance.performed -= NextLine;
    //     input.UI.Disable();
    // }

    public void StartSequence(DialogueSequence sequence)
    {
        current = sequence;
        index = 0;
        DisplayLine();
    }

    private void NextLine(InputAction.CallbackContext ctx)
    {
        if (current == null) return;

        index++;

        if (index >= current.lines.Length)
        {
            EndSequence();
        }
        else
        {
            DisplayLine();
        }
    }

    private void DisplayLine()
    {
        var line = current.lines[index];

        nameText.text = line.characterName;
        dialogueText.text = line.text;
        if (portrait != null)
            portrait.sprite = line.portrait;
    }

    private void EndSequence()
    {
        current = null;
        dialogueText.text = "";
        nameText.text = "";
        if (portrait != null)
            portrait.sprite = null;
    }
}