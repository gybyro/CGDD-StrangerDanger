using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup textBox;
    public CanvasGroup nameBox;
    public TMP_Text nameBoxText;
    public Typewriter typewriter;

    [Header("References")]
    public PlayerInput playerInput;
    public ChoiceUI choiceUI;
    public Animator doorAnimator;
    public AudioSource sfxSource;
    public DoorBGPallet houseBG;

    [Header("Characters")]
    public CharacterDatabase characterDB;
    private Character currentSpeaker;
    private Character activeNPC;

    private DialogueFile dialogue;
    private DialogueLine currentLine;

    private bool waitingForPlayerInput = false;
    private InputAction advanceAction;
    private Dictionary<string, Character> spawnedCharacters = new();
    private bool advanceRequested;
    private float defaultTypeSpeed;
    private bool isChoosing;
    private bool lineWasHandledByChoice;

    // ==============================
    // INIT
    // ==============================
    void Awake()
    {
        advanceAction = playerInput.actions["Next"];

        nameBox.alpha = 0;
        nameBox.interactable = false;
        textBox.alpha = 0;
        textBox.interactable = false;

        defaultTypeSpeed = typewriter.typeSpeed;
    }

    public void OnAdvancePressed()
    {
        advanceRequested = true;
    }

    // ==============================
    // LOAD JSON
    // ==============================
    public void LoadDialogue(string fileName)
    {
        TextAsset json = Resources.Load<TextAsset>(fileName);
        dialogue = JsonUtility.FromJson<DialogueFile>(json.text);
    }

    // ==============================
    // RUN DIALOGUE
    // ==============================
    public IEnumerator RunDialogue(string fileName)
    {
        LoadDialogue(fileName);
        currentLine = dialogue.lines[0];

        if (houseBG != null)
            houseBG.ApplyColorSet(dialogue.houseBgPallet);

        while (currentLine != null)
        {
            lineWasHandledByChoice = false;

            yield return RunLine(currentLine);

            if (currentLine.finished)
                EndDialogue(currentLine);

            if (!lineWasHandledByChoice)
                currentLine = GetNextLine(currentLine);
        }

        foreach (var c in spawnedCharacters.Values)
            Destroy(c.gameObject);

        spawnedCharacters.Clear();
    }

    // ==============================
    // RUN SINGLE LINE
    // ==============================
    private IEnumerator RunLine(DialogueLine line)
    {
        advanceRequested = false;

        bool isPlayerSpeaking = line.speaker.ToLower() == "you";
        Character speaker = GetOrSpawnCharacter(line.speaker);

        if (!isPlayerSpeaking && speaker != null)
        {
            currentSpeaker = speaker;
            activeNPC = speaker;
            typewriter.SetSpeaker(speaker);
            nameBoxText.text = speaker.displayName;
        }
        else
        {
            currentSpeaker = speaker;
            typewriter.SetSpeaker(speaker);
            nameBoxText.text = speaker != null ? speaker.displayName : "";
        }

        if (!string.IsNullOrEmpty(line.portrait) && speaker != null)
            speaker.SetExpression(line.portrait);

        ApplyTextColor(line.color);
        TriggerDoorAnimation(line.animationTriggerDoor);

        if (activeNPC != null)
        {
            if (line.showChar == "true") activeNPC.Show();
            else if (line.showChar == "false") activeNPC.Hide();
        }

        PlayDialogueSound(line.sound);

        if (line.sanity != 0 && Sanity_Meter.Instance != null)
            Sanity_Meter.Instance.Lower_Sanity(line.sanity);

        // ==============================
        // BRANCHING
        // ==============================
        if (line.type == "random")
        {
            currentLine = PickRandomLine(line);
            yield break;
        }

        if (line.type == "choice")
        {
            yield return HandleChoice(line);
            yield break;
        }

        if (line.type == "countdown")
        {
            yield return HandleCountdown(line);
            yield break;
        }

        // ==============================
        // TEXT
        // ==============================
        bool hasText = !string.IsNullOrEmpty(line.text);

        if (hasText)
        {
            textBox.alpha = 1;
            nameBox.alpha = 1;
            textBox.interactable = true;
            nameBox.interactable = true;

            typewriter.typeSpeed = line.typeSpeed > 0 ? line.typeSpeed : defaultTypeSpeed;
            typewriter.StartTyping(line.text);

            while (!typewriter.lineComplete)
            {
                if (!isChoosing && (advanceAction.WasPressedThisFrame() || advanceRequested))
                {
                    advanceRequested = false;
                    typewriter.CompleteLineNow();
                }
                yield return null;
            }

            if (line.tips != 0 && Money_Manager.Instance != null)
                Money_Manager.Instance.AddMoney(line.tips);
        }
        else
        {
            textBox.alpha = 0;
        }

        if (line.waitSeconds > 0)
            yield return new WaitForSeconds(line.waitSeconds);

        typewriter.typeSpeed = defaultTypeSpeed;

        if (hasText)
        {
            waitingForPlayerInput = true;
            while (waitingForPlayerInput)
            {
                if (!isChoosing && (advanceAction.WasPressedThisFrame() || advanceRequested))
                {
                    advanceRequested = false;
                    waitingForPlayerInput = false;
                }
                yield return null;
            }
        }
    }

    // ==============================
    // COUNTDOWN (NO HideChoices)
    // ==============================
    IEnumerator HandleCountdown(DialogueLine line)
    {
        textBox.alpha = 1;

        bool hasEscapeOption =
            line.options != null &&
            line.options.Length > 0 &&
            !string.IsNullOrEmpty(line.options[0].next);

        bool escapeChosen = false;
        int selectedIndex = 0;

        isChoosing = true;
        lineWasHandledByChoice = true;

        if (hasEscapeOption)
        {
            choiceUI.ShowChoices(line, (index) =>
            {
                selectedIndex = index;
                escapeChosen = true;
                isChoosing = false;
            });
        }

        float remaining = Mathf.Max(0f, line.countDownSeconds);
        string basePrompt = !string.IsNullOrEmpty(line.prompt) ? line.prompt : line.text;
        int lastShownSeconds = -1;

        while (remaining > 0f && !escapeChosen)
        {
            int shownSeconds = Mathf.CeilToInt(remaining);
            if (shownSeconds != lastShownSeconds)
            {
                lastShownSeconds = shownSeconds;
                typewriter.dialogueText.text = $"{basePrompt} ({shownSeconds})";
            }

            remaining -= Time.deltaTime;
            yield return null;
        }

        isChoosing = false;

        if (escapeChosen && hasEscapeOption)
        {
            currentLine = FindLine(line.options[selectedIndex].next);
            yield break;
        }

        currentLine = FindLine(
            !string.IsNullOrEmpty(line.timeOutNext) ? line.timeOutNext : line.next
        );
    }

    // ==============================
    // CHOICE
    // ==============================
    private IEnumerator HandleChoice(DialogueLine line)
    {
        isChoosing = true;
        lineWasHandledByChoice = true;

        bool choiceMade = false;
        int selectedIndex = 0;

        choiceUI.ShowChoices(line, (index) =>
        {
            selectedIndex = index;
            choiceMade = true;
            isChoosing = false;
        });

        while (!choiceMade)
            yield return null;

        currentLine = FindLine(line.options[selectedIndex].next);
    }

    // ==============================
    // HELPERS
    // ==============================
    private DialogueLine GetNextLine(DialogueLine line)
    {
        if (string.IsNullOrEmpty(line.next)) return null;
        return FindLine(line.next);
    }

    private DialogueLine FindLine(string id)
    {
        foreach (var l in dialogue.lines)
            if (l.id == id)
                return l;

        Debug.LogWarning("Line not found: " + id);
        return null;
    }

    private DialogueLine PickRandomLine(DialogueLine line)
    {
        int total = 0;
        foreach (var c in line.choices) total += c.weight;

        int roll = Random.Range(0, total);
        int acc = 0;

        foreach (var c in line.choices)
        {
            acc += c.weight;
            if (roll < acc) return FindLine(c.next);
        }
        return null;
    }

    private void ApplyTextColor(string hex)
    {
        if (!string.IsNullOrEmpty(hex) &&
            ColorUtility.TryParseHtmlString(hex, out Color c))
        {
            typewriter.dialogueText.color = c;
        }
        else if (currentSpeaker != null)
        {
            typewriter.dialogueText.color = currentSpeaker.defaultTextColor;
        }
        else
        {
            typewriter.dialogueText.color = Color.white;
        }
    }

    private void PlayDialogueSound(string soundName)
    {
        if (string.IsNullOrEmpty(soundName)) return;

        AudioClip clip = Resources.Load<AudioClip>("Sounds/" + soundName);
        if (clip != null) sfxSource.PlayOneShot(clip);
    }

    private void TriggerDoorAnimation(string trigger)
    {
        if (!string.IsNullOrEmpty(trigger) && doorAnimator != null)
            doorAnimator.SetTrigger(trigger);
    }

    private Character GetOrSpawnCharacter(string id)
    {
        if (spawnedCharacters.TryGetValue(id, out var existing))
            return existing;

        Character prefab = characterDB.GetCharacter(id);
        if (prefab == null) return null;

        Character newChar = Instantiate(prefab);
        spawnedCharacters.Add(id, newChar);
        return newChar;
    }

    private void EndDialogue(DialogueLine line)
    {
        if (activeNPC == null) return;
        if (GameManager.Instance == null) return;

        GameManager.Instance.AdvanceCharDial(activeNPC.ID, line.nextDialFile);
    }
}