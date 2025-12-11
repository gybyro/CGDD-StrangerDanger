using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public string menuSceneName = "MainMenu";
    public GameObject mainMenuPanel;

    void Start()
    {
        // Tell SettingsUIManager which panel is the main menu
        if (SettingsUIManager.Instance != null)
            SettingsUIManager.Instance.mainMenuPanel = mainMenuPanel;
    }

    public void OnMainMenuClicked()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.ResetAllData();

        if (Money_Manager.Instance != null)
            Money_Manager.Instance.money = 0;

        SceneManager.LoadScene(menuSceneName);
    }

    public void OpenSettings()
    {
        Debug.Log("Open settings from main menu");
        if (SettingsUIManager.Instance != null)
        {
            SettingsUIManager.Instance.OpenFromMainMenu();
        }
    }
}
