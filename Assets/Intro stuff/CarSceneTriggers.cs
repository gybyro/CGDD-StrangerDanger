using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CarSceneTriggers : MonoBehaviour
{
    [Header("CAMERAS")]
    public Camera mainCamera;
    public AudioSource mainCamSound;

    [Header("References")]
    public PlayerCharDialogueInCar dialogueManager;
    public PlayerInput playerInput;
    public PhoneScript phone;

    public Button phoneBtn;
    private int phoneInteracted = 0;

    [Header("CRT")]
    public CRTController crtController;

    [Header("Phone Accept -> Default Next Scene")]
    public string phoneAcceptSceneName = "StartingHouseScene";

    private int currentDay;
    private int currentTime;
    private int carTick;

    private Coroutine bossCallCoroutine;

    // Scene to load after accept
    private string pendingSceneToLoad = "";

    // Controls first-entry logic
    private bool isFirstCarSceneEntry = false;

    void Awake()
    {
        if (phone != null)
        {
            phone.OnPhoneCompleted += HandlePhoneCompleted;
            phone.OnDeclineCall += HandleDeclineCall;
            phone.OnPhoneOpened += HandlePhoneOpened;
        }
    }

    void OnDestroy()
    {
        if (phone != null)
        {
            phone.OnPhoneCompleted -= HandlePhoneCompleted;
            phone.OnDeclineCall -= HandleDeclineCall;
            phone.OnPhoneOpened -= HandlePhoneOpened;
        }
    }

    private void HandlePhoneCompleted()
    {
        if (mainCamSound != null)
            mainCamSound.Stop();

        string scene = string.IsNullOrEmpty(pendingSceneToLoad)
            ? phoneAcceptSceneName
            : pendingSceneToLoad;

        pendingSceneToLoad = "";

        if (GameManager.Instance != null)
            GameManager.Instance.LoadSceneWithFade(scene);
    }

    private void HandleDeclineCall()
    {
        if (mainCamSound != null)
            mainCamSound.Stop();
    }

    private void HandlePhoneOpened()
    {
        // Boss call only on very first CarScene entry
        if (isFirstCarSceneEntry && phoneInteracted == 0)
        {
            PlayBossCallAndAutoDecline();
            phoneInteracted = 1;
        }
    }

    void Start()
    {
        if (GameManager.Instance != null)
        {
            currentDay  = GameManager.Instance.GetDay();
            currentTime = GameManager.Instance.GetTime();
            carTick     = GameManager.Instance.GetCarTick();

            // Define first time entering CarScene
            isFirstCarSceneEntry = (currentDay == 1 && currentTime == 0 && carTick == 0);

            if (phone != null)
                phone.SetFirstVisit(isFirstCarSceneEntry);

            // Advance time ONLY on repeat entries
            if (!isFirstCarSceneEntry)
                GameManager.Instance.AdvanceTime();

            if (carTick != 0 && crtController != null)
                crtController.ToggleCRT(false);
        }

        StartCoroutine(RunCarScene());
    }

    private IEnumerator RunCarScene()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);

        currentDay  = GameManager.Instance.GetDay();
        currentTime = GameManager.Instance.GetTime();
        carTick     = GameManager.Instance.GetCarTick();

        if (carTick == 4 || carTick == 8 || carTick == 12 || carTick == 16 || carTick == 20)
        {
            GameManager.Instance.AdvanceCarTick();
            GameManager.Instance.LoadSceneWithFade("DoneForTheDay");
            yield break;
        }

        switch (currentDay)
        {
            case 1: yield return PlayMon(); break;
            case 2: yield return PlayTue(); break;
            case 3: yield return PlayWed(); break;
            case 4: yield return PlayThu(); break;
            case 5: yield return PlayFri(); break;
            case 6: yield return PlaySat(); break;
            case 7: yield return PlaySun(); break;
        }
    }

    // -------------------------
    // BOSS CALL + AUTO DECLINE
    // -------------------------
    private void PlayBossCallAndAutoDecline()
    {
        if (mainCamSound == null || phone == null) return;

        AudioClip clip = Resources.Load<AudioClip>("Sounds/boss_call");
        if (clip == null)
        {
            Debug.LogWarning("boss_call not found in Resources/Sounds/");
            return;
        }

        // 🔊 ONLY CHANGE: increased volume (1.5x)
        mainCamSound.PlayOneShot(clip, 3f);

        if (bossCallCoroutine != null)
            StopCoroutine(bossCallCoroutine);

        bossCallCoroutine = StartCoroutine(AutoDeclineWhenCallEnds(clip.length));
    }

    private IEnumerator AutoDeclineWhenCallEnds(float duration)
    {
        yield return new WaitForSeconds(duration);

        if (phone != null && phone.IsOnCallScreen())
            phone.AutoDeclineCall();
    }

    // Optional legacy hook
    public void ClickedOnPhone()
    {
        if (GameManager.Instance == null) return;

        carTick = GameManager.Instance.GetCarTick();

        if (carTick == 0)
            pendingSceneToLoad = phoneAcceptSceneName;
        else
            pendingSceneToLoad = "WalkingScene";
    }

    // -------------------------
    // DAYS
    // -------------------------
    IEnumerator PlayMon()
    {
        switch (carTick)
        {
            case 0: yield return PlayMon01(); break;
            case 1: yield return PlayMon02(); break;
            case 2: yield return PlayMon03(); break;
            case 3: yield return PlayMon04(); break;
            case 4: yield return PlayMon05(); break;
        }
        GameManager.Instance.AdvanceCarTick();
    }

    IEnumerator PlayMon01()
    {
        crtController.FadeOutCRT(2f);
        yield return new WaitForSeconds(2f);
        crtController.ToggleCRT(false);
    }

    IEnumerator PlayMon02()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(dialogueManager.RunDialogue(1));
    }

    IEnumerator PlayMon03()
    {
        StartCoroutine(dialogueManager.RunDialogue(1));
        yield return null;
    }

    IEnumerator PlayMon04()
    {
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator PlayMon05()
    {
        yield return new WaitForSeconds(5f);
        GameManager.Instance.LoadSceneWithFade("WalkingScene");
    }

    IEnumerator PlayTue() { yield return PlayMon02(); GameManager.Instance.AdvanceCarTick(); }
    IEnumerator PlayWed() { yield return PlayMon02(); GameManager.Instance.AdvanceCarTick(); }
    IEnumerator PlayThu() { yield return PlayMon02(); GameManager.Instance.AdvanceCarTick(); }
    IEnumerator PlayFri() { yield return PlayMon02(); GameManager.Instance.AdvanceCarTick(); }
    IEnumerator PlaySat() { yield return PlayMon02(); GameManager.Instance.AdvanceCarTick(); }
    IEnumerator PlaySun() { yield return PlayMon02(); GameManager.Instance.AdvanceCarTick(); }
}
