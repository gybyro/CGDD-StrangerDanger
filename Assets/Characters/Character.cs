using UnityEngine;


// class for assigning names to Sprite Images
[System.Serializable]
public class CharacterExpression
{
    public string name;
    public Sprite sprite;
}


public class Character : MonoBehaviour
{
    [Header("Character Info")] // =======================================================
    public string characterID;      // "john" (matches JSON)
    public string displayName;      // "John" (shown in UI)

    [Header("Animation")] // ============================================================
    public Animator animator; // optional - slide in/out, talk, idle animations

    [Header("Text sounds")] // ================================================================
    public AudioClip[] voiceSounds;   // small clips, 20–200 ms
    public float voicePitchMin = 0.95f;
    public float voicePitchMax = 1.05f;
    [HideInInspector]
    public AudioSource audioSource;


    [Header("Sprite stuff")] // =========================================================
    public SpriteRenderer spriteRenderer;  // drag SpriteRenderer here
    public CharacterExpression[] expressions;

    

    // how to fetch sprites?
        // public Sprite GetSprite(string expressionName)
        // {
        //     foreach (var exp in expressions)
        //         if (exp.name == expressionName)
        //             return exp.sprite;

        //     return null;
        // }



    // ===================================================================================
    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    // ===================================================================================
    // PUBLIC METHODS
    // ===================================================================================

    public void SetExpression(string expressionName)
    {
        Sprite sprite = GetExpressionSprite(expressionName);
        if (sprite == null)
        {
            Debug.LogWarning($"Expression {expressionName} not found on {characterID}");
            return;
        }
        spriteRenderer.sprite = sprite;
    }

    public Sprite GetExpressionSprite(string expressionName)
    {
        foreach (var exp in expressions)
        {
            if (exp.name == expressionName)
                return exp.sprite;
        }
        return null;
    }

    public void Show()
    {
        if (animator != null)
            animator.Play("SlideIn");
        else
            gameObject.SetActive(true);
    }

    public void Hide()
    {
        if (animator != null)
            animator.Play("SlideOut");
        else
            gameObject.SetActive(false);
    }
}

// [Header("Settings")] // =======================================================
//     public float walkStepInterval = 0.8f;   // time between steps (walking)
//     public float sprintStepInterval = 0.5f; // time between steps (sprinting)

//     [Header("References")] // =====================================================
//     public GameObject playerCapsule;  // drag PlayerCapsule once
//     public AudioSource audioSource;

//     [Header("Audio")] // ==========================================================
//     public AudioClip[] grassSteps;
//     public AudioClip[] stoneSteps;

//     private CharacterController cc;
//     private StarterAssetsInputs input;
//     private FirstPersonController controller;

//     private float stepTimer = 0f;
//     private AudioClip[] lastSurface;
//     private AudioClip[] currentSurface;
//     private Vector3 lastPos;

//     void Start()
//     {
//         cc = playerCapsule.GetComponent<CharacterController>();
//         input = playerCapsule.GetComponent<StarterAssetsInputs>();
//         controller = playerCapsule.GetComponent<FirstPersonController>();

//         // lastPos = playerCapsule.transform.position;
//     }