using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;   // <- THIS is important

public class HeadBopScript : MonoBehaviour
{
    [Header("Bob settings")]
    [Range(0f, 0.5f)]
    public float Amount = 0.05f;
    [Range(1f, 30f)]
    public float Frequancy = 10.0f;
    [Range(0f, 20f)]
    public float Smooth = 10.0f;

    public int rotationSpeed = 180;
    public int speed = 0;

    public bool playerIsWalking = false;

    // Instead of Input.GetKey we use this:
    [Header("References")]
    public StarterAssetsInputs playerInput;   // drag PlayerCapsule here

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        if (playerInput == null)
        {
            // If you see this in the console, you forgot to assign PlayerCapsule
            //Debug.LogWarning("HeadBopScript: playerInput is NOT assigned!");
            return;
        }

        // ---- OLD 'is moving' check, but using new input ----
        Vector2 move = playerInput.move;
        bool isMoving = move.sqrMagnitude > 0.01f;

        if (isMoving)
        {
            playerIsWalking = true;

            // your “walking” feel
            Amount = 0.05f;
            Frequancy = 10.0f;
            Smooth = 10.0f;
        }
        else
        {
            playerIsWalking = false;

            // your “idle” feel
            Amount = 0.02f;
            Frequancy = 3.0f;
            Smooth = 5.0f;
        }

        // ---- Sprint (same idea as old LeftShift, but new system) ----
        if (playerInput.sprint && isMoving)
        {
            Frequancy = 20.0f;
        }

        ApplyHeadBop();
    }

    private void ApplyHeadBop()
    {
        float sin = Mathf.Sin(Time.time * Frequancy);
        float cos = Mathf.Cos(Time.time * (Frequancy / 2f));

        Vector3 offset = Vector3.zero;
        offset.y = sin * Amount * 1.4f;
        offset.x = cos * Amount * 1.6f;

        Vector3 targetPos = startPos + offset;

        // smooth movement, like your old Smooth variable
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            targetPos,
            Smooth * Time.deltaTime
        );
    }

    private void StopHeadBop()
    {
        // you can snap back if you ever need to:
        // transform.localPosition = startPos;
    }
}
