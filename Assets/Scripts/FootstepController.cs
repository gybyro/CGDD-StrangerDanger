using UnityEngine;
using StarterAssets;

public class FootstepController : MonoBehaviour
{
    [Header("Settings")] // =======================================================
    public float walkStepInterval = 0.8f;   // time between steps (walking)
    public float sprintStepInterval = 0.5f; // time between steps (sprinting)

    [Header("References")] // =====================================================
    public GameObject playerCapsule;  // drag PlayerCapsule once

    private CharacterController cc;
    private StarterAssetsInputs input;
    private FirstPersonController controller;


    [Header("Audio")] // ==========================================================
    public AudioSource audioSource;

    public AudioClip[] grassSteps;
    public AudioClip[] stoneSteps;

    private float stepTimer = 0f;
    private AudioClip[] lastSurface;
    private AudioClip[] currentSurface;
    private Vector3 lastPos;

    void Start()
    {
        cc = playerCapsule.GetComponent<CharacterController>();
        input = playerCapsule.GetComponent<StarterAssetsInputs>();
        controller = playerCapsule.GetComponent<FirstPersonController>();

        lastPos = playerCapsule.transform.position;
    }

    private void Update()
    {
        if (input == null || controller == null) return;

        currentSurface = DetectSurface(); // detect surface FIRST
        if (lastSurface != currentSurface) // reset timer if terrain tag changes
        {
            stepTimer = 0f;
            lastSurface = currentSurface;
        }

        // bool isMoving = input.move.sqrMagnitude > 0.01f;
        bool isMoving = IsActuallyMoving();
        if (isMoving && controller.Grounded)
        {
            stepTimer -= Time.deltaTime;

            float interval = input.sprint ? sprintStepInterval : walkStepInterval;

            if (stepTimer <= 0f)
            {
                PlayFootstep();
                stepTimer = interval;
            }
        }
        else stepTimer = 0f;
    }

    private void PlayFootstep()
    {
        if (currentSurface == null || currentSurface.Length == 0) return;

        AudioClip clip = currentSurface[Random.Range(0, currentSurface.Length)];
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(clip);
    }

    private AudioClip[] DetectSurface()
    {
        Ray ray = new Ray(controller.transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 2f))
        {
            Debug.Log("Hit: " + hit.collider.gameObject.name + " / Tag: " + hit.collider.tag);
            
            if (hit.collider.CompareTag("Grass")) return grassSteps;

            else if (hit.collider.CompareTag("Road")) return stoneSteps;
            
            
        }
        // fallback
        return stoneSteps; 
    }

    private bool IsActuallyMoving()
    {
        Vector3 currentPos = playerCapsule.transform.position;

        // movement distance this frame
        Vector3 delta = currentPos - lastPos;
        delta.y = 0f; // ignore vertical motion

        float distance = delta.magnitude;
        lastPos = currentPos;
        
        return distance > 0.02f; // threshold: 0.01–0.03
    }
}
