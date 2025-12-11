using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Cursor/EyeCursorAnimation")]
public class CursorAnimation : ScriptableObject
{
    public Texture2D[] idleFrames;   // frames 0–2
    public Texture2D[] blinkFrames;  // frames 3–6

    public float frameRate = 6f;

    [Header("Blink Settings")]
    public float minBlinkDelay = 3f;   // minimum seconds between blinks
    public float maxBlinkDelay = 7f;   // maximum seconds between blinks

    public Vector2 hotspot = Vector2.zero;
}




public class CursorAnimator : MonoBehaviour
{
    public static CursorAnimator Instance;

    private Coroutine animRoutine;
    private CursorAnimation currentAnim;

    void Awake()
    {
        Instance = this;
    }

    public void Play(CursorAnimation anim)
    {
        Stop(); 
        currentAnim = anim;
        animRoutine = StartCoroutine(RunAnimation(anim));
    }

    public void Stop()
    {
        if (animRoutine != null)
            StopCoroutine(animRoutine);

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        currentAnim = null;
    }

    private IEnumerator RunAnimation(CursorAnimation anim)
    {
        float frameDelay = 1f / anim.frameRate;
        float nextBlinkTime = Time.time + Random.Range(anim.minBlinkDelay, anim.maxBlinkDelay);

        while (true)
        {
            // ----- IDLE LOOP until it's time to blink -----
            while (Time.time < nextBlinkTime)
            {
                for (int i = 0; i < anim.idleFrames.Length; i++)
                {
                    Cursor.SetCursor(anim.idleFrames[i], anim.hotspot, CursorMode.ForceSoftware);
                    yield return new WaitForSeconds(frameDelay);

                    // If blink time arrives mid-idle, break early
                    if (Time.time >= nextBlinkTime)
                        break;
                }
            }

            // ----- BLINK -----
            for (int i = 0; i < anim.blinkFrames.Length; i++)
            {
                Cursor.SetCursor(anim.blinkFrames[i], anim.hotspot, CursorMode.ForceSoftware);
                yield return new WaitForSeconds(frameDelay);
            }

            // ----- Schedule next blink -----
            nextBlinkTime = Time.time + Random.Range(anim.minBlinkDelay, anim.maxBlinkDelay);
        }
    }
}
