using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarCollider : MonoBehaviour
{
    public string sceneToLoad = "CarScene";
    
    [SerializeField] private Button nextSceneButton;     // UI button
    public TMP_Text btnText;
    [SerializeField] private SceneTransition sceneTransition; // reference to your fade script

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
        if (spawnByDoor) btnText.text = "Press E to Leave";
        else btnText.text = "I must finnish what I started...";
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
            LoadSceneFade();
        }
    }



    public void LoadSceneFade()
    {
        if (spawnByDoor) sceneTransition.LoadSceneWithFade(sceneToLoad);
    }

}