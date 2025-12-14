using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.UI;

public class CarSceneTriggers : MonoBehaviour
{

    [Header("CAMERAS")]
    public Camera mainCamera;
    public AudioSource mainCamSound; // drag same as cam

    [Header("UI")]


    [Header("References")]
    public PlayerCharDialogueInCar dialogueManager;
    public PlayerInput playerInput;
    public PhoneScript phone;
    
    public Button phoneBtn;
    private int phoneInteracted = 0;

    [Header("CRT")]
    public CRTController crtController;
    // private bool isTransitioning = false;

    private int currentDay;
    private int currentTime;
    private int carTick;

    void Awake()
    {
        if (phone != null) phone.OnPhoneCompleted += HandlePhoneCompleted;
    }
    void OnDestroy()
    {
        if (phone != null)
            phone.OnPhoneCompleted -= HandlePhoneCompleted;
    }
    private void HandlePhoneCompleted()
    {
        // phoneInteracted = true;
    }

    void Start()
    {
        StartCoroutine(RunCarScene());
        carTick = GameManager.Instance.GetCarTick();
        if (carTick != 0) crtController.ToggleCRT(false);
    }

    private IEnumerator RunCarScene()
    {
        // Wait until GameManager exists
        yield return new WaitUntil(() => GameManager.Instance != null);

        // Now it's safe
        currentDay  = GameManager.Instance.GetDay();
        currentTime = GameManager.Instance.GetTime();
        carTick = GameManager.Instance.GetCarTick();


        Debug.Log("It is  CarTick:  " + GameManager.Instance.GetCarTick() + ",  curr Time: " + currentTime);
        if (carTick == 4 || carTick == 8 || carTick == 12 || carTick == 16 || carTick == 20) {
            

            GameManager.Instance.AdvanceCarTick();
            GameManager.Instance.LoadSceneWithFade("DoneForTheDay");
        }
        else

        switch (currentDay)
        {
            case 1:  yield return PlayMon(); break;
            case 2:  yield return PlayTue(); break;
            case 3:  yield return PlayWed(); break;
            case 4:  yield return PlayThu(); break;
            case 5:  yield return PlayFri(); break;
            case 6:  yield return PlaySat(); break;
            case 7:  yield return PlaySun(); break;
        }
    }

    // private void GetDaTime(int currentDay)
    // {
    //     currentTime = GameManager.Instance.currentTime;
    //     // switch (currentTime)
    //     // {
    //     //     case 0: return currentDay.morning.currentLinesToPlay;
    //     //     case 1: return currentDay.eve.currentLinesToPlay;
    //     //     case 2: return currentDay.dusk.currentLinesToPlay;
    //     //     case 3: return currentDay.midnight.currentLinesToPlay;
    //     //     case 4: return currentDay.deep.currentLinesToPlay;
    //     // }
        
    // }

    // ===================== CAMERA =====================
    // public void UseMainCamera()
    // {
    //     mainCamera.enabled = true;
    //     uiCamera.enabled = false;
    // }

    // public void UseUICamera()
    // {
    //     mainCamera.enabled = false;
    //     uiCamera.enabled = true;
    // }

    // ===================== AUDIO =====================
    private void PlayFromResources(AudioSource cam, string soundName)
    {
        if (string.IsNullOrEmpty(soundName)) return;

        AudioClip clip = Resources.Load<AudioClip>("Sounds/" + soundName);
        if (clip != null)
            cam.PlayOneShot(clip);
    }
    private void PlayLoop(AudioSource source, string soundName)
    {
        if (string.IsNullOrEmpty(soundName)) return;

        AudioClip clip = Resources.Load<AudioClip>("Sounds/" + soundName);
        if (clip != null)
        {
            source.clip = clip;
            source.loop = true;
            source.Play();
        }
    }



    // ===================== DAYSSS =====================
    // 
    // MONDAY
    IEnumerator PlayMon()
    {
        Debug.Log("PLAYING MONDAY IN CARR");
        switch (carTick)
        {
            case 0:  yield return PlayMon01(); break; // morning
            case 1:  yield return PlayMon02(); break; // evening
            case 2:  yield return PlayMon03(); break; // dusk
            case 3:  yield return PlayMon04(); break; // midnight
            case 4:  yield return PlayMon05(); break; // creepy
        }
        GameManager.Instance.AdvanceCarTick();
        Debug.Log("AdvanceD CarTick:  " + GameManager.Instance.GetCarTick());

    }

    public void ClickedOnPhone()
    {
        if (carTick != 0) GameManager.Instance.AdvanceTime();
        if (carTick == 0)
        {
            switch (phoneInteracted)
            {
                case 0:
                    PlayFromResources(mainCamSound, "boss_call"); 
                    break; 
                case 1:
                    mainCamSound.Stop();
                    break;
                case 2:
                    StartCoroutine(dialogueManager.RunDialogue(1));
                    break;
                case 3:
                    StartCoroutine(dialogueManager.RunDialogue(2));
                    break;
                case 4:
                    GameManager.Instance.LoadSceneWithFade("StartingHouseScene");
                    break;
            }
            phoneInteracted++;
        }
        else if (carTick == 1) { 
            switch (phoneInteracted)
            {
                case 0:
                    StartCoroutine(dialogueManager.RunDialogue(2));
                    break; 
                case 1:
                    mainCamSound.Stop();
                    break;
                
            }
            phoneInteracted++;
            
            GameManager.Instance.LoadSceneWithFade("WalkingScene");
        }
        else if (carTick == 2) { 
            
            GameManager.Instance.LoadSceneWithFade("WalkingScene");
        }
        else if (carTick == 3) { 
            
            GameManager.Instance.LoadSceneWithFade("WalkingScene");
        }
        else if (carTick == 5) { 
            
            GameManager.Instance.LoadSceneWithFade("WalkingScene");
        }
        else GameManager.Instance.LoadSceneWithFade("WalkingScene");

    }
    IEnumerator PlayMon01()
    {
        // CRTController.ToggleCRT(true);
        // useing CRTPreset goodPreset
        crtController.FadeOutCRT(2f);
        yield return new WaitForSeconds(2f);
        crtController.ToggleCRT(false);

        Debug.Log("PLAYING MONDAY  0000000000000000001 IN CARR");
        
        // PlayFromResources(mainCamSound, "boss_call");
        // yield return new WaitForSeconds(60.3f);



        // wait for 10s
        // play phone animation

        // StartCoroutine(dialogueManager.RunDialogue(1));
        // StartCoroutine(dialogueManager.RunDialogue(2));
        // StartCoroutine(dialogueManager.RunDialogue(3));
        // GameManager.Instance.LoadSceneWithFade("StartingHouseScene");

        // ---- FIRST DIALOGUE RUN ----
        // PlayerCharDialogueInCar.DialogueStopReason stopReason =
        //     PlayerCharDialogueInCar.DialogueStopReason.Ended;

        // yield return StartCoroutine(
        //     dialogueManager.RunDialogue(r => stopReason = r)
        // );
        // // ---- PAUSE POINT ----
        // if (stopReason == PlayerCharDialogueInCar.DialogueStopReason.Paused)
        // {
        //     phoneInteracted = false;
        //     phone.gameObject.SetActive(true);

        //     // // Wait until player presses phone UI button
        //     yield return new WaitUntil(() => phoneInteracted);

        //     phone.gameObject.SetActive(false);

        //     // ---- RESUME DIALOGUE ----
        //     yield return StartCoroutine(
        //         dialogueManager.RunDialogue(r => stopReason = r)
        //     );
        // }


        // sceneTransition.LoadSceneWithFade("StartingHouseScene");
    }
    IEnumerator PlayMon02()
    {  
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(dialogueManager.RunDialogue(1));
        

    }
    IEnumerator PlayMon03()
    {
        StartCoroutine(dialogueManager.RunDialogue(1));
        yield return new WaitForSeconds(0);

    }
    IEnumerator PlayMon04()
    {
       
        yield return new WaitForSeconds(0.5f);
        

    }
    IEnumerator PlayMon05()
    {
       
        yield return new WaitForSeconds(5f);
        GameManager.Instance.LoadSceneWithFade("WalkingScene");
        // GameManager.Instance.ResetCarTick();
    }




    
    IEnumerator PlayTue()
    {
        switch (carTick)
        {
            case 5:  yield return PlayMon02(); break; // evening
            case 6:  yield return PlayMon03(); break; // dusk
            case 7:  yield return PlayMon04(); break; // midnight
        }
        GameManager.Instance.AdvanceCarTick();
    }

    IEnumerator PlayWed()
    {
        switch (carTick)
        {
            case 8:  yield return PlayMon02(); break; // evening
            case 9:  yield return PlayMon03(); break; // dusk
            case 10:  yield return PlayMon04(); break; // midnight
        }
        GameManager.Instance.AdvanceCarTick();

    }
    IEnumerator PlayThu()
    {
        switch (carTick)
        {
            case 11:  yield return PlayMon02(); break; // evening
            case 12:  yield return PlayMon03(); break; // dusk
            case 13:  yield return PlayMon04(); break; // midnight
        }
        GameManager.Instance.AdvanceCarTick();

    }
    
    
    IEnumerator PlayFri()
    {
        switch (carTick)
        {
            case 14:  yield return PlayMon02(); break; // evening
            case 15:  yield return PlayMon03(); break; // dusk
            case 16:  yield return PlayMon04(); break; // midnight
        }
        GameManager.Instance.AdvanceCarTick();

    }
    IEnumerator PlaySat()
    {
        switch (carTick)
        {
            case 1:  yield return PlayMon02(); break; // evening
            case 2:  yield return PlayMon03(); break; // dusk
            case 3:  yield return PlayMon04(); break; // midnight
            case 4:  yield return PlayMon05(); break; // creepy
        }
        GameManager.Instance.AdvanceCarTick();

    }
    IEnumerator PlaySun()
    {
        switch (carTick)
        {
            case 1:  yield return PlayMon02(); break; // evening
            case 2:  yield return PlayMon03(); break; // dusk
            case 3:  yield return PlayMon04(); break; // midnight
            case 4:  yield return PlayMon05(); break; // creepy
        }
        GameManager.Instance.AdvanceCarTick();

    }

    

}

