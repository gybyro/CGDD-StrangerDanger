using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public string menuSceneName = "MainMenu";

    public void OnMainMenuClicked()
    {
        // Reset all game data before loading menu
        if (GameManager.Instance != null)
            GameManager.Instance.ResetAllData();

        if (Money_Manager.Instance != null)
            Money_Manager.Instance.money = 0;

        // Load main menu scene
        SceneManager.LoadScene(menuSceneName);
    }
}
