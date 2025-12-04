using UnityEngine;

public class Money_Manager : MonoBehaviour
{
    public static Money_Manager Instance;

    public int money = 0;

    private void Awake() 
    {
        if (Instance != null && Instance != this) 
        {
            Destroy(gameObject);
            return; 
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddMoney(int amount) 
    {
        money += amount;
        Debug.Log("Money changed by " + amount + ". New total: " + money);
    }
}
