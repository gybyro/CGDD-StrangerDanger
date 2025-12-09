using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StoryLine : MonoBehaviour
{
    [Header("References")]
    public DialogueManager dialogueManager;

    private string defaultDialogue = "dial_test2";
    private string currentDialogue;
    private bool hasStarted = false;


    void Awake()
    {
        // Pull dialogue key from GameManager
        if (GameManager.Instance != null && !string.IsNullOrEmpty(GameManager.Instance.nextDialogue))
            currentDialogue = GameManager.Instance.nextDialogue;
        else
            currentDialogue = defaultDialogue; // fallback
    }

    void Start()
    {
        if (hasStarted) return;
        hasStarted = true;

        if (string.IsNullOrEmpty(currentDialogue))
            currentDialogue = defaultDialogue;

        StartCoroutine(RunStory());


        // 1st dialogue = "start
        // 2nd = "dial_test2"
        // 3rd = "character dialouge no..."

        // in gameManager, every character should have their own "nextDialogue" which is set up
        // based on the choices you made from the json file,

        // storyLine should dictate which character plays the next time this scene is called
        // StartCoroutine(dialogueManager.RunDialogue(currentDialogue)); should only be loaded once per this scene load



    }

    private IEnumerator RunStory()
    {
        // run the dialogue once for this scene
        yield return StartCoroutine(dialogueManager.RunDialogue(currentDialogue));

        // After dialogue ends, tell GameManager we finished this scene’s dialogue
        if (GameManager.Instance != null) { GameManager.Instance.lastDialoguePlayed = currentDialogue; }

        Debug.Log("StoryLine finished dialogue: " + currentDialogue);

        string sceneNameToLoad = "WalkingScene";

        SceneManager.LoadScene(sceneNameToLoad);
        Debug.Log($"Scene {sceneNameToLoad} loaded");
    }
}