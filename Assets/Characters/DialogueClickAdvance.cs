using UnityEngine;
using UnityEngine.UI;

public class DialogueClickAdvance : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public PlayerCharDialogueInCar playerDialogueManager;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (!dialogueManager) playerDialogueManager.OnAdvancePressed();
            else dialogueManager.OnAdvancePressed();
        });
    }
}