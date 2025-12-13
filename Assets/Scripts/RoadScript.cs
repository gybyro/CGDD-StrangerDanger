using UnityEngine;
using System.Collections;   // Needed for IEnumerator & coroutines

public class RoadScript : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;         // Target max speed
    private float currentSpeed = 0f;  // Actual speed used to move

    [Header("Loop Settings")]
    public float resetZ = -20f;
    public float startZ = 40f;

    [Header("Start Behavior")]
    public float accelerationTime = 3f;   // Time to accelerate to full speed
    public float delayBeforeStart = 2f;   // Only used when carPhase > 0

    [Header("Stop Behavior")]
    public float stopDuration = 3f;       // Time to slowly stop after hitting MainCamera

    // Shared between ALL RoadScript instances
    private static bool globalStop = false;

    // Acceleration state
    private bool isAccelerating = false;
    private float accelerationTimer = 0f;

    // Stopping state
    private bool isStopping = false;
    private float stopTimer = 0f;
    private float startStopSpeed = 0f;

    private void Start()
    {
        // Reset stop state each time scene starts
        globalStop = false;
        isStopping = false;
        stopTimer = 0f;

        // Get carPhase from GameManager
        int phase = 0;
        if (GameManager.Instance != null)
        {
            phase = GameManager.Instance.carPhase;
        }
        Debug.Log("[RoadScript] " + name + " using carPhase = " + phase);

        // Start movement behavior based on carPhase
        if (phase == 0)
        {
            currentSpeed = speed;
            isAccelerating = false;
            Debug.Log("[RoadScript] " + name + " Phase 0: start at speed " + speed);
        }
        else
        {
            currentSpeed = 0f;
            isAccelerating = false;
            Debug.Log("[RoadScript] " + name + " Phase > 0: waiting " + delayBeforeStart + "s");
            StartCoroutine(StartAfterDelay());
        }
    }

    private IEnumerator StartAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeStart);

        accelerationTimer = 0f;
        isAccelerating = true;
        Debug.Log("[RoadScript] " + name + " acceleration started");
    }

    private void Update()
    {
        if (globalStop)
        {
            if (!isStopping)
            {
                isStopping = true;
                isAccelerating = false;
                stopTimer = 0f;
                startStopSpeed = currentSpeed;
                Debug.Log("[RoadScript] " + name + " global stop triggered");
            }

            stopTimer += Time.deltaTime;
            float t = Mathf.Clamp01(stopTimer / stopDuration);
            currentSpeed = Mathf.Lerp(startStopSpeed, 0f, t);

            if (t >= 1f)
            {
                currentSpeed = 0f;
            }
        }
        else
        {
            if (isAccelerating)
            {
                accelerationTimer += Time.deltaTime;
                float t = Mathf.Clamp01(accelerationTimer / accelerationTime);
                currentSpeed = Mathf.Lerp(0f, speed, t);

                if (t >= 1f)
                {
                    currentSpeed = speed;
                    isAccelerating = false;
                    // Debug.Log("[RoadScript] " + name + " reached full speed: " + speed);
                }
            }
        }

        // Move road using currentSpeed in WORLD space
        transform.Translate(Vector3.back * currentSpeed * Time.deltaTime, Space.World);

        // Loop road piece along Z
        if (transform.position.z <= resetZ)
        {
            Vector3 pos = transform.position;
            pos.z = startZ;
            transform.position = pos;
            // Debug.Log("[RoadScript] " + name + " looped to z=" + startZ);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            // Debug.Log("[RoadScript] " + name + " hit MainCamera → stop + AdvanceCarPhase");
            globalStop = true;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.AdvanceCarPhase();
            }
        }
    }
}
