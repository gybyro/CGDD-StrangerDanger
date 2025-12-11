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
        CursorAnimator.Instance.Play(GameManager.Instance.hoverCursor);
    }

    void OnMouseExit()
    {

        Debug.Log("STOP CALLED from: " + gameObject.name);
        CursorAnimator.Instance.Stop();
    }


    void OnMouseDown()
    {   
        audioSource.PlayOneShot(clickSound);
    }
}