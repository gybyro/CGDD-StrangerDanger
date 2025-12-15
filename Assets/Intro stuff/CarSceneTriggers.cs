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

    [Header("Phone Accept -> Next Scene")]
    public string phoneAcceptSceneName = "StartingHouseScene";

    private int currentDay;
    private int currentTime;
    private int carTick;

    private Coroutine bossCallCoroutine;

    // -------------------------
    // SETUP
    // -------------------------
    void Awake()
    {
        if (phone != null)
        {
            phone.OnPhoneCompleted += HandlePhoneCompleted;
            phone.OnDeclineCall += HandleDeclineCall;
        }
    }

    void OnDestroy()
    {
        if (phone != null)
        {
            phone.OnPhoneCompleted -= HandlePhoneCompleted;
            phone.OnDeclineCall -= HandleDeclineCall;
        }
    }

    // -------------------------
    // PHONE EVENTS
    // -------------------------
    private void HandlePhoneCompleted()
    {
        if (mainCamSound != null)
            mainCamSound.Stop();

        if (GameManager.Instance != null)
            GameManager.Instance.LoadSceneWithFade(phoneAcceptSceneName);
    }

    private void HandleDeclineCall()
    {
        if (mainCamSound != null)
            mainCamSound.Stop();
    }

    // -------------------------
    // START
    // -------------------------
    void Start()
    {
        StartCoroutine(RunCarScene());

        carTick = GameManager.Instance.GetCarTick();
        if (carTick != 0)
            crtController.ToggleCRT(false);
    }

    private IEnumerator RunCarScene()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);

        currentDay  = GameManager.Instance.GetDay();
        currentTime = GameManager.Instance.GetTime();
        carTick     = GameManager.Instance.GetCarTick();

        // Decide FIRST vs REPEAT visit for the phone
        bool firstVisit = (currentDay == 1 && currentTime == 0 && carTick == 0);
        if (phone != null)
            phone.SetFirstVisit(firstVisit);

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
    // BOSS CALL
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

        mainCamSound.PlayOneShot(clip);

        if (bossCallCoroutine != null)
            StopCoroutine(bossCallCoroutine);

        bossCallCoroutine = StartCoroutine(AutoDeclineWhenCallEnds(clip.length));
    }

    private IEnumerator AutoDeclineWhenCallEnds(float duration)
    {
        yield return new WaitForSeconds(duration);

        if (phone != null && phone.IsOnCallScreen())
        {
            phone.AutoDeclineCall();
        }
    }

    // -------------------------
    // PHONE BUTTON
    // -------------------------
    public void ClickedOnPhone()
    {
        if (carTick != 0)
            GameManager.Instance.AdvanceTime();

        if (carTick == 0)
        {
            switch (phoneInteracted)
            {
                case 0:
                    PlayBossCallAndAutoDecline();
                    break;
            }
            phoneInteracted++;
        }
        else
        {
            GameManager.Instance.LoadSceneWithFade("WalkingScene");
        }
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
