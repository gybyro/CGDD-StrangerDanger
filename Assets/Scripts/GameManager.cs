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
    public CustomerData currentSpoken = new CustomerData(); // What customer will SAY on the phone
    public CustomerTruthState currentTruth = CustomerTruthState.Good;

    // Who we are currently serving (1,2,3)
    public int currentCustomerNumber = 0;

    // Who the phone will load next time
    public int nextCustomerNumber = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Init fixed customers
            customer1.name = "Ryan";
            customer1.age = 26;
            customer1.workplace = "a Restaurant";

            customer2.name = "Carol";
            customer2.age = 55;
            customer2.workplace = "Carpentry Shop";

            customer3.name = "Victor";
            customer3.age = 76;
            customer3.workplace = "Unknown";

            // Start from scratch
            currentCustomerNumber = 0;
            nextCustomerNumber = 1;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // ----------------------------------------------
    // GENERATE NEW CUSTOMER – called ONLY by the phone
    // ----------------------------------------------
    public void GenerateNextCustomer()
    {
        // Decide who is next
        if (nextCustomerNumber < 1) nextCustomerNumber = 1;
        if (nextCustomerNumber > 3) nextCustomerNumber = 3;

        currentCustomerNumber = nextCustomerNumber;

        // Prepare for *next* time
        if (nextCustomerNumber < 3)
            nextCustomerNumber++;

        // Pick source data
        CustomerData source = null;

        if (currentCustomerNumber == 1)
            source = customer1;
        else if (currentCustomerNumber == 2)
            source = customer2;
        else
            source = customer3;

        // Copy true data
        currentActual.name = source.name;
        currentActual.age = source.age;
        currentActual.workplace = source.workplace;

        // Phone shows expected true info
        currentSpoken.name = currentActual.name;
        currentSpoken.age = currentActual.age;
        currentSpoken.workplace = currentActual.workplace;

        // Customer 2 is logically bad, others good
        if (currentCustomerNumber == 2)
            currentTruth = CustomerTruthState.Bad;
        else
            currentTruth = CustomerTruthState.Good;

        Debug.Log($"[GM] GENERATED CUSTOMER current={currentCustomerNumber}, next={nextCustomerNumber}");
        Debug.Log($"[GM] Actual: {GetActualDescription()}");
        Debug.Log($"[GM] Phone:  {GetPhoneDescription()}");
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

    public void ResetAllData()
    {
        currentCustomerNumber = 0;
        nextCustomerNumber = 1;
        currentTruth = CustomerTruthState.Good;

        currentActual = new CustomerData();
        currentSpoken = new CustomerData();

        Debug.Log("GAME MANAGER RESET");
    }


}
