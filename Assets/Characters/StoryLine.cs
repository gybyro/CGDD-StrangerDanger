using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StoryLine : MonoBehaviour
{
    [Header("References")]
    public DialogueManager dialogueManager;
    public bool testDialogue = false;
    public string ifTestDialogueFileName = "start";

    private string defaultDialogue = "start";
    private string currentDialogue;
    private bool hasStarted = false;
    private int charIndex = 0;

    private int currentDay;
    private int currentTime;

    void Start()
    {
        if (hasStarted) return;
        hasStarted = true;

        if (string.IsNullOrEmpty(currentDialogue))
            currentDialogue = defaultDialogue;

        if (testDialogue) StartCoroutine(dialogueManager.RunDialogue(ifTestDialogueFileName));
        else
        {
            RunDay();
        }

        


        // 1st dialogue = "start
        // 2nd = "dial_test2"
        // 3rd = "character dialouge no..."

        // in gameManager, every character should have their own "nextDialogue" which is set up
        // based on the choices you made from the json file,

        // storyLine should dictate which character plays the next time this scene is called
        // StartCoroutine(dialogueManager.RunDialogue(currentDialogue)); should only be loaded once per this scene load



    }


    private void RunDay()
    {
        // next time slot    // REMEMBER TO MOVE THIS TO THE CAR
        GameManager.Instance.AdvanceTime();
        
        currentDay = GameManager.Instance.currentDay;
        currentTime = GameManager.Instance.currentTime;
        
        Debug.Log("Current Day: " + currentDay + "Current Time: " + currentTime);

        if (currentDay == 1 || currentDay == 2) // DAY 1 & 2
        {
            charIndex = currentTime;
            
            // if (currentTime == 0) charIndex = 0;
            // else if (currentTime == 1) charIndex = 1;
            // else if (currentTime == 2) charIndex = 2;
            // else if (currentTime == 3) charIndex = 3;   
        }
            
        
        currentDialogue = GameManager.Instance.GetNextDialogue(charIndex);
        Debug.Log("GetNextDialogue Called from StoryLine! index: " + charIndex + ", nextDialogue: " + currentDialogue);

      

        StartCoroutine(RunStory());
    }

    private IEnumerator RunStory()
    {
        // run the dialogue once for this scene
        Debug.Log("Current dialogue: " + currentDialogue);
        yield return StartCoroutine(dialogueManager.RunDialogue(currentDialogue));

        // needs to be loaded AFTERWARDS >:(
        string sceneNameToLoad = "WalkingScene";
        SceneManager.LoadScene(sceneNameToLoad);
        Debug.Log($"Scene {sceneNameToLoad} loaded");
    }
}