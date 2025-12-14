using UnityEngine;

public class Money_Manager : MonoBehaviour
{
    public static Money_Manager Instance;

    public int money = 0;
    public int currentDayMoney = 0;
    public int quota = 0;

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
        currentDayMoney += amount;
        Debug.Log("Money changed by " + amount + ". New total: " + money);
    }

    public void UpdateQuotaAmount(int amount) 
    {
        quota -= amount;
        Debug.Log("Quota decreased by " + amount + ". New total: " + quota);
    }

    public void resetCurrentDayMoney()
    {
        currentDayMoney = 0;
    }


}
