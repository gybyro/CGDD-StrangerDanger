using UnityEngine;

public enum CursorMode
{
    Locked,
    Unlocked
}

public class CursorController : MonoBehaviour
{
    public static CursorController Instance;
    private CursorMode sceneCursorMode;

    private void Awake()
    {
        Instance = this;
    }

    public void SetSceneCursorMode(CursorMode mode)
    {
        sceneCursorMode = mode;
        Apply(mode);
    }
    public void ApplySceneCursorMode()
    {
        Apply(sceneCursorMode);
    }

    public void Apply(CursorMode mode)
    {
        Cursor.lockState = mode == CursorMode.Locked
            ? CursorLockMode.Locked
            : CursorLockMode.None;

        Cursor.visible = mode == CursorMode.Unlocked;
    }
}