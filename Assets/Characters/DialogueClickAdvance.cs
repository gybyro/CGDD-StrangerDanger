using UnityEngine;
using UnityEngine.UI;

public class DialogueClickAdvance : MonoBehaviour
{
    public DialogueManager dialogueManager;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            dialogueManager.OnAdvancePressed();
        });
    }
}