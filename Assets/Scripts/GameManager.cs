using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;

public enum CustomerTruthState { Good, Bad }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Settings")]
    public CanvasGroup settingsPanel;
    private bool settingsMenuIsOpen;

    // ------------------- SANITY -------------------
    // [Header("Sanity")]
    // [Range(0, 3)]
    // [SerializeField] private int sanity = 3;

    // public int GetSanity() { return sanity; }

    // public void LoseSanity(int amount = 1)
    // {
    //     sanity = Mathf.Clamp(sanity - amount, 0, 3);
    //     Debug.Log("[GM] Sanity now: " + sanity);
    // }

    // public void ResetSanity()
    // {
    //     sanity = 3;
    //     Debug.Log("[GM] Sanity reset to 3");
    // }
    // ---------------------------------------------

    // car stuff
    public int carPhase = 0;
    private int carTick = 0;
    public bool walkingSceneSpawnByDoor = false;

    // DAY STUFF
    [Header("Skyboxes")]
    public Material skyEvening;   // time = 1
    public Material skyDusk;      // time = 2
    public Material skyMidnight;  // time = 3
    public Material skyMorning;   // time = 0

    private int currentDay;
    private int currentTime = 3;

    private string char_00_nextDialogue = "dial_kyle_01"; // le dudebro
    private string char_01_nextDialogue = "dial_tired_01"; // tired is first character
    private string char_02_nextDialogue = "dial_proxy_01"; // proxy is second
    private string char_03_nextDialogue = "dial_hippie_01";
    private string char_04_nextDialogue = "dial_grumpy_01";
    private string char_05_nextDialogue = "dial_mary_01";
    private string char_06_nextDialogue = "dial_concerned_01";
    private string char_07_nextDialogue = "dial_visitor_01";

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

        // to control daytime skyboxes
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplySkybox();
    }

    // CAR STUFF ==========================================
    public void AdvanceCarPhase()
    {
        carPhase++;
    }

    public void ResetCarPhase()
    {
        carPhase = 0;
    }

    public int GetCarTick() { return carTick; }
    public void AdvanceCarTick() { carTick++; }
    public void ResetCarTick() { carTick = 0; }

    // DAY STUFF ==========================================
    public void SetDay(int index)
    {
        currentDay = index;
        Debug.Log("Current Day set to: " + currentDay);
    }

    public void SetTime(int index)
    {
        currentTime = index;
        ApplySkybox();
        Debug.Log("Current time set to: " + currentTime);
    }

    public int GetDay() { return currentDay; }
    public int GetTime() { return currentTime; }

    public void AdvanceTime()
    {
        if (currentTime >= 3)
        {
            currentDay++;
            currentTime = 0; // skip over day
        }
        else
        {
            currentTime++;
        }

        ApplySkybox();
        Debug.Log("AdvanceTime - Day: " + currentDay + ", time: " + currentTime);
    }

    private void ApplySkybox()
    {
        Material targetSky = null;

        switch (currentTime)
        {
            case 0:
                targetSky = skyMorning;
                break;
            case 1:
                targetSky = skyEvening;
                break;
            case 2:
                targetSky = skyDusk;
                break;
            case 3:
                targetSky = skyMidnight;
                break;
        }

        if (targetSky != null)
        {
            RenderSettings.skybox = targetSky;
            DynamicGI.UpdateEnvironment();
        }

        Debug.Log("ApplySkybox - Day: " + currentDay + ", time: " + currentTime);
    }

    // CHARACTER STUFF =====================================
    public void AdvanceCharDial(int charID, string nextDialogue)
    {
        Debug.Log("AdvanceCharDial Called! index: " + charID + ", nextDialogue: " + nextDialogue);

        if (charID == 0) { char_00_nextDialogue = nextDialogue; }
        else if (charID == 1) { char_01_nextDialogue = nextDialogue; }
        else if (charID == 2) { char_02_nextDialogue = nextDialogue; }
        else if (charID == 3) { char_03_nextDialogue = nextDialogue; }
        else if (charID == 4) { char_04_nextDialogue = nextDialogue; }
        else if (charID == 5) { char_05_nextDialogue = nextDialogue; }
        else if (charID == 6) { char_06_nextDialogue = nextDialogue; }
        else if (charID == 7) { char_07_nextDialogue = nextDialogue; }
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

    public void ResetAllData()
    {
        Debug.Log("GAME MANAGER RESET");

        carPhase = 0;
        carTick = 0;
        walkingSceneSpawnByDoor = false;
        currentDay = 0;
        currentTime = 3;

        char_00_nextDialogue = "dial_kyle_01"; // le dudebro
        char_01_nextDialogue = "dial_tired_01"; // tiered is first character
        char_02_nextDialogue = "dial_proxy_01"; // proxy is seconf 
        char_03_nextDialogue = "dial_hippie_01"; 
        char_04_nextDialogue = "dial_grumpy_01"; //
        char_05_nextDialogue = "dial_mary_01"; //
        char_06_nextDialogue = "dial_concerned_01"; //
        char_07_nextDialogue = "dial_visitor_01"; //
    }

    public void GenerateNextCustomer() { }
    public string GetPhoneDescription() { return ""; }

    public void TogglePlayerSpawn()
    {
        walkingSceneSpawnByDoor = !walkingSceneSpawnByDoor;
    }

    // SETTINGS ====================================================
    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        ToggleSettingsPanel();
    }

    public void ToggleSettingsPanel()
    {
        settingsMenuIsOpen = !settingsMenuIsOpen;

        if (settingsMenuIsOpen)
        {
            settingsPanel.gameObject.SetActive(true);
            settingsPanel.alpha = 1;
            settingsPanel.interactable = true;
            settingsPanel.blocksRaycasts = true;
        }
        else
        {
            settingsPanel.alpha = 0;
            settingsPanel.interactable = false;
            settingsPanel.blocksRaycasts = false;
        }
    }

    // SCENE LOADING STUFFFFF ====================================================
    public string sceneNameToLoad = "MainMenu";
    public CanvasGroup fadeGroup;
    public float fadeTime = 1f;

    private bool isTransitioning;

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(
            string.IsNullOrEmpty(sceneName) ? sceneNameToLoad : sceneName
        );
    }

    public void LoadSceneWithFade(string sceneName)
    {
        if (isTransitioning) return;

        string target = string.IsNullOrEmpty(sceneName)
            ? sceneNameToLoad
            : sceneName;

        FadeToScene(target);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit called! (This won't close the editor)");
    }

    // ========================== Scene Fade =====================================
    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeIn(sceneName));
    }

    IEnumerator FadeIn(string sceneName)
    {
        isTransitioning = true;
        fadeGroup.blocksRaycasts = true;

        float t = 0;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            fadeGroup.alpha = t / fadeTime;
            yield return null;
        }

        fadeGroup.alpha = 1f;

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
            fadeGroup.alpha = 1f - (t / fadeTime);
            yield return null;
        }

        fadeGroup.alpha = 0f;
        fadeGroup.blocksRaycasts = false;
        isTransitioning = false;
    }
}
