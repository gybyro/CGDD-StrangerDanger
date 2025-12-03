using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public enum CustomerTruthState
{
    Good,
    Bad
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public CanvasGroup fadeGroup;
    public float fadeTime = 1f;






    [System.Serializable]
    public class CustomerData
    {
        public string name;
        public int age;
        public string workplace;
    }

    // Customer templates (always correct)
    public CustomerData customer1 = new CustomerData();
    public CustomerData customer2 = new CustomerData();
    public CustomerData customer3 = new CustomerData();

    // Active customer data
    public CustomerData currentActual = new CustomerData(); // Real correct data
    public CustomerData currentSpoken = new CustomerData(); // What customer says (may be wrong)

    // Truth state for “Customer was GOOD/BAD”
    public CustomerTruthState currentTruth;

    // Tracks which customer you are on (1,2,3)
    public int currentCustomerNumber = 0;

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
    }

    private void Start()
    {
        // Scene Fade
        fadeGroup.alpha = 0;

        // Example hard-coded values (you can also set these in the Inspector)
        customer1.eyeColor   = "green eyes";
        customer1.hasPiercing = false; // no piercing
        customer1.hasBraces   = true;

        customer2.name = "Carol";
        customer2.age = 63;
        customer2.workplace = "Schoolbus driver";

        customer3.name = "Victor";
        customer3.age = 74;
        customer3.workplace = "Unknown";
    }

    // --------------------------------------------------------
    // GENERATE CUSTOMER (called before the scene starts)
    // --------------------------------------------------------
    public void GenerateNextCustomer()
    {
        CustomerData template;

        // Pick the correct template based on customer #
        if (currentCustomerNumber == 0)      template = customer1;
        else if (currentCustomerNumber == 1) template = customer2;
        else                                 template = customer3;

        // Copy real data
        currentActual.name = template.name;
        currentActual.age = template.age;
        currentActual.workplace = template.workplace;

        // Start with spoken = actual
        currentSpoken.name = currentActual.name;
        currentSpoken.age = currentActual.age;
        currentSpoken.workplace = currentActual.workplace;

        // --------------------------------------------------------
        // CUSTOMER 2 MUST LIE AT LEAST ONCE
        // --------------------------------------------------------
        if (currentCustomerNumber == 1)
        {
            MakeCustomerLie();
            currentTruth = CustomerTruthState.Bad;
        }
        else
        {
            currentTruth = CustomerTruthState.Good;
        }

        currentCustomerNumber++;

        Debug.Log("Generated customer #" + currentCustomerNumber);
        Debug.Log("Actual:  " + currentActual.name + ", " + currentActual.age + ", " + currentActual.workplace);
        Debug.Log("Spoken:  " + currentSpoken.name + ", " + currentSpoken.age + ", " + currentSpoken.workplace);
    }

    // --------------------------------------------------------
    // FORCE CUSTOMER 2 TO LIE AT LEAST ONCE (Name/Work/Age)
    // --------------------------------------------------------
    private void MakeCustomerLie()
    {
        int field = Random.Range(0, 3); // 0 = name, 1 = age, 2 = workplace

        switch (field)
        {
            case 0:
                currentSpoken.name = "Victoria";  // wrong last name
                break;

            case 1:
                currentSpoken.age = currentActual.age + Random.Range(50, 80); // a different age
                break;

            case 2:
                currentSpoken.workplace = "Schoolbus driver"; // definitely wrong workplace
                break;
        }
    }

    public string GetPhoneDescription()
    {
        return $"{currentSpoken.name}, Age {currentSpoken.age}, Works at {currentSpoken.workplace}";
    }

    public string GetActualDescription()
    {
        return $"{currentActual.name}, Age {currentActual.age}, Works at {currentActual.workplace}";
    }


    // ========================== Scene Fade =====================================
    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeIn(sceneName));
    }

    IEnumerator FadeIn(string sceneName)
    {
        float t = 0;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            fadeGroup.alpha = t / fadeTime;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);

        // Now fade out after the scene loads
        yield return StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float t = 0;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            fadeGroup.alpha = 1 - (t / fadeTime);
            yield return null;
        }
    }
}
