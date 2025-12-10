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

    [Header("Refrences")]
    public PlayerInput playerInput;
    public ChoiceUI choiceUI;
    public Animator doorAnimator;
    public AudioSource sfxSource;
    // public SceneTransition sceneTransition; // optional fade system


    [Header("Characters")]
    public CharacterDatabase characterDB;
    private Character currentSpeaker;
    private Character activeNPC;

    private DialogueFile dialogue;
    private DialogueLine currentLine;

    private bool waitingForPlayerInput = false;
    private InputAction advanceAction;
    private Dictionary<string, Character> spawnedCharacters = new Dictionary<string, Character>();
    private bool advanceRequested;
    private float defaultTypeSpeed;
    private bool isChoosing;
    private bool lineWasHandledByChoice;

    private Character GetOrSpawnCharacter(string id)
    {
        // If character already exists, reuse it
        if (spawnedCharacters.TryGetValue(id, out Character existing))
            return existing;

        // Get prefab from database
        Character prefab = characterDB.GetCharacter(id);


        if (prefab == null)
        {
            Debug.LogWarning("No character prefab found for: " + id);
            return null;
        }

        // Spawn only ONCE
        Character newChar = Instantiate(prefab);
        spawnedCharacters.Add(id, newChar);

        return newChar;
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

        while (currentLine != null)
        {
            lineWasHandledByChoice = false;

            yield return RunLine(currentLine);

            // Stop after "finished": true
            if (currentLine.finished)
                EndDialogue(currentLine);

            // ONLY advance automatically if NOT a choice
            if (!lineWasHandledByChoice)
                currentLine = GetNextLine(currentLine);
        }

        // while (currentLine != null && !currentLine.finished)
        // {
        //     yield return RunLine(currentLine);
        //     currentLine = GetNextLine(currentLine);
        // }

        Debug.Log("Dialogue finished.");
        foreach (var c in spawnedCharacters.Values)
            { Destroy(c.gameObject); }
        spawnedCharacters.Clear();
    }

    // idk somewhere in "game" put this to read the dialouge:
    // StartCoroutine(dialogueManager.RunDialogue("dialogue_day1"));



    // ==============================
    //   RUN A SINGLE LINE
    // ==============================
    private IEnumerator RunLine(DialogueLine line)
    {
        advanceRequested = false;

        // SPEAKER HANDLING ======================
        bool isPlayerSpeaking = line.speaker.ToLower() == "you";
        Character speaker = GetOrSpawnCharacter(line.speaker);

        if (!isPlayerSpeaking)
        {
            // NPC is speaking -> they become the active conversation target
            currentSpeaker = speaker;
            activeNPC = speaker;

            typewriter.SetSpeaker(speaker);
            nameBoxText.text = speaker.displayName;
        }
        else if (isPlayerSpeaking)
        {
            // Player is speaking -> keep portrait focus on last NPC
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

        if (!string.IsNullOrEmpty(line.portrait))
        {
            if (isPlayerSpeaking)
                if (activeNPC != null) 
                    activeNPC.SetExpression(line.portrait);
            
            else speaker.SetExpression(line.portrait);
        }

        // TEXT COLOR ======================
        ApplyTextColor(line.color);

        // DOOR ANIMATION ======================
        TriggerDoorAnimation(line.animationTriggerDoor);

        // after door play show or hides
        if (line.showChar == "true") { activeNPC.Show(); }
        else if (line.showChar == "false") { activeNPC.Hide(); }

        // SOUND ======================
        PlayDialogueSound(line.sound);


        // BRANCHES
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
                if (!isChoosing && (advanceAction.WasPressedThisFrame() || advanceRequested))
                {
                    advanceRequested = false;
                    typewriter.CompleteLineNow();
                }

                yield return null;
            }
            if (line.tips != 0) Money_Manager.Instance.AddMoney(line.tips);
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


    // CHOICE HANDLING ==============================
    public void ClearAdvanceInput() { advanceRequested = false; }


    // CHOICE HANDLING ==============================
    private IEnumerator HandleChoice(DialogueLine line)
    {
        bool hasText = !string.IsNullOrEmpty(line.text);

        if (hasText)
        {
            textBox.alpha = 1;
            typewriter.StartTyping(line.text);

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

        bool choiceMade = false;
        int selectedIndex = 0;

        isChoosing = true;
        lineWasHandledByChoice = true;

        choiceUI.ShowChoices(line, (index) =>
        {
            selectedIndex = index;
            choiceMade = true;
            isChoosing = false;
        });

        while (!choiceMade)
            yield return null;

        currentLine = FindLine(line.options[selectedIndex].next);
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
        // JSON override always wins
        if (!string.IsNullOrEmpty(hex))
        {
            if (ColorUtility.TryParseHtmlString(hex, out Color c))
            {
                typewriter.dialogueText.color = c;
                return;
            }
        }

        // Use character default color
        if (currentSpeaker != null)
        {
            typewriter.dialogueText.color = currentSpeaker.defaultTextColor;
            return;
        }

        // Fallback
        typewriter.dialogueText.color = Color.white;
    }

    private void PlayDialogueSound(string soundName)
    {
        if (string.IsNullOrEmpty(soundName)) return;

        AudioClip clip = Resources.Load<AudioClip>("Sounds/" + soundName);
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    private void TriggerDoorAnimation(string trigger)
    {
        if (string.IsNullOrEmpty(trigger) || doorAnimator == null) return;
        doorAnimator.SetTrigger(trigger);
    }

    private void EndDialogue(DialogueLine line)
    {
        int charID = activeNPC.ID;
        string nextDialogue = line.nextDialFile;
        

        // GameManager updates the next file to play for that character
        GameManager.Instance.AdvanceCharDial(charID, nextDialogue);
        
        // goes back to StoryLine class after this
    }
}