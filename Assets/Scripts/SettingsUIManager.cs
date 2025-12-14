using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SettingsUIManager : MonoBehaviour
{
    public static SettingsUIManager Instance { get; private set; }

    [Header("References")]
    public GameObject settingsPanel;
    public GameObject mainMenuPanel;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound;

    private bool isOpen = false;
    private bool openedFromMainMenu = false;

    private CursorLockMode prevCursorLock;
    private bool prevCursorVisible;
    private float prevTimeScale;

    public bool IsOpen => isOpen;                  // NEW
    public bool ShouldBlockGameplayInput => isOpen && !openedFromMainMenu; // NEW

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isOpen) OpenInGame();
            else CloseInGame();
        }
    }

    public void OpenFromMainMenu()
    {
        PlayOpenSound();

        openedFromMainMenu = true;
        isOpen = true;

        Time.timeScale = 1f;

        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        ClearUISelection();
    }

    public void CloseFromMainMenu()
    {
        PlayCloseSound();

        openedFromMainMenu = false;
        isOpen = false;

        Time.timeScale = 1f;

        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        ClearUISelection();
    }

    public void OpenInGame()
    {
        PlayOpenSound();

        openedFromMainMenu = false;
        isOpen = true;

        if (settingsPanel != null)
            settingsPanel.SetActive(true);

        EnterMenuMode(true);
        ClearUISelection();
    }

    public void CloseInGame()
    {
        PlayCloseSound();

        isOpen = false;
        openedFromMainMenu = false;

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        ExitMenuMode(true);
        ClearUISelection();
    }

    public void Close()
    {
        if (openedFromMainMenu) CloseFromMainMenu();
        else CloseInGame();
    }

    void EnterMenuMode(bool pauseGame)
    {
        prevCursorLock = Cursor.lockState;
        prevCursorVisible = Cursor.visible;
        prevTimeScale = Time.timeScale;

        if (pauseGame) Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void ExitMenuMode(bool pauseGame)
    {
        if (pauseGame) Time.timeScale = prevTimeScale;

        Cursor.lockState = prevCursorLock;
        Cursor.visible = prevCursorVisible;
    }

    void ClearUISelection()
    {
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
    }

    void PlayOpenSound()
    {
        if (audioSource != null && openSound != null)
            audioSource.PlayOneShot(openSound);
    }

    void PlayCloseSound()
    {
        if (audioSource != null && closeSound != null)
            audioSource.PlayOneShot(closeSound);
    }
}
