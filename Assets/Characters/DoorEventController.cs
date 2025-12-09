using System.Collections;
using UnityEngine;
using TMPro;

public class DoorEventController : MonoBehaviour
{
    [Header("Door Animations")]
    public Animator doorAnimator;
    public string[] openAnimationNames;
    public string closeAnimationName = "DoorClose";

    [Header("Characters")]
    public string[] characterIDs;
    public CharacterSpriteController characterSpriteController;

    [Header("Dialogue")]
    public DialogueManager dialogueManager; 
    public string dialogueFileName = "dialogue_day1";

    public void StartDoorEvent()
    {
        StartCoroutine(EventSequence());
    }

    private IEnumerator EventSequence()
    {
        // 1. Knock SFX
        //AudioManager.Play("knock");

        yield return new WaitForSeconds(0.5f);

        // 2. Pick who answers
        string character = characterIDs[Random.Range(0, characterIDs.Length)];

        // 3. Pick door animation
        string openAnim = openAnimationNames[Random.Range(0, openAnimationNames.Length)];

        doorAnimator.Play(openAnim);

        // Wait for door to finish
        yield return new WaitForSeconds(1.0f);

        // 4. Portrait slide-in animation
        characterSpriteController.ShowCharacter(character);

        // 5. Start the dialogue
        // yield return dialogueManager.RunDialogue(dialogueFileName, character);

        // 6. Slide portrait out
        characterSpriteController.HideCharacter();

        yield return new WaitForSeconds(0.5f);

        // 7. Close the door
        doorAnimator.Play(closeAnimationName);
    }
}