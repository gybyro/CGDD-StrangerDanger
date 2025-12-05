using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text nameBox;
    public Typewriter typewriter;

    [Header("Characters")]
    public CharacterDatabase characterDB;

    private DialogueFile dialogue;
    private DialogueLine currentLine;

    private bool waitingForPlayerInput = false;

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
        // ❗ handle random branches BEFORE showing text
        if (line.type == "random")
        {
            currentLine = PickRandomLine(line);
            yield break;
        }

        // ❗ handle player choices BEFORE showing text
        if (line.type == "choice")
        {
            yield return HandleChoice(line);
            yield break;
        }

        // =====================
        //  Character Handling
        // =====================
        Character speaker = characterDB.GetCharacter(line.speaker);
        typewriter.SetSpeaker(speaker);

        if (speaker != null)
        {
            nameBox.text = speaker.displayName;
            speaker.Show();

            if (!string.IsNullOrEmpty(line.emotion))
                speaker.SetExpression(line.emotion);
        }
        else
        {
            nameBox.text = "";
        }

        // =====================
        //   Typewriter Begin
        // =====================
        typewriter.StartTyping(line.text);

        // WAIT until typing done OR skip pressed
        while (!typewriter.lineComplete)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                typewriter.CompleteLineNow();

            yield return null;
        }

        // WAIT for Space to continue
        waitingForPlayerInput = true;
        while (waitingForPlayerInput)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                waitingForPlayerInput = false;

            yield return null;
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
}