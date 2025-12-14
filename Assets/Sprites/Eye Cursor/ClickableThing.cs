using UnityEngine;

public class ClickableThing : MonoBehaviour
{
    public AudioClip clickSound;
    public AudioSource audioSource;


    void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.clip = clickSound;
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
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        else
        {
            audioSource.Play();
        }
    }
}