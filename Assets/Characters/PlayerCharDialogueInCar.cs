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
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    private InputAction advanceAction;
    private bool advanceRequested;
    private bool waitingForPlayerInput = false;
    private float defaultTypeSpeed;

    private int currentDay;
    private int currentTime;

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


    // READ BETWEEN THE LINES MAN
    private int dayNum;
    private PlayerDialogueLine[] day1;
    private PlayerDialogueLine[] day2;
    private PlayerDialogueLine[] day3;
    private PlayerDialogueLine[] day4;
    private PlayerDialogueLine[] day5;
    private PlayerDialogueLine[] day6;
    private PlayerDialogueLine[] day7;

    private void GetDialogue(int day, int time)
    {
        
    }
    
    
    
    private IEnumerator RunLine(PlayerDialogueLine line)
    {
        advanceRequested = false;
        currentDay = GameManager.Instance.currentDay;
        currentTime = GameManager.Instance.currentTime;
        dayNum = GetDialogue(currentDay, currentTime);



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

    }


    



    private void EndLine()
    {
        
    }

    
}


// [Serializable]
// public class PlayerDialogueLine 
// {
//     public string id;
//     public string color;
//     public string text;
//     public string sound;
//     public float waitSeconds;
//     public float typeSpeed;
// }