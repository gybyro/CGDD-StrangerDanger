using UnityEngine;

public class StoryLine : MonoBehaviour
{
    [Header("References")]
    public DialogueManager dialogueManager;

    private string currentDialogue;

    void Awake()
    {
        // Pull dialogue key from GameManager
        if (GameManager.Instance != null)
            currentDialogue = GameManager.Instance.nextDialogue;
        else
            currentDialogue = "start"; // fallback
    }

    void Start()
    {
        if (string.IsNullOrEmpty(currentDialogue))
            currentDialogue = "start";

        
        // StartCoroutine(dialogueManager.RunDialogue("dial_test"));
        StartCoroutine(dialogueManager.RunDialogue("dial_test2"));
    }
}