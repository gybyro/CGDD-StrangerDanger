using UnityEngine;
using UnityEngine.SceneManagement;

public class Time_Kill_Player : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Money_Manager.Instance.money < 35)
            {
                SceneManager.LoadScene("GameOver_Screen");
            }
            else
            {
                SceneManager.LoadScene("Win_Screen");
            }
        }
    }
}