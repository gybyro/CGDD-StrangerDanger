using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public enum CustomerTruthState { Good, Bad }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    // car stuff
    public int carPhase = 0;

    public int currentCustomerNumber = 0;
    public int nextCustomerNumber = 1;



    private int currentDay = 0;
    private int currentTime = 0;

    private string char_00_nextDialogue = "start"; // le test
    private string char_01_nextDialogue = "dial_tired_01"; // tiered is first character
    private string char_02_nextDialogue = "dial_proxy_01"; // proxy is seconf 
    private string char_03_nextDialogue = "dial_visitor_01"; // last of day 1 is visitor
    private string char_04_nextDialogue = "dial_hippie_01"; //
    private string char_05_nextDialogue = "dial_grumpy_01"; //
    private string char_06_nextDialogue = "dial_mary_01"; //
    private string char_07_nextDialogue = "dial_concerned_01"; //


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

        // Start from scratch // delete later
            currentCustomerNumber = 0;
            nextCustomerNumber = 1;
    }

    // CAR STUFF ==========================================
    public void AdvanceCarPhase()
    {
        carPhase++;
        Debug.Log("[GM] Car phase is now: " + carPhase);
    }

    public void ResetCarPhase()
    {
        carPhase = 0;
        Debug.Log("[GM] Car phase reset to 0");
    }


    // DAY STUFF ==========================================
    public void AdvanceTime(bool morning)
    {
        // currentTime can be 1- 4
        // 1 = evening
        // 2 = dusk
        // 3 = midnight
        // 4 = morning

        if (!morning)
        {
            if (currentTime >= 3)
            {
                currentTime = 1;
                if (currentTime == 3) { currentDay++; }
            }
            else { currentTime++; }
        }
        else
        {
            currentTime = 4;
            currentDay++;
        }
    }

    // CHARACTER STUFF =====================================
    public void AdvanceCharDial(int index, string nextDialogue)
    {
        if (index == 0) {char_00_nextDialogue = nextDialogue; }
        else if (index == 1) {char_01_nextDialogue = nextDialogue; }
        else if (index == 2) {char_02_nextDialogue = nextDialogue; }
        else if (index == 3) {char_03_nextDialogue = nextDialogue; }
        else if (index == 4) {char_04_nextDialogue = nextDialogue; }
        else if (index == 5) {char_05_nextDialogue = nextDialogue; }
        else if (index == 6) {char_06_nextDialogue = nextDialogue; }
        else if (index == 7) {char_07_nextDialogue = nextDialogue; }
    }
    public string GetNextDialogue(int index)
    {
        if (index == 0) return char_00_nextDialogue;
        else if (index == 1) return char_01_nextDialogue;
        else if (index == 2) return char_02_nextDialogue;
        else if (index == 3) return char_03_nextDialogue;
        else if (index == 4) return char_04_nextDialogue;
        else if (index == 5) return char_05_nextDialogue;
        else if (index == 6) return char_06_nextDialogue;
        else if (index == 7) return char_07_nextDialogue;
        else return char_00_nextDialogue;
    }


    public void ResetAllData() // delete later I think
    {

        Debug.Log("GAME MANAGER RESET");
    }
    public void GenerateNextCustomer() {}
    public string GetPhoneDescription() { return ""; }
}


