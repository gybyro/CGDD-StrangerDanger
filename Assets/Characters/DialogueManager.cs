using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup textBox;
    public CanvasGroup nameBox;
    public TMP_Text nameBoxText;
    public Typewriter typewriter;

    [Header("Refrences")]
    public PlayerInput playerInput;
    [Header("Extra Systems")]
    public Animator doorAnimator;
    public AudioSource sfxSource;
    // public SceneTransition sceneTransition; // optional fade system


    [Header("Characters")]
    public CharacterDatabase characterDB;
    private Character currentSpeaker;

    private DialogueFile dialogue;
    private DialogueLine currentLine;

    private bool waitingForPlayerInput = false;
    private InputAction advanceAction;
    private Character currentSpeakerInstance;
    private bool advanceRequested;
    private float defaultTypeSpeed;

    private Character GetOrSpawnCharacter(string id)
    {
        // If already spawned, reuse it
        if (currentSpeakerInstance != null &&
            currentSpeakerInstance.characterID == id)
            return currentSpeakerInstance;

        // Get prefab from database
        Character prefab = characterDB.GetCharacter(id);

        if (prefab == null)
        {
            Debug.LogWarning("No character prefab found for: " + id);
            return null;
        }

        // Spawn into scene
        currentSpeakerInstance = Instantiate(prefab);

        return currentSpeakerInstance;
    }



    void Awake()
    {
        advanceAction = playerInput.actions["Next"];
        nameBox.alpha = 0;
        textBox.alpha = 0;
        defaultTypeSpeed = typewriter.typeSpeed;
    }

    public void OnAdvancePressed()
    {
        advanceRequested = true;
    }



    // ==============================
    //   LOAD JSON
    // ==============================
    public void LoadDialogue(string fileName)
    {
        TextAsset json = Resources.Load<TextAsset>(fileName);
        dialogue = JsonUtility.FromJson<DialogueFile>(json.text);
    }


    public void SetSpeaker(Character speaker)
    {
        
        currentSpeaker = speaker;
    }

    // ==============================
    //   RUN THE WHOLE DIALOGUE
    // ==============================
    public IEnumerator RunDialogue(string fileName)
    {
        LoadDialogue(fileName);

        // Start at line 0
        currentLine = dialogue.lines[0];

        while (currentLine != null && !currentLine.finished)
        {
            yield return RunLine(currentLine);
            currentLine = GetNextLine(currentLine);
        }

        Debug.Log("Dialogue finished.");
    }

    // idk somewhere in "game" put this to read the dialouge:
    // StartCoroutine(dialogueManager.RunDialogue("dialogue_day1"));



    // ==============================
    //   RUN A SINGLE LINE
    // ==============================
    private IEnumerator RunLine(DialogueLine line)
    {
        // RANDOM BRANCH ======================
        if (line.type == "random")
        {
            currentLine = PickRandomLine(line);
            yield break;
        }

        // CHOICE BRANCH ======================
        if (line.type == "choice")
        {
            yield return HandleChoice(line);
            yield break;
        }

        // SPEAKER HANDLING ======================
        // Character speaker = characterDB.GetCharacter(line.speaker);
        Character speaker = GetOrSpawnCharacter(line.speaker);

        if (speaker != null)
        {
            currentSpeaker = speaker;
            typewriter.SetSpeaker(speaker);
            nameBoxText.text = speaker.displayName;
        }
        else { 
            nameBox.alpha = 0;
            Debug.Log("Character speaker is NULL");
            }


        // SET PORTRAIT / SPRITE IMAGE
        if (!string.IsNullOrEmpty(line.portrait)) { speaker.SetExpression(line.portrait); }

        // TEXT COLOR ======================
        ApplyTextColor(line.color);

        // DOOR ANIMATION ======================
        TriggerDoorAnimation(line.animationTriggerDoor);

        // after door play show or hides
        if (line.showChar == "true") { speaker.Show(); }
        else if (line.showChar == "false") { speaker.Hide(); }

        // SOUND ======================
        PlayDialogueSound(line.sound);

        // TYPEWRITER SPEED =======================
        if (line.typeSpeed > 0)
            typewriter.typeSpeed = line.typeSpeed;
        else
            typewriter.typeSpeed = defaultTypeSpeed;
            
        // TYPEWRITER / TEXT ======================
        bool hasText = !string.IsNullOrEmpty(line.text);
        if (hasText)
        {
            textBox.alpha = 1;
            nameBox.alpha = 1;
            typewriter.StartTyping(line.text);

            // WAIT FOR TYPEWRITER OR SKIP
            while (!typewriter.lineComplete)
            {
                if (advanceAction.WasPressedThisFrame() || advanceRequested)
                {
                    advanceRequested = false;
                    typewriter.CompleteLineNow();
                }

                yield return null;
            }
        }
        else { textBox.alpha = 0; }
      


        // LINE WAIT IN SECONDS ======================
        if (line.waitSeconds > 0)
            yield return new WaitForSeconds(line.waitSeconds);
        typewriter.typeSpeed = defaultTypeSpeed;

        // WAIT FOR ADVANCE INPUT ======================
        if (hasText)
        {
            waitingForPlayerInput = true;
            while (waitingForPlayerInput)
            {
                if (advanceAction.WasPressedThisFrame() || advanceRequested)
                {
                    advanceRequested = false;
                    waitingForPlayerInput = false;
                }

                yield return null;
            }
        }

        // SCENE CHANGE ======================
        if (!string.IsNullOrEmpty(line.nextScene))
        {
            //sceneTransition.LoadScene(line.nextScene);
            yield break;
        }
    }

    // ==============================
    // RANDOM BRANCH HANDLING
    // ==============================
    private DialogueLine PickRandomLine(DialogueLine line)
    {
        int total = 0;
        foreach (var c in line.choices)
            total += c.weight;

        int roll = Random.Range(0, total);
        int accum = 0;

        foreach (var c in line.choices)
        {
            accum += c.weight;
            if (roll < accum)
                return FindLine(c.next);
        }

        return null;
    }

    // ==============================
    // CHOICE HANDLING (simple)
    // ==============================
    private IEnumerator HandleChoice(DialogueLine line)
    {
        Debug.Log("CHOICE DETECTED — not fully implemented UI here.");

        // TEMP: pick first option automatically
        currentLine = FindLine(line.options[0].next);
        yield return null;
    }

    // ==============================
    // FIND NEXT DIALOGUE LINE
    // ==============================
    private DialogueLine GetNextLine(DialogueLine line)
    {
        if (string.IsNullOrEmpty(line.next))
            return null;

        return FindLine(line.next);
    }

    private DialogueLine FindLine(string id)
    {
        foreach (var line in dialogue.lines)
            if (line.id == id)
                return line;

        Debug.LogWarning("Line not found: " + id);
        return null;
    }

    private void ApplyTextColor(string hex)
    {
        if (string.IsNullOrEmpty(hex))
        {
            typewriter.dialogueText.color = Color.white;
            return;
        }

        if (ColorUtility.TryParseHtmlString(hex, out Color c))
            typewriter.dialogueText.color = c;
        else
            typewriter.dialogueText.color = Color.white;
    }

    private void PlayDialogueSound(string soundName)
    {
        if (string.IsNullOrEmpty(soundName)) return;

        AudioClip clip = Resources.Load<AudioClip>("SFX/" + soundName);
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    private void TriggerDoorAnimation(string trigger)
    {
        if (string.IsNullOrEmpty(trigger) || doorAnimator == null) return;
        doorAnimator.SetTrigger(trigger);
    }
}