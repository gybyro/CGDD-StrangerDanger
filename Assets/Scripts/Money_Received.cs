using UnityEngine;

public class Money_Received : MonoBehaviour
{
    // You can tweak these in the inspector if you want different values later
    [SerializeField] private int safeReward = 10;
    [SerializeField] private int riskyReward = 15;

    // Called by the "safe" button (1A)
    public void OnSafeChoice()
    {
        Money_Manager.Instance.AddMoney(safeReward);
        // Continue dialogue, close UI, etc...
    }

    // Called by the "risky" button (1B)
    public void OnRiskyChoice()
    {
        Money_Manager.Instance.AddMoney(riskyReward);
        // Continue dialogue, close UI, etc...
    }
}