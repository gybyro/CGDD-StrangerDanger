using UnityEngine;

public class SceneCursorRequest : MonoBehaviour
{
    // PUT THIS AS A COMPONENT ON THE EVENT SYSTEM IN EVERY SCENE, THEN CALL IT    like this =>
    // 3D SCENE:
    // 
    public CursorMode cursorMode;

    private void Start()
    {
        CursorController.Instance.SetSceneCursorMode(cursorMode);
    }
}