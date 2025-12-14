using UnityEngine;
using UnityEngine.UI;

public class CursorFadeController : MonoBehaviour
{
    public static CursorFadeController Instance;

    [Header("Cursor Images")]
    public Image defaultCursor;
    public Image eyeCursor;

    [Header("Fade Settings")]
    public float eyefadeSpeed = 6f;
    public float deffadeSpeed = 10f;
    public float eyeMaxAlpha = 1;

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

        // Fade logic
        float targetEye = hovering ? eyeMaxAlpha : 0f;
        float targetDefault = hovering ? 0f : 1f;

        Color eye = eyeCursor.color;
        Color def = defaultCursor.color;

        eye.a = Mathf.Lerp(eye.a, targetEye, Time.deltaTime * eyefadeSpeed);
        def.a = Mathf.Lerp(def.a, targetDefault, Time.deltaTime * deffadeSpeed);

        eyeCursor.color = eye;
        defaultCursor.color = def;
    }

    public void OnHoverStart()
    {
        hovering = true;
    }

    public void OnHoverEnd()
    {
        hovering = false;
    }
}