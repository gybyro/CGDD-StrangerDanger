using UnityEngine;

public class DoorEventController : MonoBehaviour
{
    [Header("Door Animations")]
    public Animator doorAnimator;
    public string[] openAnimationNames;
    public string closeAnimationName = "DoorClose";

    [Header("Characters")]
    public string[] characterIDs; // "john", "mary", "leo"
    public PortraitController portraitController;

    [Header("Dialogue")]
    public DialogueManager dialogueManager; // your VN dialogue system
    public string dialogueFileName = "dialogue_day1";

    public void StartDoorEvent()
    {
        StartCoroutine(EventSequence());
    }

    private IEnumerator EventSequence()
    {
        // 1. Knock SFX
        AudioManager.Play("knock");

        yield return new WaitForSeconds(0.5f);

        // 2. Pick who answers
        string character = characterIDs[Random.Range(0, characterIDs.Length)];

        // 3. Pick door animation
        string openAnim = openAnimationNames[Random.Range(0, openAnimationNames.Length)];

        doorAnimator.Play(openAnim);

        // Wait for door to finish
        yield return new WaitForSeconds(1.0f);

        // 4. Portrait slide-in animation
        portraitController.ShowCharacter(character);

        // 5. Start the dialogue
        yield return dialogueManager.RunDialogue(dialogueFileName, character);

        // 6. Slide portrait out
        portraitController.HideCharacter();

        yield return new WaitForSeconds(0.5f);

        // 7. Close the door
        doorAnimator.Play(closeAnimationName);
    }
}