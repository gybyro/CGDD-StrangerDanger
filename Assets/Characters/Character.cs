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
            animator.SetTrigger("DoorOpen");
        else
        {
            Debug.LogWarning("where is my animation???");
            gameObject.SetActive(true);
        }
            
    }

    public void Hide()
    {
        if (animator != null)
            animator.SetTrigger("DoorClose");
        else
            gameObject.SetActive(false);
    }
}
