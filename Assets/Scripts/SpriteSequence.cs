using UnityEngine;

public class SpriteSequence : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;  // drag SpriteRenderer here
    public Sprite[] frames;                // your image sequence
    public float frameRate = 10f;          // frames per second
    public bool loop = true;

    private int currentFrame = 0;
    private float timer = 0f;

    void Update()
    {
        if (frames.Length == 0) return;

        timer += Time.deltaTime;

        if (timer >= 1f / frameRate)
        {
            timer = 0f;

            spriteRenderer.sprite = frames[currentFrame];
            currentFrame++;

            if (currentFrame >= frames.Length)
            {
                if (loop) currentFrame = 0;
                else currentFrame = frames.Length - 1; // stop on last frame
            }
        }
    }
}