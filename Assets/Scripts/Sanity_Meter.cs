using UnityEngine;

public class Sanity_Meter : MonoBehaviour
{
    // Singleton instance
    public static Sanity_Meter Instance;

    // Starting sanity (change this to whatever you want)
    public int Player_Sanity = 100;

    private void Awake()
    {
        // Basic singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // If you want sanity to persist between scenes:
        // DontDestroyOnLoad(gameObject);
    }

    // Call this to lower sanity
    public void Lower_Sanity(int amount)
    {
        Player_Sanity -= amount;

        // Optional: clamp so it doesn't go below 0
        if (Player_Sanity < 0)
            Player_Sanity = 0;

        Debug.Log($"Sanity lowered by {amount}. Current sanity = {Player_Sanity}");

        //Just making an if statement that should make something happen (placeholder until we have some effect we can add)
        if (Player_Sanity <= 75) 
        {
            ShowEvent();
        }
    }

    public void ShowEvent() 
    {
        return; 
    }
}