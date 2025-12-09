using UnityEngine;
using TMPro;   // ← ADD THIS

public class deathScrean : MonoBehaviour
{
    public TextMeshProUGUI totalMoneyText;

    private void OnEnable()
    {
        UpdateMoneyDisplay();
    }

    private void UpdateMoneyDisplay()
    {
        if (Money_Manager.Instance != null)
        {
            int money = Money_Manager.Instance.money;
            totalMoneyText.text = "Total Money: " + money;
        }
        else
        {
            totalMoneyText.text = "Total Money: 0";
        }
    }

    public void RestartGame()
    {
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
