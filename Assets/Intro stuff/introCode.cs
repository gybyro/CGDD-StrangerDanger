using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class CarSceneTriggers : MonoBehaviour
{

    [Header("CAMERAS")]
    public Camera mainCamera;
    public AudioSource mainCamSound; // drag same as cam

    [Header("UI")]
    public Canvas introPanel;
    public Animator fadeToBlack;


    [Header("CRT")]
    public CRTController crtController;


    void Awake()
    {
        introPanel.enabled = true;
        crtController.ToggleCRT(true);
    }

    void Start()
    {
        fadeToBlack.SetTrigger("FadeOut");
    }


    // ===================== AUDIO =====================
    private void PlayFromResources(AudioSource cam, string soundName)
    {
        if (string.IsNullOrEmpty(soundName)) return;

        AudioClip clip = Resources.Load<AudioClip>("Sounds/" + soundName);
        if (clip != null)
            cam.PlayOneShot(clip);
    }



    // ===================== OPENING =====================
    public void AcceptWarning() { StartCoroutine(AcceptWarningSequence()); }
    public IEnumerator AcceptWarningSequence()
    {
        PlayFromResources(mainCamSound, "tv-shutdown");
        
        // wait for 1:20 seconds anim
        yield return new WaitForSeconds(1.2f);


        crtController.FadeGoodToHigh(2f);
        yield return new WaitForSeconds(2f);

        introPanel.enabled = false;

        // see again
        crtController.FadeOutCRT(2f);
        yield return new WaitForSeconds(2f); 
        crtController.ToggleCRT(false);



        GameManager.Instance.LoadSceneWithFade("StartingHouseScene");
    }


}