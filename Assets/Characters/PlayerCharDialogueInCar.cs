using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;



public class PlayerCharDialogueInCar : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup textBox;
    public CanvasGroup nameBox;
    public TMP_Text nameBoxText;
    public Typewriter typewriter;

    [Header("Refrences")]
    public PlayerInput playerInput;
    public AudioSource sfxSource;


    private InputAction advanceAction;
    private bool advanceRequested;
    private bool waitingForPlayerInput = false;
    private float defaultTypeSpeed;

    private int currentDay;
    private int currentTime;

    private PlayerDialogueLine[] dialogue;
    private int currentLineIndex = 0;
    private bool returning;

    [Header("Dialogue Days")]
    public DayInWeek day1;
    public DayInWeek day2;
    public DayInWeek day3;
    public DayInWeek day4;
    public DayInWeek day5;
    public DayInWeek day6;
    public DayInWeek day7;

    void Awake()
    {
        advanceAction = playerInput.actions["Next"];
        nameBox.alpha = 0;
        textBox.alpha = 0;
        defaultTypeSpeed = typewriter.typeSpeed;
        returning  = false;

    }
    public void OnAdvancePressed()
    {
        advanceRequested = true;
    }


    private PlayerDialogueLine[] GetDialogue()
    {
        currentDay = GameManager.Instance.currentDay;
        switch (currentDay)
        {
            case 1: return GetDaTime(day1);
            case 2: return GetDaTime(day2);
            case 3: return GetDaTime(day3);
            case 4: return GetDaTime(day4);
            case 5: return GetDaTime(day5);
            case 6: return GetDaTime(day6);
            case 7: return GetDaTime(day7);
        }
        return day1.deep.currentLinesToPlay; // fallback
    }
    private PlayerDialogueLine[] GetDaTime(DayInWeek currentDay)
    {
        currentTime = GameManager.Instance.currentTime;
        switch (currentTime)
        {
            case 0: return currentDay.morning.currentLinesToPlay;
            case 1: return currentDay.eve.currentLinesToPlay;
            case 2: return currentDay.dusk.currentLinesToPlay;
            case 3: return currentDay.midnight.currentLinesToPlay;
            case 4: return currentDay.deep.currentLinesToPlay;
        }
        return currentDay.deep.currentLinesToPlay; // fallback
        
    }

    public IEnumerator RunDialogue()
    {
        if (!returning) dialogue = GetDialogue();

        while (dialogue[currentLineIndex] != null)
        {
            yield return RunLine(dialogue[currentLineIndex]);
            currentLineIndex++;
            if (dialogue[currentLineIndex].end)
            {
                returning = false;
                break;
            }
            if (dialogue[currentLineIndex].next)
            {
                returning = true;
                break;
            }
        }
    }
    
    
    private IEnumerator RunLine(PlayerDialogueLine line)
    {
        advanceRequested = false;


        sfxSource.PlayOneShot(line.sound);
        typewriter.dialogueText.color = line.color;

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
    }

}


    






