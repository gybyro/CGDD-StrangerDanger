using UnityEngine;

public class UnlockCursorOnStart : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;  // free the cursor
        Cursor.visible = true;                   // show the cursor
    }
}