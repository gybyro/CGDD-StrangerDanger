using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DoorCollider : MonoBehaviour
{
    public string sceneToLoad = "GB_HouseScene";
    
    [SerializeField] private Button nextSceneButton;     // UI button
    public TMP_Text btnText;
    [SerializeField] private SceneTransition sceneTransition; // reference to your fade script
    [SerializeField] private AudioSource knockAudio;


    private bool playerInside = false;
    public bool spawnByDoor;

    private void Start()
    {
        if (nextSceneButton != null)
            nextSceneButton.gameObject.SetActive(false);

        spawnByDoor = GameManager.Instance.walkingSceneSpawnByDoor;
        SetBtnText();
        
    }
    private void SetBtnText()
    {
        if (!spawnByDoor) btnText.text = "Press E \n to knock";
        else btnText.text = "";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && nextSceneButton != null)
        {
            playerInside = true;
            nextSceneButton.gameObject.SetActive(true);
            Debug.Log("Player entered trigger, enabling button");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && nextSceneButton != null)
        {
            playerInside = false;
            nextSceneButton.gameObject.SetActive(false);
            Debug.Log("Player left trigger, disabling button");
        }
    }

    private void Update()
    {
        // If player is inside trigger and presses E
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E pressed inside trigger - knocking + loading scene");
            if (!spawnByDoor) PlayKnockAndLoad();   // <-- new function to handle audio + load
        }
    }

    public void PlayKnockAndLoad()
    {
        if (knockAudio != null )
            knockAudio.Play();

        // Wait for audio to finish, then fade + load
        float delay = knockAudio != null && knockAudio.clip != null
            ? knockAudio.clip.length
            : 0f;

        Invoke(nameof(LoadSceneFade), delay);
    }

    private void LoadSceneFade()
    {
        sceneTransition.LoadSceneWithFade(sceneToLoad);
    }

}