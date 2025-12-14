using UnityEngine;

public class ShadowPersonController : MonoBehaviour
{
    [Header("Assign your shadow GameObject here")]
    public GameObject shadowPerson;

    public GameObject shadowMan_Stage1;
    public GameObject shadowMan_Stage2;

    private int currentDay;
    private int currentTime;

    

    // Optional: only apply logic once when this scene loads
    private bool hasUpdatedOnce = false;

    private void Start()
    {
        currentDay  = GameManager.Instance.GetDay();
        currentTime = GameManager.Instance.GetTime();
        UpdateShadow();
    }

    // You can also call this from other scripts if needed
    public void UpdateShadow()
    {
        if (hasUpdatedOnce) return;

        // if (GameManager.Instance == null || Money_Manager.Instance == null)
        // {
        //     Debug.LogError("ShadowPersonController: Missing GameManager or Money_Manager!");
        //     return;
        // }

        // int customersVisited = GameManager.Instance.GetTime();
        int money = Money_Manager.Instance.currentDayMoney;

        // Debug.Log($"[Shadow] Customers visited = {customersVisited}, Money = {money}");

        // --- SIMPLE RULE FOR NOW ---
        // After visiting at least 1 customer:
        // - If money == 15 → do nothing (shadow stays off)
        // - If money == 10 → show shadow



        if (currentTime > 2)
        {   shadowMan_Stage1.SetActive(true);
            if (money < 16)
            {
                // Player is doing badly → shadow appears
                if (shadowPerson != null)
                    shadowMan_Stage1.SetActive(false);
                    shadowMan_Stage2.SetActive(true);
            }
            else if (money > 16)
            {
                // Doing okay → shadow hidden
                if (shadowPerson != null)
                    shadowMan_Stage1.SetActive(true);
                    shadowMan_Stage2.SetActive(false);
            }


            hasUpdatedOnce = true; // comment this out if you want it to keep updating
        }
    }
}
