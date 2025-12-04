using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class Scene1Dialogue : MonoBehaviour
{
    public int primeInt = 1;

    public TMP_Text Char2name;
    public TMP_Text Char2speech;

    public GameObject DialogueDisplay;

    public GameObject CaraCustomer;      // Customer 1
    public GameObject JohnCustomer;      // Customer 3
    public GameObject VictoriaCustomer;  // Customer 2

    public GameObject DoorOpen;
    public GameObject DoorClosed;
    public GameObject ArtBG1;

    // Question buttons
    public GameObject ChoiceNameButton;
    public GameObject ChoiceWorkButton;
    public GameObject ChoiceAgeButton;

    public TMP_Text ChoiceNameText;
    public TMP_Text ChoiceWorkText;
    public TMP_Text ChoiceAgeText;

    // Stay/Leave buttons
    public GameObject StayButton;
    public GameObject LeaveButton;

    public GameObject nextButton;

    // Question tracking
    private bool askedName = false;
    private bool askedWork = false;
    private bool askedAge = false;
    private int questionsAsked = 0;

    // Which customer we're dealing with
    private bool isCustomer2 = false;
    private bool isCustomer3 = false;

    // Track if customer 2 has already lied once
    private bool hasLiedOnceForCustomer2 = false;

    // Customer data (true data from GameManager)
    private string cName;
    private int cAge;
    private string cWork;

    // Customer 3 name parts
    private string c3FirstName;
    private string c3FakeLastName;

    // Scenes to load
    public string BackToWalkingScene;   // normal continue
    public string leaveSceneName;       // when you leave
    public string deathSceneName;       // when you die (customer 2, stay)
    public string winSceneName;         // when you win (customer 3, enough money)
    public string loseSceneName;

    void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("Scene1Dialogue: No GameManager.Instance found in scene!");
            enabled = false;
            return;
        }

        DialogueDisplay.SetActive(false);

        // Door starts CLOSED, no customer visible
        DoorOpen.SetActive(false);
        DoorClosed.SetActive(true);

        CaraCustomer.SetActive(false);
        JohnCustomer.SetActive(false);
        VictoriaCustomer.SetActive(false);

        ArtBG1.SetActive(true);

        ChoiceNameButton.SetActive(false);
        ChoiceWorkButton.SetActive(false);
        ChoiceAgeButton.SetActive(false);

        StayButton.SetActive(false);
        LeaveButton.SetActive(false);

        nextButton.SetActive(true);

        // Assume GameManager has already generated data for the current customer
        LoadCustomerData();
        UpdateCustomerSprites(false); // don't show until door opens
    }

    private void LoadCustomerData()
    {
        int num = GameManager.Instance.currentCustomerNumber;

        isCustomer2 = (num == 2);
        isCustomer3 = (num == 3);

        // Always start from the TRUE data stored in GameManager
        var a = GameManager.Instance.currentActual;

        cName = a.name;
        cAge = a.age;
        cWork = a.workplace;

        if (isCustomer3)
        {
            string[] parts = a.name.Split(' ');
            c3FirstName   = parts.Length > 0 ? parts[0] : "Unknown";
            c3FakeLastName = (parts.Length > 1 ? parts[1] : "Lastname") + "son";
        }

        Debug.Log($"[Scene1Dialogue] Loaded data: name={cName}, age={cAge}, work={cWork}, is2={isCustomer2}, is3={isCustomer3}, num={num}");
    }

    /// <summary>
    /// Ensures only the correct customer sprite is shown.
    /// Mapping: 1 -> Cara, 2 -> Victoria, 3 -> John
    /// If visible = false, hide all.
    /// </summary>
    private void UpdateCustomerSprites(bool visible)
    {
        CaraCustomer.SetActive(false);
        JohnCustomer.SetActive(false);
        VictoriaCustomer.SetActive(false);

        if (!visible) return;

        int num = GameManager.Instance.currentCustomerNumber;

        if (num == 1)
        {
            CaraCustomer.SetActive(true);
        }
        else if (num == 2)
        {
            VictoriaCustomer.SetActive(true);
        }
        else if (num == 3)
        {
            JohnCustomer.SetActive(true);
        }
    }

    // ------------------------------
    // MAIN NEXT BUTTON
    // ------------------------------
    public void Next()
    {
        // If we just showed an answer, Next should go to question/tip logic
        if (primeInt == 101 || primeInt == 201 || primeInt == 301)
        {
            nextButton.SetActive(false);
            AfterAnswer();
            return;
        }

        primeInt++;

        Char2name.text = "Customer";
        Char2speech.text = "";

        if (primeInt == 2)
        {
            DialogueDisplay.SetActive(true);

            // Open the door now and show the customer
            DoorOpen.SetActive(true);
            DoorClosed.SetActive(false);
            UpdateCustomerSprites(true);

            if (!isCustomer2 && !isCustomer3)
                Char2speech.text = "Hey, you must be the delivery driver.";
            else if (isCustomer2)
                Char2speech.text = "Heyyy, pizza is here!";
            else
                Char2speech.text = "You're late.";
        }
        else if (primeInt == 3)
        {
            if (!isCustomer2 && !isCustomer3)
                Char2speech.text = "Man, I'm so hungry.";
            else if (isCustomer2)
                Char2speech.text = "Whoa, you were fast.";
            else
                Char2speech.text = "I am an important man, and I require my pizzas on time.";
        }
        else if (primeInt == 4)
        {
            if (!isCustomer2 && !isCustomer3)
                Char2speech.text = "Now give me the pizza.";
            else if (isCustomer2)
                Char2speech.text = "Now hurry and give me the pizza.";
            else
                Char2speech.text = "Give me the pizza now.";
        }
        else if (primeInt == 5)
        {
            nextButton.SetActive(false);
            ShowRemainingQuestions();
        }
        else if (primeInt == 600)
        {
            // TIP LINE AFTER SECOND QUESTION
            if (!isCustomer2 && !isCustomer3)
                Char2speech.text = "If you stay a little longer, I can go get a tip for you.";
            else if (isCustomer2)
                Char2speech.text = "Wait here while I get a tip for you.";
            else
                Char2speech.text = "I will retrieve the money and tip. Stay there.";

            nextButton.SetActive(false);
            StayButton.SetActive(true);
            LeaveButton.SetActive(true);
        }
    }

    // ------------------------------
    // QUESTIONS
    // ------------------------------
    private void ShowRemainingQuestions()
    {
        ChoiceNameButton.SetActive(!askedName);
        ChoiceWorkButton.SetActive(!askedWork);
        ChoiceAgeButton.SetActive(!askedAge);

        ChoiceNameText.text = "What is your name?";
        ChoiceWorkText.text = "Where do you work?";
        ChoiceAgeText.text = "How old are you?";
    }

    public void OnAskName()
    {
        askedName = true;
        questionsAsked++;

        DisableQuestions();
        nextButton.SetActive(true);

        primeInt = 100;
    }

    public void OnAskWork()
    {
        askedWork = true;
        questionsAsked++;

        DisableQuestions();
        nextButton.SetActive(true);

        primeInt = 200;
    }

    public void OnAskAge()
    {
        askedAge = true;
        questionsAsked++;

        DisableQuestions();
        nextButton.SetActive(true);

        primeInt = 300;
    }

    private void DisableQuestions()
    {
        ChoiceNameButton.SetActive(false);
        ChoiceWorkButton.SetActive(false);
        ChoiceAgeButton.SetActive(false);
    }

    // ------------------------------
    // CUSTOMER 2 LIE HELPERS
    // ------------------------------
    private string GetFakeName()
    {
        // Simple fake; you can customize
        return "Alex Turner";
    }

    private string GetFakeWork()
    {
        if (cWork == "Hotdog Stand")
            return "Night shift at the mall";
        return "Hotdog Stand";
    }

    private int GetFakeAge()
    {
        return cAge + Random.Range(3, 10);
    }

    // Decide if Customer 2 lies on THIS answer
    private bool ShouldCustomer2LieThisTime()
    {
        if (!isCustomer2)
            return false;

        // Already lied once → must tell truth now
        if (hasLiedOnceForCustomer2)
            return false;

        bool lie = Random.value < 0.5f;
        if (lie)
            hasLiedOnceForCustomer2 = true;

        return lie;
    }

    // ------------------------------
    // ANSWER HANDLING
    // ------------------------------
    void Update()
    {
        // NAME ANSWER
        if (primeInt == 100)
        {
            if (isCustomer2)
            {
                // Customer 2: maybe lie about name
                if (ShouldCustomer2LieThisTime())
                    Char2speech.text = $"My name? It's {GetFakeName()}.";
                else
                    Char2speech.text = $"My name? It's {cName}.";
            }
            else if (isCustomer3)
            {
                Char2speech.text = $"My name is {c3FirstName} {c3FakeLastName}.";
            }
            else
            {
                Char2speech.text = $"My name? It's {cName}.";
            }

            primeInt = 101;
        }

        // WORK ANSWER
        if (primeInt == 200)
        {
            if (isCustomer2)
            {
                if (ShouldCustomer2LieThisTime())
                    Char2speech.text = $"I work at {GetFakeWork()}.";
                else
                    Char2speech.text = $"I work at {cWork}.";
            }
            else if (isCustomer3)
            {
                Char2speech.text = "I am an important man, and my work is important.";
            }
            else
            {
                Char2speech.text = $"I work at {cWork}.";
            }

            primeInt = 201;
        }

        // AGE ANSWER
        if (primeInt == 300)
        {
            if (isCustomer2)
            {
                if (ShouldCustomer2LieThisTime())
                    Char2speech.text = $"I'm {GetFakeAge()}.";
                else
                    Char2speech.text = $"I'm {cAge}.";
            }
            else if (isCustomer3)
            {
                Char2speech.text = "How dare you ask my age? I'm old.";
            }
            else
            {
                Char2speech.text = $"I'm {cAge}.";
            }

            primeInt = 301;
        }

        // TIP LINE guard
        if (primeInt == 600)
        {
            if (!isCustomer2 && !isCustomer3)
                Char2speech.text = "If you stay a little longer, I can go get a tip for you.";
            else if (isCustomer2)
                Char2speech.text = "Wait here while I get a tip for you.";
            else
                Char2speech.text = "I will retrieve the money and tip. Stay there.";

            StayButton.SetActive(true);
            LeaveButton.SetActive(true);
            nextButton.SetActive(false);
            primeInt = 601; // prevent repeat
        }
    }

    private void AfterAnswer()
    {
        if (questionsAsked < 2)
        {
            ShowRemainingQuestions();
            return;
        }

        // After 2 questions → go to the tip flow
        primeInt = 600;
    }

    // ------------------------------
    // ENDING – STAY / LEAVE
    // ------------------------------
    public void OnStay()
    {
        StayButton.SetActive(false);
        LeaveButton.SetActive(false);
        nextButton.SetActive(false);

        Char2name.text = "Customer";

        int num   = GameManager.Instance.currentCustomerNumber;
        int money = Money_Manager.Instance.money;

        Debug.Log($"STAY CLICKED → Customer {num}, Money = ${money}, Truth = {GameManager.Instance.currentTruth}");

        // 1) CUSTOMER 2 → ALWAYS DEATH
        if (num == 2)
        {
            Char2speech.text = "Customer was BAD.";
            if (!string.IsNullOrEmpty(deathSceneName))
                StartCoroutine(WaitAndLoadScene(deathSceneName));
            return;
        }

        // 2) CUSTOMER 3 → WIN or LOSE based on money
        if (num == 3)
        {
            if (money >= 35)
            {
                Char2speech.text = "Customer was GOOD. You earned enough tips!";
                if (!string.IsNullOrEmpty(winSceneName))
                    StartCoroutine(WaitAndLoadScene(winSceneName));
            }
            else
            {
                Char2speech.text = "Customer was GOOD... but you didn't earn enough money.";
                if (!string.IsNullOrEmpty(loseSceneName))
                    StartCoroutine(WaitAndLoadScene(loseSceneName));
            }
            return;
        }

        // 3) CUSTOMER 1 → normal continue
        Char2speech.text = "Customer was GOOD.";
        if (!string.IsNullOrEmpty(BackToWalkingScene))
            StartCoroutine(WaitAndLoadScene(BackToWalkingScene));
    }



    public void OnLeave()
    {
        StayButton.SetActive(false);
        LeaveButton.SetActive(false);
        nextButton.SetActive(false);

        Char2name.text = "Customer";
        Char2speech.text = "You left.";

        int num   = GameManager.Instance.currentCustomerNumber;
        int money = Money_Manager.Instance.money;

        Debug.Log($"LEAVE CLICKED → Customer {num}, Money = ${money}");

        // If this is CUSTOMER 3, we should still check win/lose,
        // because the day is essentially over after this one.
        if (num == 3)
        {
            if (money >= 35)
            {
                Char2speech.text = "You left... but you earned enough money!";
                if (!string.IsNullOrEmpty(winSceneName))
                    StartCoroutine(WaitAndLoadScene(winSceneName));
            }
            else
            {
                Char2speech.text = "You left... and you didn't earn enough money.";
                if (!string.IsNullOrEmpty(loseSceneName))
                    StartCoroutine(WaitAndLoadScene(loseSceneName));
            }
            return;
        }

        // For customer 1 and 2, leaving just sends you back to the walking/car scene
        if (!string.IsNullOrEmpty(leaveSceneName))
            StartCoroutine(WaitAndLoadScene(leaveSceneName));
    }


    private IEnumerator WaitAndLoadScene(string sceneName)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }
}
//Generate