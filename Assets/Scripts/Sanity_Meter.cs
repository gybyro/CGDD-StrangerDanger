// using UnityEngine;
// using Cinemachine;
// using UnityEngine.SceneManagement;

// public class Sanity_Meter : MonoBehaviour
// {
//     // Singleton
//     public static Sanity_Meter Instance;

//     // Starting sanity
//     public int Player_Sanity = 100;

//     [Header("FOV insanity settings")]
//     [SerializeField] private float fovIncreaseAmount = 10f;   // how much to ADD
//     private bool hasTriggeredInsanityPov = false;

//     // This will be found at runtime in the active scene
//     private CinemachineVirtualCamera sanityVcam;

//     private void Awake()
//     {
//         // Basic singleton
//         if (Instance != null && Instance != this)
//         {
//             Destroy(gameObject);
//             return;
//         }

//         Instance = this;

//         // Keep GameManager + Sanity_Meter when changing scenes
//         DontDestroyOnLoad(gameObject);

//         // Listen for scene changes so we can find the camera in each scene
//         SceneManager.sceneLoaded += OnSceneLoaded;
//     }

//     private void OnDestroy()
//     {
//         if (Instance == this)
//         {
//             SceneManager.sceneLoaded -= OnSceneLoaded;
//         }
//     }

//     // Called every time a new scene loads
//     private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//     {
//         // Try to find a CinemachineVirtualCamera in the newly loaded scene
//         sanityVcam = FindObjectOfType<CinemachineVirtualCamera>();

//         if (sanityVcam != null)
//         {
//             Debug.Log($"Sanity_Meter: Found Cinemachine vcam in scene '{scene.name}': {sanityVcam.name}");
//         }
//         else
//         {
//             Debug.Log($"Sanity_Meter: No Cinemachine vcam found in scene '{scene.name}'. That's fine if it's a 2D scene.");
//         }
//     }

//     // Call this whenever a "bad thing" happens
//     public void Lower_Sanity(int amount)
//     {
//         Player_Sanity -= amount;

//         if (Player_Sanity < 0)
//             Player_Sanity = 0;

//         Debug.Log($"Sanity lowered by {amount}. Current sanity = {Player_Sanity}");

//         // Trigger insanity POV effect only once
//         if (Player_Sanity <= 75 && !hasTriggeredInsanityPov)
//         {
//             hasTriggeredInsanityPov = true;
//             IncreasePOV();
//         }
//     }

//     private void IncreasePOV()
//     {
//         // If we don't have a vcam cached yet, try to find one now
//         if (sanityVcam == null)
//         {
//             sanityVcam = FindObjectOfType<CinemachineVirtualCamera>();
//         }

//         if (sanityVcam == null)
//         {
//             Debug.LogWarning("Sanity_Meter: Cannot change FOV because no CinemachineVirtualCamera exists in this scene.");
//             return;
//         }

//         sanityVcam.m_Lens.FieldOfView += fovIncreaseAmount;
//         Debug.Log("Sanity_Meter: Increased vcam FOV to " + sanityVcam.m_Lens.FieldOfView);
//     }
// }