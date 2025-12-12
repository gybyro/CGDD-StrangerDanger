using UnityEngine;

public class ClickableThing : MonoBehaviour
{
    public AudioClip clickSound;
    public AudioSource audioSource;

    void Start()
    {
        if (clickSound != null)
        {
            if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;   
        }
    }

    void OnMouseEnter()
    {
        UICursorAnimator.Instance.OnHoverStart();
    }

    void OnMouseExit()
    {
        UICursorAnimator.Instance.OnHoverEnd();
    }


    void OnMouseDown()
    {   
        audioSource.PlayOneShot(clickSound);
    }
}