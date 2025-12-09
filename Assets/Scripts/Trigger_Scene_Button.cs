using UnityEngine;

public class Trigger_Scene_Button : MonoBehaviour
{
    [SerializeField] private GameObject nextSceneButton;     // UI button
    [SerializeField] private SceneTransition sceneTransition; // reference to your fade script

    private bool playerInside = false;

    private void Start()
    {
        if (nextSceneButton != null)
            nextSceneButton.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && nextSceneButton != null)
        {
            playerInside = true;
            nextSceneButton.SetActive(true);
            Debug.Log("Player entered trigger, enabling button");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && nextSceneButton != null)
        {
            playerInside = false;
            nextSceneButton.SetActive(false);
            Debug.Log("Player left trigger, disabling button");
        }
    }

    private void Update()
    {
        // If player is inside trigger and presses E, do same as clicking the button
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E pressed inside trigger - loading scene");
            sceneTransition.LoadSceneWithFade();   // or sceneTransition.LoadScene();
        }
    }
}