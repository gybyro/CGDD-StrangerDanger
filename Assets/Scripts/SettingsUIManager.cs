using UnityEngine;

public class SettingsUIManager : MonoBehaviour
{
    public static SettingsUIManager Instance { get; private set; }

    [Header("References")]
    public GameObject settingsPanel;      // <- assign SettingPanel here in Inspector
    [HideInInspector] public GameObject mainMenuPanel;

    private bool openedFromMainMenu = false;
    private bool isOpen = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // IMPORTANT: this keeps the entire canvas object when loading new scenes
        DontDestroyOnLoad(gameObject);

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        Debug.Log("SettingsUIManager: Awake on " + gameObject.name);
    }

    void Update()
    {
        // Check ESC every frame
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log($"SettingsUIManager: ESC pressed. openedFromMainMenu={openedFromMainMenu}, isOpen={isOpen}");

            // ESC is only used for in-game, not while in main menu settings
            if (!openedFromMainMenu)
            {
                if (!isOpen) OpenInGame();
                else CloseInGame();
            }
        }
    }

    // ========= MAIN MENU ==========

    public void OpenFromMainMenu()
    {
        openedFromMainMenu = true;
        isOpen = true;

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);

        if (settingsPanel != null)
            settingsPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("SettingsUIManager: OpenFromMainMenu");
    }

    public void CloseFromMainMenu()
    {
        openedFromMainMenu = false;
        isOpen = false;

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);

        Debug.Log("SettingsUIManager: CloseFromMainMenu");
    }


    // ========= IN GAME ==========

    public void OpenInGame()
    {
        isOpen = true;

        if (settingsPanel != null)
            settingsPanel.SetActive(true);
        else
            Debug.LogWarning("SettingsUIManager: settingsPanel is NULL in OpenInGame");

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("SettingsUIManager: OpenInGame");
    }

    public void CloseInGame()
    {
        isOpen = false;
        openedFromMainMenu = false;

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        Time.timeScale = 1f;

        // Keep cursor usable after closing settings in-game:
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("SettingsUIManager: CloseInGame");
    }



    public void Close()
    {
        if (openedFromMainMenu)
        {
            CloseFromMainMenu();
        }
        else
        {
            CloseInGame();
        }
    }

}
