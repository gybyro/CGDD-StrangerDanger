using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public CanvasGroup fadeGroup;
    public float fadeTime = 1f;






    [System.Serializable]
    public class CustomerData
    {
        public string eyeColor;   // "green eyes" / "blue eyes"
        public bool hasPiercing;  // nose piercing
        public bool hasBraces;
    }

    // Actual, fixed info for each customer
    public CustomerData customer1 = new CustomerData();
    public CustomerData customer2 = new CustomerData();
    public CustomerData customer3 = new CustomerData();

    // Who we are currently dealing with
    public CustomerData currentActual = new CustomerData(); // how they really look
    public CustomerData currentPhone  = new CustomerData(); // what the phone says

    private int customerIndex = 0; // 0 = first, 1 = second, 2 = third

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

        customer2.eyeColor   = "blue eyes";
        customer2.hasPiercing = true;
        customer2.hasBraces   = false;

        customer3.eyeColor   = "green eyes";
        customer3.hasPiercing = false;
        customer3.hasBraces   = false;
    }

    /// <summary>
    /// Call this before showing the phone text for each new customer.
    /// 1st: phone matches actual
    /// 2nd: phone is slightly wrong
    /// 3rd: phone matches actual
    /// </summary>
    public void GenerateNextCustomer()
    {
        CustomerData template;

        if (customerIndex == 0)
            template = customer1;
        else if (customerIndex == 1)
            template = customer2;
        else
            template = customer3;

        // Copy template into "actual"
        currentActual.eyeColor   = template.eyeColor;
        currentActual.hasPiercing = template.hasPiercing;
        currentActual.hasBraces   = template.hasBraces;

        // By default, phone info matches actual
        currentPhone.eyeColor   = currentActual.eyeColor;
        currentPhone.hasPiercing = currentActual.hasPiercing;
        currentPhone.hasBraces   = currentActual.hasBraces;

        // If this is the second customer, we make the phone info wrong
        if (customerIndex == 1)
        {
            MakePhoneInfoSlightlyWrong();
        }

        Debug.Log($"Customer #{customerIndex + 1}");
        Debug.Log($"Phone:  {GetPhoneDescription()}");
        Debug.Log($"Actual: {GetActualDescription()}");

        customerIndex++;
    }

    private void MakePhoneInfoSlightlyWrong()
    {
        // Change ONE thing to be wrong
        int fieldToChange = Random.Range(0, 3); // 0 = eyes, 1 = piercing, 2 = braces

        switch (fieldToChange)
        {
            case 0: // eyes
                currentPhone.eyeColor = (currentActual.eyeColor == "green eyes")
                    ? "blue eyes"
                    : "green eyes";
                break;

            case 1: // piercing
                currentPhone.hasPiercing = !currentActual.hasPiercing;
                break;

            case 2: // braces
                currentPhone.hasBraces = !currentActual.hasBraces;
                break;
        }
    }

    public string GetPhoneDescription()
    {
        string piercingText = currentPhone.hasPiercing ? "nose piercing" : "no nose piercing";
        string bracesText   = currentPhone.hasBraces   ? "braces"       : "no braces";

        return $"{currentPhone.eyeColor}, {piercingText}, {bracesText}";
    }

    public string GetActualDescription()
    {
        string piercingText = currentActual.hasPiercing ? "nose piercing" : "no nose piercing";
        string bracesText   = currentActual.hasBraces   ? "braces"        : "no braces";

        return $"{currentActual.eyeColor}, {piercingText}, {bracesText}";
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
