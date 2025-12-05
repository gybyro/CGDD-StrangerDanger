using UnityEngine;

public class DelayedEnable : MonoBehaviour
{
    public GameObject objectToEnable;

    private void Start()
    {
        Invoke(nameof(EnableObject), 10f);
    }

    void EnableObject()
    {
        objectToEnable.SetActive(false); // hide object after 10f
    }
}