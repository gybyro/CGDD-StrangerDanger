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
    public GameObject JohnCustomer;
    public GameObject VictoriaCustomer;

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

    // Customer data (pulled from GameManager)
    private string cName;
    private int cAge;
    private string cWork;

    private string c3FirstName;
    private string c3FakeLastName;

    // Scenes to load
    public string BackToWalkingScene;   // when you stay
    public string leaveSceneName;       // when you leave

    void Start()
    {
        // Basic null-safety: if there is no GameManager, stop this script
        if (GameManager.Instance == null)
        {
            Debug.LogError("Scene1Dialogue: No GameManager.Instance found in scene!");
            enabled = false;
            return;
        }

        DialogueDisplay.SetActive(false);
        DoorOpen.SetActive(true);
        DoorClosed.SetActive(false);

        JohnCustomer.SetActive(false);
        VictoriaCustomer.SetActive(false);

        ArtBG1.SetActive(true);

        ChoiceNameButton.SetActive(false);
        ChoiceWorkButton.SetActive(false);
        ChoiceAgeButton.SetActive(false);

        StayButton.SetActive(false);
        LeaveButton.SetActive(false);

        nextButton.SetActive(true);

        // Only generate if no customer has been generated yet
        if (GameManager.Instance.currentCustomerNumber == 0)
            GameManager.Instance.GenerateNextCustomer();

        LoadCustomerData();
    }

    private void LoadCustomerData()
    {
        int num = GameManager.Instance.currentCustomerNumber;

        isCustomer2 = (num == 2);
        isCustomer3 = (num == 3);

        if (isCustomer2)
        {
            // Customer 2 uses possibly wrong spoken data
            cName = GameManager.Instance.currentSpoken.name;
            cAge = GameManager.Instance.currentSpoken.age;
            cWork = GameManager.Instance.currentSpoken.workplace;
        }
        else if (isCustomer3)
        {
            var a = GameManager.Instance.currentActual;

            string[] parts = a.name.Split(' ');
            c3FirstName = parts.Length > 0 ? parts[0] : "Unknown";
            c3FakeLastName = (parts.Length > 1 ? parts[1] : "Lastname") + "son";

            cName = a.name;
            cAge = a.age;
            cWork = a.workplace;
        }
        else
        {
            // Customer 1 – correct data
            var a = GameManager.Instance.currentActual;
            cName = a.name;
            cAge = a.age;
            cWork = a.workplace;
        }

        Debug.Log($"Loaded data: name={cName}, age={cAge}, work={cWork}, is2={isCustomer2}, is3={isCustomer3}");
    }

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
            DoorOpen.SetActive(false);
            DoorClosed.SetActive(true);

            // pick which sprite to show
            if (!isCustomer2 && !isCustomer3)
            {
                JohnCustomer.SetActive(true);
            }
            else if (isCustomer2)
            {
                // TODO: add customer 2 sprite here when you have one
                JohnCustomer.SetActive(true); // temp
            }
            else if (isCustomer3)
            {
                VictoriaCustomer.SetActive(true);
            }

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

    private void ShowRemainingQuestions()
    {
        ChoiceNameButton.SetActive(!askedName);
        ChoiceWorkButton.SetActive(!askedWork);
        ChoiceAgeButton.SetActive(!askedAge);

        ChoiceNameText.text = "What is your name?";
        ChoiceWorkText.text = "Where do you work?";
        ChoiceAgeText.text = "How old are you?";
    }

    // ------------------------------
    // QUESTION BUTTONS
    // ------------------------------
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
    // ANSWER HANDLING
    // ------------------------------
    void Update()
    {
        // NAME ANSWER
        if (primeInt == 100)
        {
            if (!isCustomer3)
                Char2speech.text = $"My name? It's {cName}.";
            else
                Char2speech.text = $"My name is {c3FirstName} {c3FakeLastName}.";

            primeInt = 101;
        }

        // WORK ANSWER
        if (primeInt == 200)
        {
            if (!isCustomer3)
                Char2speech.text = $"I work at {cWork}.";
            else
                Char2speech.text = "I am an important man, and my work is important.";

            primeInt = 201;
        }

        // AGE ANSWER
        if (primeInt == 300)
        {
            if (!isCustomer3)
                Char2speech.text = $"I'm {cAge}.";
            else
                Char2speech.text = "How dare you ask my age? I'm old.";

            primeInt = 301;
        }

        // TIP LINE extra guard: if something set primeInt to 600, ensure we show tip once
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

        if (GameManager.Instance.currentTruth == CustomerTruthState.Bad)
        {
            Char2speech.text = "Customer was BAD.";
            if (!string.IsNullOrEmpty(BackToWalkingScene))
                StartCoroutine(WaitAndLoadScene(BackToWalkingScene));
        }
        else
        {
            Char2speech.text = "Customer was GOOD.";
            if (!string.IsNullOrEmpty(BackToWalkingScene))
                StartCoroutine(WaitAndLoadScene(BackToWalkingScene));
        }
    }

    public void OnLeave()
    {
        StayButton.SetActive(false);
        LeaveButton.SetActive(false);
        nextButton.SetActive(false);

        Char2name.text = "Customer";
        Char2speech.text = "You left.";

        if (!string.IsNullOrEmpty(leaveSceneName))
            StartCoroutine(WaitAndLoadScene(leaveSceneName));
    }

    private IEnumerator WaitAndLoadScene(string sceneName)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }
}
