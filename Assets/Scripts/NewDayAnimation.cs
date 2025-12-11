using UnityEngine;
using System.Collections;

public class NewDayAnimation : MonoBehaviour
{
    [Header("Animators")]
    public Animator datesAnimator;      // Animator for the dates (MON -> TUE)
    public Animator highlightAnimator;  // Animator for the highlight bar

    [Header("Trigger Names")]
    public string monToTueTrigger = "MonToTue";
    public string fadeTextTrigger = "fadeText";   // NEW
    public string highlightFadeTrigger = "fade";

    [Header("Timing")]
    public float waitBeforeMonToTue = 1f;   // time before triggering MonToTue
    public float waitBeforeFade = 2f;      // time after MonToTue before triggering fadeText & fade

    public bool autoPlayOnStart = true;

    void Start()
    {
        if (autoPlayOnStart)
        {
            StartCoroutine(PlaySequence());
        }
    }

    // Call this manually if needed
    public void Play()
    {
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        // -------- 1) MON → TUE --------
        yield return new WaitForSeconds(waitBeforeMonToTue);

        if (datesAnimator != null)
        {
            datesAnimator.ResetTrigger(monToTueTrigger);
            datesAnimator.SetTrigger(monToTueTrigger);
            Debug.Log("NewDayAnimation: Triggered MonToTue");
        }

        // -------- 2) WAIT, then fadeText --------
        yield return new WaitForSeconds(waitBeforeFade);

        if (datesAnimator != null)
        {
            datesAnimator.ResetTrigger(fadeTextTrigger);
            datesAnimator.SetTrigger(fadeTextTrigger);
            Debug.Log("NewDayAnimation: Triggered fadeText");
        }

        // -------- 3) TRIGGER highlight fade --------
        if (highlightAnimator != null)
        {
            highlightAnimator.ResetTrigger(highlightFadeTrigger);
            highlightAnimator.SetTrigger(highlightFadeTrigger);
            Debug.Log("NewDayAnimation: Triggered fade");
        }
    }
}
