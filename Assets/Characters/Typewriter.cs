using System.Collections;
using UnityEngine;
using TMPro;

public class Typewriter : MonoBehaviour
{
    [Header("References")]
    public TMP_Text dialogueText;
    public GameObject continueArrow;

    [Header("Settings")]
    public float typeSpeed = 0.03f;

    public bool isTyping { get; private set; }
    public bool lineComplete { get; private set; }

    private Coroutine typingCoroutine;
    private Character currentSpeaker;

    public void SetSpeaker(Character speaker)
    {
        currentSpeaker = speaker;
    }

    public void StartTyping(string text)
    {
        if (continueArrow != null)
            continueArrow.SetActive(false);

        if (dialogueText == null)
        {
            Debug.LogError("Typewriter: dialogueText is NOT assigned!");
            return;
        }

        Debug.Log("Typewriter starting line: " + text);

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(text));
    }

    public void CompleteLineNow()
    {
        if (!isTyping || dialogueText == null) return;

        isTyping = false;
        lineComplete = true;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialogueText.maxVisibleCharacters = dialogueText.textInfo.characterCount;

        if (continueArrow != null)
            continueArrow.SetActive(true);
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        lineComplete = false;

        dialogueText.text = text;
        dialogueText.maxVisibleCharacters = 0;

        yield return null;
        dialogueText.ForceMeshUpdate();

        int totalCharacters = dialogueText.textInfo.characterCount;

        for (int i = 0; i < totalCharacters; i++)
        {
            if (!isTyping) yield break;

            // Voice sound
            if (currentSpeaker != null &&
                currentSpeaker.voiceSounds != null &&
                currentSpeaker.voiceSounds.Length > 0 &&
                char.IsLetter(text[i]) &&
                currentSpeaker.audioSource != null)
            {
                PlayVoiceSound(currentSpeaker);
            }

            dialogueText.maxVisibleCharacters = i + 1;
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
        lineComplete = true;

        if (continueArrow != null)
            continueArrow.SetActive(true);
    }

    private void PlayVoiceSound(Character speaker)
    {
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