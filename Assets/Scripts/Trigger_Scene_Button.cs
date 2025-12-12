using UnityEngine;

public class Trigger_Scene_Button : MonoBehaviour
{
    [SerializeField] private GameObject nextSceneButton;     // UI button
    [SerializeField] private SceneTransition sceneTransition; // reference to your fade script
    [SerializeField] private AudioSource knockAudio;

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
        // If player is inside trigger and presses E
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E pressed inside trigger - knocking + loading scene");
            PlayKnockAndLoad();   // <-- new function to handle audio + load
        }
    }

    public void PlayKnockAndLoad()
    {
        if (knockAudio != null)
            knockAudio.Play();

        // Wait for audio to finish, then fade + load
        float delay = knockAudio != null && knockAudio.clip != null
            ? knockAudio.clip.length
            : 0f;

        Invoke(nameof(LoadSceneFade), delay);
    }

    private void LoadSceneFade()
    {
        // sceneTransition.LoadSceneWithFade();
    }

}