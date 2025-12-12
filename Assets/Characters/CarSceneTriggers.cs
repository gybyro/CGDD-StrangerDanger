using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class CarSceneTriggers : MonoBehaviour
{

    [Header("UI")]
    public CanvasGroup textBox;
    public CanvasGroup nameBox;
    public TMP_Text nameBoxText;
    public Typewriter typewriter;


    [Header("References")]
    public PlayerCharDialogueInCar dialogueManager;
    public PlayerInput playerInput;
    public AudioSource sfxSource;

    private int currentDay;
    private int currentTime;



    private InputAction advanceAction;
    private bool advanceRequested;
    private bool waitingForPlayerInput = false;
    private float defaultTypeSpeed;



    private PlayerDialogueLine[] dialogue;
    private int currentLineIndex = 0;
    private bool returning;

    void Awake()
    {
        currentDay = GameManager.Instance.currentDay;
        currentTime = GameManager.Instance.currentTime;
    }

    void Start()
    {
        switch (currentDay)
        {
            case 1:  PlayDay01(); break;
            case 2:  GetDaTime(2); break;
            case 3:  GetDaTime(3); break;
            case 4:  GetDaTime(4); break;
            case 5:  GetDaTime(5); break;
            case 6:  GetDaTime(6); break;
            case 7:  GetDaTime(7); break;
        }
    }
    private void GetDaTime(int currentDay)
    {
        currentTime = GameManager.Instance.currentTime;
        // switch (currentTime)
        // {
        //     case 0: return currentDay.morning.currentLinesToPlay;
        //     case 1: return currentDay.eve.currentLinesToPlay;
        //     case 2: return currentDay.dusk.currentLinesToPlay;
        //     case 3: return currentDay.midnight.currentLinesToPlay;
        //     case 4: return currentDay.deep.currentLinesToPlay;
        // }
        
    }



    private void PlayDay01()
    {
        switch (currentTime)
        {
            case 0: // INTROOO
                {


                    break;
                } 
            // case 1: return currentDay.eve.currentLinesToPlay;
            // case 2: return currentDay.dusk.currentLinesToPlay;
            // case 3: return currentDay.midnight.currentLinesToPlay;
            // case 4: return currentDay.deep.currentLinesToPlay;
        }
        
    }

}