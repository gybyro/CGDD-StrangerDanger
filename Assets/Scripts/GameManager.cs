using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public enum CustomerTruthState { Good, Bad }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [System.Serializable]
    public class CustomerData
    {
        public string name;
        public int age;
        public string workplace;
    }

    // Fixed customer data
    public CustomerData customer1 = new CustomerData();
    public CustomerData customer2 = new CustomerData();
    public CustomerData customer3 = new CustomerData();

    // Current truth and spoken data
    public CustomerData currentActual = new CustomerData(); // What customer really is
    public CustomerData currentSpoken = new CustomerData(); // What customer will SAY
    public CustomerTruthState currentTruth = CustomerTruthState.Good;

    public int currentCustomerNumber = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            customer1.name = "John Miller";
            customer1.age = 26;
            customer1.workplace = "City Library";

            customer2.name = "Charlie Vega";
            customer2.age = 34;
            customer2.workplace = "Carpentry Shop";

            customer3.name = "Victor Hale";
            customer3.age = 67;
            customer3.workplace = "Unknown";
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // EXAMPLE hardcoded baseline values —
        // You can change these in the Inspector.
    }

    // ----------------------------------------------
    // GENERATE NEW CUSTOMER
    // ----------------------------------------------
    public void GenerateNextCustomer()
    {
        currentCustomerNumber++;

        // Clamp to 1–3
        if (currentCustomerNumber < 1) currentCustomerNumber = 1;
        if (currentCustomerNumber > 3) currentCustomerNumber = 3;

        CustomerData source = null;

        if (currentCustomerNumber == 1)
            source = customer1;
        else if (currentCustomerNumber == 2)
            source = customer2;
        else if (currentCustomerNumber == 3)
            source = customer3;

        // Copy true data
        currentActual.name = source.name;
        currentActual.age = source.age;
        currentActual.workplace = source.workplace;

        // Default spoken = truth
        currentSpoken.name = currentActual.name;
        currentSpoken.age = currentActual.age;
        currentSpoken.workplace = currentActual.workplace;

        // Customer 2 must lie (1 wrong answer)
        if (currentCustomerNumber == 2)
        {
            MakeCustomerLie();
            currentTruth = CustomerTruthState.Bad;
        }
        else
        {
            currentTruth = CustomerTruthState.Good;
        }

        Debug.Log($"GENERATED CUSTOMER {currentCustomerNumber}");
        Debug.Log($"Actual: {GetActualDescription()}");
        Debug.Log($"Spoken: {GetPhoneDescription()}");
    }

    // ----------------------------------------------
    // CUSTOMER 2 LIES ABOUT ONE THING
    // ----------------------------------------------
    private void MakeCustomerLie()
    {
        int f = Random.Range(0, 3);

        switch (f)
        {
            case 0: // name
                currentSpoken.name = "Wrong Name";
                break;

            case 1: // workplace
                currentSpoken.workplace = "Wrong Workplace";
                break;

            case 2: // age
                currentSpoken.age = currentActual.age + Random.Range(3, 15);
                break;
        }
    }

    // ----------------------------------------------
    // DESCRIPTION HELPERS
    // ----------------------------------------------
    public string GetPhoneDescription()
    {
        return $"{currentSpoken.name}, Age {currentSpoken.age}, Works at {currentSpoken.workplace}";
    }

    public string GetActualDescription()
    {
        return $"{currentActual.name}, Age {currentActual.age}, Works at {currentActual.workplace}";
    }

}
