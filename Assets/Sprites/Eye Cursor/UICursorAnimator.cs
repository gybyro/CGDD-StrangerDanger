using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UICursorAnimator : MonoBehaviour
{
    public static UICursorAnimator Instance;

    [Header("Cursor UI")]
    public Image cursorImage;

    [Header("Animation Frames")]
    public Sprite[] idleFrames;     // looping frames
    public Sprite[] blinkFrames;    // blink sequence

    [Header("Settings")]
    public float frameRate = 12f;
    public float fadeSpeed = 6f;
    public float visibleAlpha = 0.9f;

    private Coroutine animRoutine;
    private bool hovering;
    private bool animating;

    void Awake()
    {
        Instance = this;
        Cursor.visible = true;  // keep system cursor visible
    }

    void Update()
    {
        // Follow mouse position
        transform.position = Input.mousePosition;

        // Fade in/out smoothly
        Color c = cursorImage.color;
        float targetAlpha = hovering ? visibleAlpha : 0f;
        c.a = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime * fadeSpeed);
        cursorImage.color = c;
    }

    //------------------------------------------------------
    // Called by interactables
    //------------------------------------------------------

    public void OnHoverStart()
    {
        hovering = true;

        if (!animating)
        {
            animRoutine = StartCoroutine(Animate());
        }
    }

    public void OnHoverEnd()
    {
        hovering = false;
    }

    //------------------------------------------------------
    // Animation Logic
    //------------------------------------------------------

    private IEnumerator Animate()
    {
        animating = true;

        float frameDelay = 1f / frameRate;
        int idleIndex = 0;
        float nextBlink = Time.time + Random.Range(3f, 7f);

        while (hovering) // <-- stops immediately when hover ends
        {
            // ----- Idle Frame -----
            cursorImage.sprite = idleFrames[idleIndex];
            idleIndex = (idleIndex + 1) % idleFrames.Length;

            yield return new WaitForSeconds(frameDelay);

            // ----- Blink Trigger -----
            if (Time.time >= nextBlink)
            {
                foreach (Sprite s in blinkFrames)
                {
                    cursorImage.sprite = s;
                    yield return new WaitForSeconds(frameDelay);
                }

                nextBlink = Time.time + Random.Range(3f, 7f);
            }
        }

        animating = false;
    }
}