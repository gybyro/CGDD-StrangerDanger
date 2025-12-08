using UnityEngine;

public class RoadPieceLooper : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;         // How fast the road moves on the Z axis

    [Header("Loop Settings")]
    public float resetZ = -20f;       // When Z position is less than this -> teleport
    public float startZ = 40f;        // New Z position after teleport

    [Header("Stop Settings")]
    public float stopDuration = 3f;   // Time (in seconds) to fully stop

    private static bool globalStop = false;   // Shared by ALL RoadPieceLooper objects
    private float stopTimer = 0f;
    private float startSpeed;
    private bool hasSavedSpeed = false;

    void Update()
    {
        // If the global stop has been triggered, all pieces slow down
        if (globalStop)
        {
            if (!hasSavedSpeed)
            {
                startSpeed = speed;   // Save original speed once
                hasSavedSpeed = true;
            }

            stopTimer += Time.deltaTime;
            float t = stopTimer / stopDuration;
            t = Mathf.Clamp01(t);

            speed = Mathf.Lerp(startSpeed, 0f, t);

            if (Mathf.Approximately(speed, 0f))
                speed = 0f;
        }

        // Move backwards
        transform.Translate(Vector3.back * speed * Time.deltaTime);

        // Loop object
        if (transform.position.z <= resetZ)
        {
            Vector3 pos = transform.position;
            pos.z = startZ;
            transform.position = pos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If ANY road piece touches an object tagged "MainCamera"
        if (other.CompareTag("MainCamera"))
        {
            Debug.Log("StopCar");
            globalStop = true;   // Stop ALL road pieces
        }
    }
}
