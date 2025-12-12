// using System.Collections;
// using UnityEngine;
// using TMPro;
// using UnityEngine.InputSystem;
// using System.Collections.Generic;

// public class CarSceneTriggers : MonoBehaviour
// {

//     [Header("CAMERAS")]
//     public Camera mainCamera;
//     public AudioSource mainCamSound; // drag same as cam
//     public Camera uiCamera;
//     public AudioSource uiCamSound;

//     [Header("UI")]
//     public Canvas introPanel;

//     [Header("References")]
//     public PlayerCharDialogueInCar dialogueManager;
//     public PlayerInput playerInput;
//     public PhoneScript phone;
//     private bool phoneInteracted;

//     [Header("CRT")]
//     public CRTController crtController;


//     private int currentDay;
//     private int currentTime;

//     void Awake()
//     {
//         currentDay = GameManager.Instance.currentDay;
//         currentTime = GameManager.Instance.currentTime;

//         introPanel.enabled = false;
//         UseMainCamera();


//         if (phone != null) phone.OnPhoneCompleted += HandlePhoneCompleted;
//     }
//     void OnDestroy()
//     {
//         if (phone != null)
//             phone.OnPhoneCompleted -= HandlePhoneCompleted;
//     }


//     private void HandlePhoneCompleted()
//     {
//         phoneInteracted = true;
//     }

//     // void Start()
//     // {
//     //     StartCoroutine(RunCarScene());
//     // }

//     // private IEnumerator RunCarScene()
//     // {
//     //     switch (currentDay)
//     //     {
//     //         case 1:  yield return PlayDay01(); break;
//     //         case 2:  GetDaTime(2); break;
//     //         case 3:  GetDaTime(3); break;
//     //         case 4:  GetDaTime(4); break;
//     //         case 5:  GetDaTime(5); break;
//     //         case 6:  GetDaTime(6); break;
//     //         case 7:  GetDaTime(7); break;
//     //     }
//     // }
//     private void GetDaTime(int currentDay)
//     {
//         currentTime = GameManager.Instance.currentTime;
//         // switch (currentTime)
//         // {
//         //     case 0: return currentDay.morning.currentLinesToPlay;
//         //     case 1: return currentDay.eve.currentLinesToPlay;
//         //     case 2: return currentDay.dusk.currentLinesToPlay;
//         //     case 3: return currentDay.midnight.currentLinesToPlay;
//         //     case 4: return currentDay.deep.currentLinesToPlay;
//         // }
        
//     }

//     // ===================== CAMERA =====================
//     public void UseMainCamera()
//     {
//         mainCamera.enabled = true;
//         uiCamera.enabled = false;
//     }

//     public void UseUICamera()
//     {
//         mainCamera.enabled = false;
//         uiCamera.enabled = true;
//     }

//     // ===================== AUDIO =====================
//     private void PlayFromResources(AudioSource cam, string soundName)
//     {
//         if (string.IsNullOrEmpty(soundName)) return;

//         AudioClip clip = Resources.Load<AudioClip>("Sounds/" + soundName);
//         if (clip != null)
//             cam.PlayOneShot(clip);
//     }



//     // ===================== OPENING =====================
//     public void AcceptWarning() { StartCoroutine(AcceptWarningSequence()); }
//     public IEnumerator AcceptWarningSequence()
//     {
//         PlayFromResources(uiCamSound, "tv-shutdown");
        
//         // wait for 1:20 seconds anim
//         yield return new WaitForSeconds(1.2f);


//         crtController.FadeGoodToHigh(2f);
//         yield return new WaitForSeconds(2f);

//         // Switch camera while you cant see because of the CRT
//         UseMainCamera();
//         introPanel.enabled = false;

//         // see again
//         crtController.FadeOutCRT(2f);
//         yield return new WaitForSeconds(2f); 
//         crtController.ToggleCRT(false);


//         // wait for 10s
//         // play phone animation
//         PlayFromResources(mainCamSound, "boss_call");
//         yield return new WaitForSeconds(0.5f);
//         // show phone call close

//         // ---- FIRST DIALOGUE RUN ----
//         PlayerCharDialogueInCar.DialogueStopReason stopReason =
//             PlayerCharDialogueInCar.DialogueStopReason.Ended;

//         yield return StartCoroutine(
//             dialogueManager.RunDialogue(r => stopReason = r)
//         );
//         // ---- PAUSE POINT ----
//         if (stopReason == PlayerCharDialogueInCar.DialogueStopReason.Paused)
//         {
//             phoneInteracted = false;
//             phone.gameObject.SetActive(true);

//             // // Wait until player presses phone UI button
//             yield return new WaitUntil(() => phoneInteracted);

//             phone.gameObject.SetActive(false);

//             // ---- RESUME DIALOGUE ----
//             yield return StartCoroutine(
//                 dialogueManager.RunDialogue(r => stopReason = r)
//             );
//         }


//         // sceneTransition.LoadSceneWithFade("StartingHouseScene");
        
//     }


//     // MONDAY
//     private void PlayDay01()
//     {
        
//         if (currentTime == 0) // day
//         {
//             // CRTController.ToggleCRT(true);
//             // useing CRTPreset goodPreset
//             UseUICamera();
//             introPanel.enabled = true;// Wait for player to press AcceptWarning
//                 // yield return new WaitUntil(() => warningAccepted);
//                 // rest play out on pressing the AcceptWarning button
            
//         }
//             {
                

                
                
//         } 
// }


//     private IEnumerator  PlayDay01()
//     {
//         switch (currentTime)
//         {
//             case 0: // morning of day 01
//                 {
//                     // CRTController.ToggleCRT(true);
//                     // useing CRTPreset goodPreset
//                     UseUICamera();
//                     introPanel.enabled = true;

//                     // Wait for player to press AcceptWarning
//                     // yield return new WaitUntil(() => warningAccepted);
//                     // rest play out on pressing the AcceptWarning button
                    
//                 } 
//             // case 1: return currentDay.eve.currentLinesToPlay;
//             // case 2: return currentDay.dusk.currentLinesToPlay;
//             // case 3: return currentDay.midnight.currentLinesToPlay;
//             // case 4: return currentDay.deep.currentLinesToPlay;
//         }
        
//     }

    

// }

