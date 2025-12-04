using UnityEngine;

public class deathScrean : MonoBehaviour
{
    public void RestartGame()
    {
        // Reset game data before returning to the main menu
        if (GameManager.Instance != null)
            GameManager.Instance.ResetAllData();

        if (Money_Manager.Instance != null)
            Money_Manager.Instance.money = 0;

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
