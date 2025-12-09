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

    // Shared between ALL RoadPieceLooper instances
    private static bool globalStop = false;

    // Acceleration state
    private bool isAccelerating = false;
    private float accelerationTimer = 0f;

    // Stopping state
    private bool isStopping = false;
    private float stopTimer = 0f;
    private float startStopSpeed = 0f;

    [Header("Objects for each Car Phase")]
    public GameObject[] phaseObjects;

    private void Start()
    {
        // Reset stop state each time scene starts
        globalStop = false;
        isStopping = false;
        stopTimer = 0f;

        // -------------------------
        // Get carPhase from GameManager
        // -------------------------
        int phase = 0;
        if (GameManager.Instance != null)
        {
            phase = GameManager.Instance.carPhase;
        }

        // -------------------------
        // Enable correct phase object
        // -------------------------
        if (phaseObjects != null && phaseObjects.Length > 0)
        {
            // Clamp so we don’t go outside the array
            phase = Mathf.Clamp(phase, 0, phaseObjects.Length - 1);

            // Disable all
            for (int i = 0; i < phaseObjects.Length; i++)
            {
                if (phaseObjects[i] != null)
                    phaseObjects[i].SetActive(false);
            }

            // Enable the correct phase object
            if (phaseObjects[phase] != null)
                phaseObjects[phase].SetActive(true);

            Debug.Log("Enabled Car Phase Object: " + phase);
        }

        // -------------------------
        // Start movement behavior based on carPhase
        // -------------------------
        if (phase == 0)
        {
            // First car scene → start moving immediately
            currentSpeed = speed;
            isAccelerating = false;
        }
        else
        {
            // Later car scenes → wait, then accelerate
            currentSpeed = 0f;
            isAccelerating = false;
            StartCoroutine(StartAfterDelay());
        }
    }

    private IEnumerator StartAfterDelay()
    {
        // Wait before starting motion (for carPhase > 0)
        yield return new WaitForSeconds(delayBeforeStart);

        accelerationTimer = 0f;
        isAccelerating = true;
    }

    private void Update()
    {
        // If global stop triggered by MainCamera collision
        if (globalStop)
        {
            if (!isStopping)
            {
                isStopping = true;
                isAccelerating = false;     // Cancel any acceleration
                stopTimer = 0f;
                startStopSpeed = currentSpeed;
            }

            stopTimer += Time.deltaTime;
            float t = Mathf.Clamp01(stopTimer / stopDuration);
            currentSpeed = Mathf.Lerp(startStopSpeed, 0f, t);

            if (t >= 1f)
            {
                currentSpeed = 0f;
                // We stay stopped; globalStop remains true
            }
        }
        else
        {
            // Only accelerate if we're not in stop mode
            if (isAccelerating)
            {
                accelerationTimer += Time.deltaTime;
                float t = Mathf.Clamp01(accelerationTimer / accelerationTime);
                currentSpeed = Mathf.Lerp(0f, speed, t);

                if (t >= 1f)
                {
                    currentSpeed = speed;
                    isAccelerating = false;
                }
            }
        }

        // Move road using currentSpeed
        transform.Translate(Vector3.back * currentSpeed * Time.deltaTime);

        // Loop road piece
        if (transform.position.z <= resetZ)
        {
            Vector3 pos = transform.position;
            pos.z = startZ;
            transform.position = pos;
        }
    }

    private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("MainCamera"))
    {
        globalStop = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AdvanceCarPhase();
        }
    }
}

}
