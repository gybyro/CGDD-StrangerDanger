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

    public enum DialogueStopReason { Ended, Paused }


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
    public List<DayInWeek> week = new List<DayInWeek>();

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


    /// Get get the current day and time 
    private PlayerDialogueLine[] GetDialogue()
    {
        int dayIndex = GameManager.Instance.currentDay - 1;
        int timeIndex = GameManager.Instance.currentTime;

        if (dayIndex < 0 || dayIndex >= week.Count)
            return null;

        return GetTimeSlot(week[dayIndex], timeIndex);
    }
    private PlayerDialogueLine[] GetTimeSlot(DayInWeek day, int timeIndex)
    {
        if (timeIndex < 0 || timeIndex >= day.timeSlots.Length)
            return null;

        return day.timeSlots[timeIndex].currentLinesToPlay;
    }





    public IEnumerator RunDialogue(System.Action<DialogueStopReason> onStop)
    {
        if (!returning) {
            dialogue = GetDialogue();
            currentLineIndex = 0;
        }

        if (dialogue == null || dialogue.Length == 0)
        {
            onStop?.Invoke(DialogueStopReason.Ended);
            yield break;
        }

        while (currentLineIndex < dialogue.Length)
        {
            PlayerDialogueLine line = dialogue[currentLineIndex];
            yield return RunLine(line);

            if (line.end) {
                returning = false;
                onStop?.Invoke(DialogueStopReason.Ended);
                yield break;
            }
            if (line.next) {
                currentLineIndex++;  // resume on the NEXT line
                returning = true;
                onStop?.Invoke(DialogueStopReason.Paused);
                yield break;
            }
            currentLineIndex++;
        }
        returning = false;
        onStop?.Invoke(DialogueStopReason.Ended);
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


    






