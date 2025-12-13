using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class IntroCode : MonoBehaviour
{

    [Header("CAMERAS")]
    public Camera mainCamera;
    public AudioSource mainCamSound; // drag same as cam

    [Header("UI")]
    public Canvas introPanel;
    public CanvasGroup fadeGroup;
    public float fadeTime = 1f;
    public Animator bgPannel;


    [Header("CRT")]
    public CRTController crtController;
    private bool isTransitioning = false;


    void Awake()
    {
        introPanel.gameObject.SetActive(true);
        fadeGroup.gameObject.SetActive(true);
        fadeGroup.alpha = 1f;                // IMPORTANT: start fully visible
        fadeGroup.blocksRaycasts = true;
        // crtController.ToggleCRT(true);
    }

    void Start()
    {
        PlayLoop(mainCamSound, "CRTTV_static_delay");
        StartCoroutine(FadeOutRoutine());
        StartCoroutine(SetTimes());
    }

    IEnumerator SetTimes()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);
        GameManager.Instance.SetDay(1);
        GameManager.Instance.SetTime(0);
    }


    // im fadeeed
    IEnumerator FadeOutRoutine()
    {
        isTransitioning = true;

        float t = 0f;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            fadeGroup.alpha = 1f - (t / fadeTime);
            yield return null;
        }
        fadeGroup.alpha = 0f;
        fadeGroup.blocksRaycasts = false;
        isTransitioning = false;

        //yield return new WaitForSeconds(0.1f);
        
    }


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



    // ===================== OPENING =====================
    public void AcceptWarning() { 
        if (!isTransitioning) StartCoroutine(AcceptWarningSequence()); }

    public IEnumerator AcceptWarningSequence()
    {

        isTransitioning = true;
        
        mainCamSound.Stop();
        PlayFromResources(mainCamSound, "tv-shutdown");
        
        // wait for 1:20 seconds anim
        yield return new WaitForSeconds(1.2f);
        bgPannel.SetTrigger("moreFades");


        crtController.FadeGoodToHigh(2f);
        //yield return new WaitForSeconds(2f);

        introPanel.gameObject.SetActive(false);

        // see again
        // crtController.FadeOutCRT(2f);
        yield return new WaitForSeconds(2f); 
        // crtController.ToggleCRT(false);



        GameManager.Instance.LoadScene("CarScene");
    }


}