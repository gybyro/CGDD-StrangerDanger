using UnityEngine;

public class LookToggle : MonoBehaviour
{
    public Camera playerCamera;
    private SpriteRenderer spriteRenderer;

    private bool isInView = false;        // is object in view THIS frame?
    private bool wasInViewLastFrame = false; // was it in view LAST frame?

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            Debug.LogError("No SpriteRenderer found on this object!");
    }

    void Update()
    {
        Vector3 vp = playerCamera.WorldToViewportPoint(transform.position);

        // Object is considered "in view" if inside the viewport:
        bool inView = vp.z > 0 && vp.x > 0 && vp.x < 1 && vp.y > 0 && vp.y < 1;

        isInView = inView;

        // 👉 Only toggle when object transitions from visible → NOT visible
        if (!isInView && wasInViewLastFrame)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
        }

        wasInViewLastFrame = isInView;
    }
}