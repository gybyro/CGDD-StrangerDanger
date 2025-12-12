using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public string menuSceneName = "MainMenu";
    public GameObject mainMenuPanel;

    private void Start()
    {
        if (SettingsUIManager.Instance != null)
        {
            SettingsUIManager.Instance.mainMenuPanel = mainMenuPanel;
            Debug.Log("MainMenuScript: Registered mainMenuPanel with SettingsUIManager");
        }
        else
        {
            Debug.LogWarning("MainMenuScript: SettingsUIManager.Instance is NULL in Start");
        }
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
        Debug.Log("MainMenuScript: OpenSettings called");
        if (SettingsUIManager.Instance != null)
        {
            SettingsUIManager.Instance.OpenFromMainMenu();
        }
        else
        {
            Debug.LogWarning("MainMenuScript: SettingsUIManager.Instance is NULL in OpenSettings");
        }
    }
}
