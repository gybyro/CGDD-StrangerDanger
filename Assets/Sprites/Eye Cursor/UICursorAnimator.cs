using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UICursorAnimator : MonoBehaviour
{
    public static UICursorAnimator Instance;
    public Image cursorImage;

    public Sprite[] idleFrames;
    public Sprite[] blinkFrames;

    public float frameRate = 12f;
    public float fadeSpeed = 6f;
    public float visibleAlpha = 0.9f;

    private Coroutine animRoutine;
    private bool hovering;

    void Awake()
    {
        Instance = this;
        Cursor.visible = false; // hide system cursor
    }



    void Update()
    {
        // Follow mouse
        transform.position = Input.mousePosition;

        // Smooth fade
        Color c = cursorImage.color;
        float target = hovering ? visibleAlpha : 0f;
        c.a = Mathf.Lerp(c.a, target, Time.deltaTime * fadeSpeed);
        cursorImage.color = c;
    }

    public void OnHoverStart()
    {
        hovering = true;

        if (animRoutine == null)
            animRoutine = StartCoroutine(Animate());
    }

    public void OnHoverEnd()
    {
        hovering = false;
    }

    private IEnumerator Animate()
    {
        int index = 0;
        float nextBlink = Time.time + Random.Range(3f, 7f);

        while (true)
        {
            // Idle loop
            cursorImage.sprite = idleFrames[index];
            index = (index + 1) % idleFrames.Length;
            yield return new WaitForSeconds(1f / frameRate);

            // Blink
            if (Time.time > nextBlink)
            {
                foreach (var s in blinkFrames)
                {
                    cursorImage.sprite = s;
                    yield return new WaitForSeconds(1f / frameRate);
                }

                nextBlink = Time.time + Random.Range(3f, 7f);
            }
        }
    }
}