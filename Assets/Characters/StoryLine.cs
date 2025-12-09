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
    private int gameTick = 0;
    private int charIndex = 0;

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
        if (gameTick == 0) {
            // day before
            GameManager.Instance.AdvanceTime(true); // day time
            currentDialogue = "start";
        }
        else
        {
            if (gameTick == 1) charIndex = 1;
            else if (gameTick == 2) charIndex = 2;
            else if (gameTick == 3) charIndex = 3;
            else if (gameTick == 4) charIndex = 1;
            else if (gameTick == 5) charIndex = 2;
            else if (gameTick == 6) charIndex = 3;
            currentDialogue = GameManager.Instance.GetNextDialogue(charIndex);
        }
        
        GameManager.Instance.AdvanceTime(false);
        gameTick++;
        StartCoroutine(RunStory());
        
    }

    private IEnumerator RunStory()
    {
        // run the dialogue once for this scene
        yield return StartCoroutine(dialogueManager.RunDialogue(currentDialogue));


        Debug.Log("StoryLine finished dialogue: " + currentDialogue);

        string sceneNameToLoad = "WalkingScene";

        SceneManager.LoadScene(sceneNameToLoad);
        Debug.Log($"Scene {sceneNameToLoad} loaded");
    }
}