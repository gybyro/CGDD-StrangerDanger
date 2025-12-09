using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public enum CustomerTruthState { Good, Bad }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string nextDialogue = "dial_test2";
    public string lastDialoguePlayed;

    public int currentCustomerNumber = 0;
    public int nextCustomerNumber = 1;

    private int day = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }




        // Start from scratch
            currentCustomerNumber = 0;
            nextCustomerNumber = 1;
    }

    public void AdvanceDay()
    {
        
    }




    public void ResetAllData()
    {

        Debug.Log("GAME MANAGER RESET");
    }
    public void GenerateNextCustomer() {}
    public string GetPhoneDescription() { return ""; }
}
