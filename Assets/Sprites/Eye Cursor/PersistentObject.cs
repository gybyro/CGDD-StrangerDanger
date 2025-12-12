using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private static PersistentObject instance;

    void Awake()
    {
        // If one already exists, destroy this duplicate
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        // Keep this canvas alive between scenes
        DontDestroyOnLoad(gameObject);
    }
}
