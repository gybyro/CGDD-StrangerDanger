using System.Collections;
using UnityEngine;
using TMPro; // if using TextMeshPro

public class Typewriter : MonoBehaviour
{
    [Header("References")]// =============================================================
    public TMP_Text dialogueText; // text from script / text mesh 

    [Header("Settings")]
    public float typeSpeed = 0.03f;
    public bool isTyping { get; private set; }
    public bool lineComplete { get; private set; }
    public AudioSource audioSource;

    private Coroutine typingCoroutine;
    private Character currentSpeaker;

    /// <summary>
    /// Starts typing a new line of dialogue.
    /// </summary>
    public void StartTyping(string text)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(text));
    }

    /// <summary>
    /// Immediately finishes typing the line (skip effect).
    /// </summary>
    public void CompleteLineNow()
    {
        if (!isTyping) return;

        isTyping = false;
        lineComplete = true;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        // instantly write full text
        dialogueText.maxVisibleCharacters = dialogueText.textInfo.characterCount;
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        lineComplete = false;

        dialogueText.text = text;
        dialogueText.ForceMeshUpdate();

        int totalCharacters = dialogueText.textInfo.characterCount;
        dialogueText.maxVisibleCharacters = 0;

        for (int i = 0; i < totalCharacters; i++)
        {
            if (!isTyping) yield break; // typing cancelled by skip

            // SOUND PER LETTER
            // if (char.IsLetter(text[i]))
            // {
            //     audioSource.Play("typeSound");
            // }
            if (currentSpeaker != null && char.IsLetter(text[i]))
                {
                    PlayVoiceSound(currentSpeaker);
                }

            dialogueText.maxVisibleCharacters = i + 1;
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
        lineComplete = true;
    }

    // sound per letter function ================================================
    private void PlayVoiceSound(Character speaker)
    {
        if (speaker.voiceSounds == null || speaker.voiceSounds.Length == 0) return;

        AudioClip clip = speaker.voiceSounds[
            Random.Range(0, speaker.voiceSounds.Length)
        ];

        speaker.audioSource.pitch = Random.Range(
            speaker.voicePitchMin, 
            speaker.voicePitchMax
        );

        speaker.audioSource.PlayOneShot(clip);
    }
}

// setup
// Create a UI Canvas
// Add a TextMeshProUGUI object
// Add Typewriter script to it
// Drag the TMP object into the dialogueText slot
// In your dialogue manager, reference the typewriter Done.


// ================= Example of calling the typewriter:

// public Typewriter typewriter;
// public IEnumerator ShowLine(DialogueLine line)
// {
//     typewriter.StartTyping(line.text);

//     // Wait until line is fully typed
//     while (!typewriter.lineComplete)
//     {
//         // SKIP TEXT if player presses space
//         if (Keyboard.current.spaceKey.wasPressedThisFrame)
//         {
//             typewriter.CompleteLineNow();
//         }

//         yield return null;
//     }

//     // Now wait until player presses space to continue
//     bool pressed = false;
//     while (!pressed)
//     {
//         if (Keyboard.current.spaceKey.wasPressedThisFrame)
//             pressed = true;

//         yield return null;
//     }
// }

