using System.Collections;
using UnityEngine;
using TMPro;

public class SingleTypewriter : MonoBehaviour
{
    [Header("References")]
    public TMP_Text dialogueText;

    [Header("Defaults")]
    public float defaultTypeSpeed = 0.03f;
    public AudioSource audioSource;
    public AudioClip typingSound;

    public bool isTyping { get; private set; }
    public bool lineComplete { get; private set; }

    private Coroutine typingCoroutine;

    // =========================
    // PUBLIC API
    // =========================

    public void StartTyping(
        string text,
        float? typeSpeedOverride = null,
        AudioClip soundOverride = null
    )
    {
        if (dialogueText == null)
        {
            Debug.LogError("Typewriter: dialogueText is not assigned!");
            return;
        }


        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        float speed = typeSpeedOverride ?? defaultTypeSpeed;
        AudioClip sound = soundOverride ?? typingSound;

        typingCoroutine = StartCoroutine(TypeText(text, speed, sound));
    }

    public void CompleteLineNow()
    {
        if (!isTyping || dialogueText == null)
            return;

        isTyping = false;
        lineComplete = true;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialogueText.maxVisibleCharacters = dialogueText.textInfo.characterCount;

        if (continueArrow != null)
            continueArrow.SetActive(true);
    }

    // =========================
    // INTERNALS
    // =========================

    private IEnumerator TypeText(string text, float speed, AudioClip sound)
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
            if (!isTyping)
                yield break;

            char c = text[i];

            // Play typing sound only for visible characters
            if (sound != null && audioSource != null && char.IsLetterOrDigit(c))
            {
                audioSource.PlayOneShot(sound);
            }

            dialogueText.maxVisibleCharacters = i + 1;
            yield return new WaitForSeconds(speed);
        }

        isTyping = false;
        lineComplete = true;

        if (continueArrow != null)
            continueArrow.SetActive(true);
    }
}